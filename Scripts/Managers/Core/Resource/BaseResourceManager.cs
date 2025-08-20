using System;
using TMPro;
using UnityEngine;

namespace SongLib
{
    public abstract class BaseResourceManager<U> : SingletonWithMono<U>, IResourceManager, IBaseManager where U : BaseResourceManager<U>, new()
    {
        // Prefab ============================
        protected GameResource<GameObject> resourcePrefab;
        
        // UI ============================
        protected GameResourceUIPopup resourceUIPopup;
        protected GameResource<GameObject> resourceUIPrefab;
        
        // Effect ============================
        protected GameResource<GameObject> resourceEffect;
        
        // Sprite ============================
        protected GameResource<Sprite> resourceSprite;
        protected GameResourceSpriteAtlas resourceSpriteAtlas;
        
        // Audio ============================
        protected GameResource<AudioClip> resourceAudio;
        
        // Material ============================
        protected GameResource<Material> resourceMaterial;
        
        // TextAsset ============================
        protected GameResource<TextAsset> resourceTextAsset;
        
        // TMP_FontAsset ============================
        protected GameResource<TMP_FontAsset> resourceFontAsset;

        public bool IsInitialized { get; set; } = false;

        public void Init()
        {
            NewGameResource();
            InitGameResource();
            Global.Init(this);
            
            DebugHelper.Log(EDebugType.Init, $"🟢 - [ ResourceManager ] Initialize Completed!");
            IsInitialized = true;
        }

        protected virtual void NewGameResource()
        {
            // Prefab ============================
            resourcePrefab = new GameResource<GameObject>("Prefabs, Prefabs/UI, Prefabs/Effect", "prefab");
            
            // UI ============================
            resourceUIPopup = new GameResourceUIPopup("Prefabs/UI/Popup", "prefab");
            resourceUIPrefab = new GameResource<GameObject>("Prefabs/UI/Prefab, Prefabs/UI/Map", "prefab");
            
            // Effect ============================
            resourceEffect = new GameResource<GameObject>("Effect, Effect/Base Game");
            
            // Sprite ============================
            resourceSprite = new GameResource<Sprite>("Sprites");
            resourceSpriteAtlas = new GameResourceSpriteAtlas("SpriteAtlas");
            
            // Audio ============================
            resourceAudio = new GameResource<AudioClip>("Sound, Sound/SFX, Sound/BGM", "ogg, wav");
            
            // Material ============================
            resourceMaterial = new GameResource<Material>("Materials");
            
            // TextAsset ============================
            resourceTextAsset = new GameResource<TextAsset>("TextAsset");
            
            // TMP_FontAsset ============================
            resourceFontAsset = new GameResource<TMP_FontAsset>("FontAsset");
        }

        private void InitGameResource()
        {
            resourceUIPopup.Init();
            resourceSpriteAtlas.Init();
            resourceAudio.AllResourceLoad();
        }

        #region << =========== Prefab =========== >>

        public GameObject GetPrefab(string fileName) => resourcePrefab.GetResource(fileName);
        public void DelPrefab(string fileName) => resourcePrefab.DelResource(fileName);

        #endregion

        #region << =========== UI =========== >>

        public GameObject GetUIPopupPrefab(int id) => resourceUIPopup.GetPopUpUI(id);
        public void DelUIPopupPrefab(int id) => resourceUIPopup.DelPopupUI(id);

        public GameObject GetUIPrefab(string fileName) => resourceUIPrefab.GetResource(fileName);
        public void DelUIPrefab(string fileName) => resourceUIPrefab.DelResource(fileName);

        #endregion

        #region << =========== Effect =========== >>

        public GameObject GetEffectPrefab(string fileName) => resourceEffect.GetResource(fileName);
        public void DelEffectPrefab(string fileName) => resourceEffect.DelResource(fileName);

        #endregion
        
        #region << =========== Sprite =========== >>
        
        public Sprite GetSprite(string fileName) => resourceSprite.GetResource(fileName);
        public void DelSprite(string fileName) => resourceSprite.DelResource(fileName);

        public Sprite GetSpriteAtlas(string fileName) => resourceSpriteAtlas.GetSprite(fileName);
        public void DelSpriteAtlas(string fileName) => resourceSpriteAtlas.DelResource(fileName);
        
        #endregion
        
        #region << =========== Audio =========== >>

        public AudioClip GetAudioClip(string fileName) => resourceAudio.GetResource(fileName);
        public void DelAudioClip(string fileName) => resourceAudio.DelResource(fileName);
        
        #endregion
        
        #region << =========== Material =========== >>
        
        public Material GetMaterial(string fileName) => resourceMaterial.GetResource(fileName);
        public void DelMaterial(string fileName) => resourceMaterial.DelResource(fileName);
        
        #endregion
        
        #region << =========== TextAsset =========== >>
        
        public TextAsset GetTextAsset(string fileName) => resourceTextAsset.GetResource(fileName);
        public void DelTextAsset(string fileName) => resourceTextAsset.DelResource(fileName);
        
        #endregion
        
        #region << =========== TMP_FontAsset =========== >>
        
        public TMP_FontAsset GetFontAsset(string fileName) => resourceFontAsset.GetResource(fileName);
        public void DelFontAsset(string fileName) => resourceFontAsset.DelResource(fileName);
        
        #endregion
    }
}