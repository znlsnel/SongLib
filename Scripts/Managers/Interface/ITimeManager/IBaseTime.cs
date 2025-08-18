namespace SongLib
{
    public interface IBaseTime
    {
        float BaseDeltaTime { get; }
        float CurrentTime { get; }
        float BaseTimeScale { get; set; }
        void Update(float unscaledDeltaTime);
    }
}