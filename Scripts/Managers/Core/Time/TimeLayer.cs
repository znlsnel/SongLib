namespace SongLib
{
    public enum TimeLayer
    {
        // 📌 전체적인 흐름 (항상 흐름)
        Global, // 무조건 흐르는 시간 (로딩 화면, UI 애니메이션 등)

        // 🎮 실제 플레이에만 해당
        Game, // 게임 로직에 사용되는 일반적인 시간 (정지 가능)

        // 🔁 재생/시뮬레이션 관련
        Replay, // 리플레이/시뮬레이션 시간
        Preview, // 에디터 내 프리뷰 시 시간 (ex. UI, 타임라인 에디터)

        // 🎞 연출, CutScene 등
        Cinematic, // 컷씬/연출용 시간 (종종 Game과 분리되어야 함)

        // 🌠 시각 효과 전용
        Effect, // 파티클 등 연출용 이펙트 (Game이 멈춰도 돌아가야 할 때)
        Tween, // DOTween 같은 트윈 기반 연출 (별도 관리 시)

        // 🎛 UI 및 UX
        UI, // UI 애니메이션 등
        UIInputDelay, // UI 쿨타임, 버튼 딜레이용 시간

        // 🧠 AI 및 외부 시스템
        AI, // AI의 시뮬레이션이나 계산용 시간
        Network, // 서버 기반 시간 계산 (리플레이 시간과 다를 수 있음)

        // 🧪 개발/디버깅
        Debug, // 디버깅용 슬로우모션/프리캠 등
        EditorPreview // 에디터 테스트에만 쓰이는 타임라인
    }
}