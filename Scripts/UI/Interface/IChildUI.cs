using SongLib;

public interface IChildUI<T> where T : UIBase
{
    T Parent { get; }
    void Setup(T parent);
}