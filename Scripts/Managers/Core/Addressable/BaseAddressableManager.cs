#if SET_ADDRESSABLE
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;


namespace SongLib
{
    public abstract class BaseAddressableManager<U> : SingletonWithMono<U>, IResourceManager, IBaseManager where U : BaseAddressableManager<U>, new()
    {
        // Prefab ============================
        protected GameAddressable<GameObject> resourcePrefab = new GameAddressable<GameObject>();
        
        // UI ============================
        protected GameAddressableUIPopup resourceUIPopup = new GameAddressableUIPopup();
        protected GameAddressable<GameObject> resourceUIPrefab = new GameAddressable<GameObject>();
        
        // Effect ============================
        protected GameAddressable<GameObject> resourceEffect = new GameAddressable<GameObject>();
        
        // Sprite ============================
        protected GameAddressable<Sprite> resourceSprite = new GameAddressable<Sprite>();
        protected GameAddressableSpriteAtlas resourceSpriteAtlas = new GameAddressableSpriteAtlas();
        
        // Audio ============================
        protected GameAddressable<AudioClip> resourceAudio = new GameAddressable<AudioClip>();
        
        // Material ============================
        protected GameAddressable<Material> resourceMaterial = new GameAddressable<Material>();
        
        // TextAsset ============================
        protected GameAddressable<TextAsset> resourceTextAsset = new GameAddressable<TextAsset>();
        
        // TMP_FontAsset ============================
        protected GameAddressable<TMP_FontAsset> resourceFontAsset = new GameAddressable<TMP_FontAsset>();
        
        public bool IsInitialized { get; set; } = false;
        public async void Init()
        {
            await InitResource();
            Global.Init(this);
            
            DebugHelper.Log(DebugTag.Initialization, $"🟢- [ AddressableManager ] Initialize Completed!");
            IsInitialized = true;
        }
        
        private async Awaitable InitResource()
        {
            await resourcePrefab.Init();
            await resourceUIPopup.Init();
            await resourceUIPrefab.Init();
            await resourceEffect.Init();
            await resourceSprite.Init();
            await resourceSpriteAtlas.Init();
            await resourceAudio.Init();
            await resourceMaterial.Init();
            await resourceTextAsset.Init();
            await resourceFontAsset.Init();
        }

        #region << =========== Prefab =========== >>

        public GameObject GetPrefab(string fileName) => resourcePrefab.GetResource(fileName);
        public void DelPrefab(string fileName) => resourcePrefab.DelResource(fileName);

        #endregion

        #region << =========== UI =========== >>

        public GameObject GetUIPopupPrefab(int id) => resourceUIPopup.GetUIPopup(id);
        public void DelUIPopupPrefab(int id) => resourceUIPopup.DelUIPopup(id);

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
#endif
