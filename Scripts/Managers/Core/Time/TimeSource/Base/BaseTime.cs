namespace SongLib
{
    public abstract class BaseTime : IBaseTime
    {
        protected static TimeLayer timeLayer { get; set; }
        protected static IBaseTime cached;

        public static float DeltaTime
        {
            get
            {
                cached ??= Global.Time.Get(timeLayer);
                return cached.BaseDeltaTime;
            }
        }

        public float CurrentTime { get; protected set; } = 0f;
        public float BaseDeltaTime { get; protected set; } = 0f;
        public float BaseTimeScale { get; set; } = 1f;

        public virtual void Update(float unscaledDeltaTime)
        {
            BaseDeltaTime = unscaledDeltaTime * BaseTimeScale;
            CurrentTime += BaseDeltaTime;
        }
    }
}