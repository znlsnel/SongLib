namespace SongLib
{
    public abstract class BaseDebugManager<T> :  Singleton<T>, IDebugManager where T : BaseDebugManager<T>, new()
    {
        #region << =========== SERIALIZED FIELDS =========== >>
        
        protected IDebugAsset _debugAsset;
        
        #endregion
        
        #region << =========== INIT =========== >>

        public void Init()
        { 
            Global.Init(this);
            LoadAsset();
        }

        protected abstract void LoadAsset();
        
        #endregion

        #region << =========== PUBLIC METHODS =========== >>

        public bool IsEnabled() => _debugAsset != null && _debugAsset.IsEnabled;
        public bool IsTagEnabled(IDebugTag tag) => _debugAsset != null && _debugAsset.IsTagEnabled(tag);
        public bool IsTagEnabled(EDebugType type) => _debugAsset != null && _debugAsset.IsTagEnabled(type);
        
        public IDebugAsset GetDebugAsset() => _debugAsset;
        public void SetDebugAsset(IDebugAsset asset) => _debugAsset = asset;

        #endregion
    }
}