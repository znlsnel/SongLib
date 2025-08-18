namespace SongLib
{
    public interface ITimeManager
    {
        public IBaseTime Get(TimeLayer layer);
    }
}