## BaseSceneFlowManager 분석 및 사용 가이드

### 개요
- **역할**: 씬 전환(LoadScene)을 관리하고, 선택적 전환 효과(ISceneTransition)와 연동하여 로딩 전/후 페이드 등 연출을 수행하는 추상 매니저.
- **상속 계층**: `BaseSceneFlowManager<U> : SingletonWithMono<U>, IBaseManager`
- **핵심 의존성**:
  - `UnityEngine.SceneManagement.SceneManager`
  - `ISceneTransition`(전환 연출 인터페이스)
  - `Global.Popup` 시스템(전환 시작 전 열린 팝업 강제 종료)

### 공개 API
- `bool IsInitialized { get; set; }`
- `virtual void Init()`
  - 초기화 로그 출력 및 `IsInitialized = true` 설정
- `void LoadScene(string sceneName, int transitionType = -1)`
  - 씬 로드 요청의 단일 진입점
  - `transitionType == -1`이면 전환 연출 없이 직접 로드
  - 그 외에는 `sceneTransitionDict`에서 전환 객체를 조회하거나 `AddPopup(transitionType)`으로 생성

### 내부 동작 흐름
1) `LoadScene(sceneName, transitionType)` 호출
   - 실행 중인 전환 코루틴 정리(`ClearTransitionCoroutine()`)
   - `Global.Popup?.ForceCloseAllOpenPopup()`로 열린 팝업 전부 닫기
   - 전환 타입 결정(기본: 전환 없음, 지정 시: 사전 조회→없으면 `AddPopup` 호출)
   - `StartCoroutine(LoadSceneCoroutine(sceneName))`

2) `LoadSceneCoroutine`
   - 전환 객체가 있으면:
     - `Open()` 호출 후 `StartTransition()` 실행, 완료 콜백을 기다림
   - `SceneManager.LoadSceneAsync(sceneName)` 수행
     - `allowSceneActivation = false`로 설정하고 `progress < 0.9f`까지 대기
     - 진행 로그 출력 후 `allowSceneActivation = true`
   - 전환 객체가 있으면:
     - `WaitForSecondsRealtime(_sceneTransitionStopTime)` 대기
     - `EndTransition()` 완료까지 대기 후 `Close()` 호출
   - 종료 시 내부 코루틴 핸들 비움

### 주요 필드
- `protected readonly Dictionary<int, ISceneTransition> sceneTransitionDict`
  - 전환 타입(ID) → 전환 객체 매핑
- `private ISceneTransition _currentSceneTransition`
- `private Coroutine _sceneTransitionCoroutine`
- `private float _sceneTransitionStopTime = 1f`
  - 씬 활성화 직후 전환 종료 전 잠시 멈추는(연출 유지) 실시간 대기 시간

### 확장 포인트
- `protected abstract ISceneTransition AddPopup(int popupID)`
  - 프로젝트의 팝업/전환 생성 방식에 맞게 구현하여 전환 객체를 반환
  - 구현부에서 `sceneTransitionDict[popupID] = created;` 식으로 캐시 권장

예시(프로젝트 API에 맞춰 수정 필요):
```csharp
protected override ISceneTransition AddPopup(int popupID)
{
    // TODO: 프로젝트의 팝업 생성/획득 방식으로 전환 객체를 준비하세요
    // 예: var popup = Global.Popup.Open<UIPopupTransitionDim>();
    ISceneTransition popup = /* create or fetch transition popup */ null; // 프로젝트별 구현
    if (popup != null)
    {
        sceneTransitionDict[popupID] = popup;
    }
    return popup;
}
```

### 에러/예외 처리 포인트
- `LoadSceneAsync`가 null인 경우 에러 로그 후 중단
- 전환 객체가 없는 경우(기본 전환)에도 문제 없이 동작
- 중복 호출 대비: 새 전환 시작 전 `ClearTransitionCoroutine()`로 기존 코루틴 중단

### 설계 의도 및 특징
- **전환 연출의 선택적 적용**: 기본 전환(-1)과 특정 전환 타입을 분리해 호출부를 단순화
- **비차단 로딩**: `LoadSceneAsync`와 진행률(0.9f까지) 대기, 활성화 시점 제어
- **연출 타이밍 보장**: 씬 활성화 직후 `_sceneTransitionStopTime`만큼 유지 후 페이드 아웃
- **팝업 정리 선행**: 씬 전환 충돌을 방지하기 위해 열린 팝업을 강제 종료

### 연동 예: UIPopupTransitionDim
- `ISceneTransition` 구현체로 페이드 인/아웃을 수행
- `StartTransition`에서 딤 이미지 알파 0→1, `EndTransition`에서 1→0
- `Time.unscaledDeltaTime` 사용으로 타임스케일과 무관한 연출

### 사용 가이드
- 씬 전환 호출부에서:
  - 기본 전환: `LoadScene("GameScene")`
  - 특정 전환: `LoadScene("GameScene", (int)GlobalType.TransitionDim)`
- 커스텀 전환 추가 시:
  - `ISceneTransition` 구현 팝업 제작
  - `BaseSceneFlowManager` 파생 클래스에서 `AddPopup` 구현으로 생성/캐시

### 주의사항
- 전환 타입(ID) 체계는 프로젝트 전역(enum 등)과 일관되게 관리하세요.
- `Global.Popup` 사용 여부 및 API는 프로젝트 버전에 따라 다를 수 있으니, 실제 제공 메서드에 맞춰 `AddPopup`/호출부를 조정하세요.
- 장시간 로딩 처리(네트워크/에셋)는 별도 로딩 UI와 병행 고려가 필요합니다.

### 최소 예시(개념)
```csharp
public sealed class SceneFlowManager : BaseSceneFlowManager<SceneFlowManager>
{
    protected override ISceneTransition AddPopup(int popupID)
    {
        // 프로젝트 팝업 시스템에서 전환 팝업을 생성/획득
        ISceneTransition transition = /* ... */ null;
        if (transition != null)
        {
            sceneTransitionDict[popupID] = transition;
        }
        return transition;
    }
}

// 사용
// 기본 전환 없이 바로 로드
SceneFlowManager.Instance.LoadScene("Lobby");

// 특정 전환(예: Dim 페이드)을 사용해 로드
SceneFlowManager.Instance.LoadScene("Battle", (int)GlobalType.TransitionDim);
```




