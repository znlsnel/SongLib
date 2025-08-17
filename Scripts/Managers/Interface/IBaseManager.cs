
namespace SongLib
{
    public interface IBaseManager
    {
        bool IsInitialized { get; set; }
        void Init();
    }
}