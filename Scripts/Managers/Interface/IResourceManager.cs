using TMPro;
using UnityEngine;

namespace SongLib
{
    public interface IResourceManager
    {
        // Prefab ============================
        GameObject GetPrefab(string fileName);
        void DelPrefab(string fileName);
        
        // UI ============================
        GameObject GetUIPrefab(string fileName);
        void DelUIPrefab(string fileName);
        GameObject GetUIPopupPrefab(int id);
        void DelUIPopupPrefab(int id);
        
        // Effect ============================
        GameObject GetEffectPrefab(string fileName);
        void DelEffectPrefab(string fileName);
        
        // Sprite ============================
        Sprite GetSprite(string fileName);
        void DelSprite(string fileName);
        Sprite GetSpriteAtlas(string fileName);
        void DelSpriteAtlas(string fileName);
        
        // Audio ============================
        AudioClip GetAudioClip(string fileName);
        void DelAudioClip(string fileName);
        
        // Material ============================
        Material GetMaterial(string fileName);
        void DelMaterial(string fileName);
        
        // TextAsset ============================
        TextAsset GetTextAsset(string fileName);
        void DelTextAsset(string fileName);
        
        // TMP_FontAsset ============================
        TMP_FontAsset GetFontAsset(string fileName);
        void DelFontAsset(string fileName);

        // Scriptable Objects ============================
        ScriptableObject GetSOData(string fileName);
        void DelSOData(string fileName);
    }
}