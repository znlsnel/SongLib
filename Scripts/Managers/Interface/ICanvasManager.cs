namespace SongLib
{
    public interface ICanvasManager
    {
        ECanvasType CanvasType { get; }
        void Init();
        bool IsInitialized { get; set; }
    }
}   