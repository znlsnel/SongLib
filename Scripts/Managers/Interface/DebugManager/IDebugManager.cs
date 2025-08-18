namespace SongLib
{
    public interface IDebugManager
    {
        bool IsTagEnabled(IDebugTag tag);
        bool IsEnabled();
        IDebugAsset GetDebugAsset();
    }
}