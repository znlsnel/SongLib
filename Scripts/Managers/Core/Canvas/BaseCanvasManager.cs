using UnityEngine;

namespace SongLib
{

    public class BaseCanvasManager<T> : SingletonWithMono<T>, IBaseManager, ICanvasManager where T : BaseCanvasManager<T>
    {
        protected ECanvasType _canvasType = ECanvasType.Height;

        public ECanvasType CanvasType => _canvasType;
        public bool IsInitialized { get; set; }

        protected override void Awake()
        {
            base.Awake();
            IsInitialized = false;
        }

        // TODO:minb:check - 공사중... 향후 다양한 플랫폼의 게임을 지원하기 위해 캔버스 사이즈를 조절하는 기능을 추가할 예정
        public void Init()
        {
            if (IsInitialized) return;
         
            // Global.Init(this);
            switch (_canvasType)
            {
                case ECanvasType.Width:
                    break;
                case ECanvasType.Height:
                    break;
            }
            
            DebugHelper.Log(EDebugType.Init, $"🟢- [ CanvasManager ] Initialize Completed!");
            IsInitialized = true;
        }
    }
}