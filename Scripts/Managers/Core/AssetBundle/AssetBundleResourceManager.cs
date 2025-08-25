using Newtonsoft.Json.Linq;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

namespace SongLib
{
    public class AssetBundleResourceManager : SingletonWithMono<AssetBundleResourceManager>
    {
        protected AssetBundleResource<GameObject> unitPrefabResource = new AssetBundleResource<GameObject>("hero, monster");
        protected AssetBundleResource<GameObject> effectPrefabResource = new AssetBundleResource<GameObject>("effect");
        protected AssetBundleResource<AudioClip> audioPrefabResource = new AssetBundleResource<AudioClip>("sound");
        protected AssetBundleSceneResource sceneResource = new AssetBundleSceneResource();

        private bool _isAssetBundleDownloaded = false;
        public static string AssetBundleURL = "";

        public virtual IEnumerator DownloadAssetBundle(Action<float> onLoadProgress, Action onLoadFail)
        {
            if (!_isAssetBundleDownloaded)
            {
                string assetBundleInfoText = "";
                using (UnityWebRequest request = UnityWebRequest.Get(AssetBundleURL + "assetbundle.info"))
                {
                    yield return request.SendWebRequest();
                    assetBundleInfoText = request.downloadHandler.text;
                }

                AssetBundleData assetBundleData = new AssetBundleData();
                assetBundleData.StartJsonDecode(assetBundleInfoText);
                int assetCount = assetBundleData.abInfos.Count;
                float progressValue = 100f / assetCount;
                for (int i = 0; i < assetCount; ++i)
                {
                    using (UnityWebRequest request = UnityWebRequestAssetBundle.GetAssetBundle(
                               AssetBundleURL + assetBundleData.abInfos[i].AssetBundleName,
                               Hash128.Parse(assetBundleData.abInfos[i].hash)))
                    {
                        request.SendWebRequest();
                        while (!request.isDone)
                        {
                            onLoadProgress?.Invoke(progressValue * i + progressValue * request.downloadProgress);
                            yield return null;
                        }

                        if (request.result == UnityWebRequest.Result.ConnectionError ||
                            request.result == UnityWebRequest.Result.ProtocolError)
                        {
                            DebugHelper.LogError(EDebugType.Info, "Connection Error " + request.error);
                            onLoadFail?.Invoke();
                            yield break;
                        }
                        else
                        {
                            AssetBundle assetBundle = DownloadHandlerAssetBundle.GetContent(request);
                            unitPrefabResource.SetInfo(assetBundleData.abInfos[i]);
                            effectPrefabResource.SetInfo(assetBundleData.abInfos[i]);
                            audioPrefabResource.SetInfo(assetBundleData.abInfos[i]);
                            sceneResource.SetInfo(assetBundleData.abInfos[i]);
                            assetBundle.Unload(true);
                        }
                    }
                }

                yield return StartCoroutine(audioPrefabResource.LoadAssetBundle("sound/"));
                yield return StartCoroutine(effectPrefabResource.LoadAssetBundle("effect/fx_ui"));
                yield return StartCoroutine(effectPrefabResource.LoadAssetBundle("effect/fx_environment"));
                _isAssetBundleDownloaded = true;
            }
        }

        /// 에셋 번들은 사용전에 미리 로딩을 해놓자. 그래야 인게임이 원활하게 돌아간다.
        /// UnLoad는 해야 하는 것과 하지 말아야 하는 것으로 나누자.
        public IEnumerator LoadScene(string sceneName)
        {
            yield return StartCoroutine(sceneResource.LoadScene(sceneName));
        }

        public IEnumerator LoadUnitAssetBundle(string fileName)
        {
            yield return StartCoroutine(unitPrefabResource.LoadAsset(fileName));
        }

        public IEnumerator LoadEffectAssetBundle(string fileName)
        {
            yield return StartCoroutine(effectPrefabResource.LoadAsset(fileName));
        }

        public IEnumerator LoadAudioAssetBundle(string fileName)
        {
            yield return StartCoroutine(audioPrefabResource.LoadAsset(fileName));
        }

        public void UnloadUnitAssetBundle(string fileName) => unitPrefabResource.UnloadAsset(fileName);

        public void UnloadEffectAssetBundle(string fileName) => effectPrefabResource.UnloadAsset(fileName);

        public void UnloadAudioAssetBundle(string fileName) => audioPrefabResource.UnloadAsset(fileName);

        public void UnusedUnloadAssetBundle() => unitPrefabResource.UnusedUnloadAssetBundle();

        public GameObject GetUnitPrefab(string fileName) => unitPrefabResource.GetResource(fileName);

        public GameObject GetEffectPrefab(string fileName) => effectPrefabResource.GetResource(fileName);

        public AudioClip GetAudioClip(string fileName) => audioPrefabResource.GetResource(fileName);

        protected class AssetBundleInfo : SerializerJson
        {
            public string AssetBundleName;
            public string hash;
            public uint CRC;
            public List<string> assets = new List<string>();

            protected override JObject JsonEncode() => m_JsonObject;

            protected override void JsonDecode()
            {
                GetJsonValue("AssetBundleName", ref AssetBundleName);
                GetJsonValue("hash", ref hash);
                GetJsonValue("CRC", ref CRC);
                assets.Clear();
                m_TempJsonArray = GetJsonArrayValue("assets");
                if (m_TempJsonArray == null)
                    return;
                for (int index = 0; index < m_TempJsonArray.Count; ++index)
                    assets.Add(m_TempJsonArray[index].ToString());
            }
        }

        protected class AssetBundleData : SerializerJson
        {
            public List<AssetBundleInfo> abInfos = new List<AssetBundleInfo>();

            protected override JObject JsonEncode() => m_JsonObject;

            protected override void JsonDecode()
            {
                abInfos.Clear();
                m_TempJsonArray = GetJsonArrayValue("abInfos");
                if (m_TempJsonArray == null)
                    return;
                for (int index = 0; index < m_TempJsonArray.Count; ++index)
                {
                    AssetBundleInfo assetBundleInfo = new AssetBundleInfo();
                    assetBundleInfo.StartJsonDecode(m_TempJsonArray[index].ToString());
                    abInfos.Add(assetBundleInfo);
                }
            }
        }

        protected abstract class AssetBundleBaseInfo
        {
            protected string assetBundleName;
            protected string hash;
            protected uint crc;
            protected AssetBundle assetBundle = null;

            public abstract bool HasAsset(string assetName);

            public void SetAssetBundle(AssetBundle ab) => assetBundle = ab;

            public string GetAssetBundleName() => assetBundleName;

            public string GetAssetBundleHash() => hash;

            public uint GetAssetBundleCRC() => crc;

            public bool IsLoadedAssetBundle() => assetBundle != null;
        }

        protected class AssetBundleResourceInfo<T> : AssetBundleBaseInfo where T : UnityEngine.Object
        {
            protected Dictionary<string, T> resourceDictionary = new Dictionary<string, T>();

            public void SetInfo(AssetBundleInfo info)
            {
                assetBundleName = info.AssetBundleName;
                hash = info.hash;
                crc = info.CRC;
                int count = info.assets.Count;
                for (int index = 0; index < count; ++index)
                    resourceDictionary.Add(info.assets[index], default(T));
            }

            private T LoadAsset(string assetName)
            {
                if (assetBundle == null)
                    return default(T);
                T obj = assetBundle.LoadAsset<T>(assetName);
                resourceDictionary[assetName] = obj;
                return obj;
            }

            public override bool HasAsset(string assetName)
            {
                return resourceDictionary.ContainsKey(assetName);
            }

            private T FindAsset(string assetName)
            {
                return HasAsset(assetName) ? resourceDictionary[assetName] : default(T);
            }

            public void UnloadAssetBundle(bool unloadAllLoadedObjects)
            {
                if (assetBundle != null)
                    assetBundle.Unload(unloadAllLoadedObjects);
                assetBundle = null;
            }

            public T LoadResource(string assetName)
            {
                T asset = FindAsset(assetName);
                return asset != null ? asset : LoadAsset(assetName);
            }

            public void UnloadAsset(string assetName)
            {
                if (!HasAsset(assetName) || resourceDictionary[assetName] == null)
                    return;
                resourceDictionary[assetName] = default(T);
            }

            public bool IsAllResourceLoadDone()
            {
                foreach (var resource in resourceDictionary)
                {
                    if (resource.Value == null)
                        return false;
                }
                return true;
            }

            public void UnusedUnloadAssetBundle()
            {
                foreach (var resource in resourceDictionary)
                {
                    if (resource.Value != null)
                        return;
                }
                UnloadAssetBundle(true);
            }
        }

        protected class AssetBundleResource<T> where T : UnityEngine.Object
        {
            protected List<string> pathNames;
            protected List<AssetBundleResourceInfo<T>> resourceInfos = new List<AssetBundleResourceInfo<T>>();

            public AssetBundleResource(string pathName)
            {
                pathNames = new List<string>();
                pathNames.ParsingStringSplit(pathName, ',');
            }

            public void SetInfo(AssetBundleInfo info)
            {
                int count = pathNames.Count;
                for (int index = 0; index < count; ++index)
                {
                    if (info.AssetBundleName.IndexOf(pathNames[index] + "/") >= 0)
                    {
                        AssetBundleResourceInfo<T> bundleResourceInfo = new AssetBundleResourceInfo<T>();
                        bundleResourceInfo.SetInfo(info);
                        resourceInfos.Add(bundleResourceInfo);
                    }
                }
            }

            public IEnumerator LoadAsset(string assetName)
            {
                int count = resourceInfos.Count;
                for (int i = 0; i < count; ++i)
                {
                    if (resourceInfos[i].HasAsset(assetName))
                    {
                        if (resourceInfos[i].IsLoadedAssetBundle())
                            break;
                        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(
                                   AssetBundleURL + resourceInfos[i].GetAssetBundleName(),
                                   Hash128.Parse(resourceInfos[i].GetAssetBundleHash())))
                        {
                            yield return uwr.SendWebRequest();
                            resourceInfos[i].SetAssetBundle(DownloadHandlerAssetBundle.GetContent(uwr));
                        }
                        break;
                    }
                }
            }

            public IEnumerator LoadAssetBundle(string assetBundleName)
            {
                int count = resourceInfos.Count;
                for (int i = 0; i < count; ++i)
                {
                    if (resourceInfos[i].GetAssetBundleName().IndexOf(assetBundleName) >= 0 &&
                        !resourceInfos[i].IsLoadedAssetBundle())
                    {
                        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(
                                   AssetBundleURL + resourceInfos[i].GetAssetBundleName(),
                                   Hash128.Parse(resourceInfos[i].GetAssetBundleHash())))
                        {
                            yield return uwr.SendWebRequest();
                            resourceInfos[i].SetAssetBundle(DownloadHandlerAssetBundle.GetContent(uwr));
                        }
                    }
                }
            }

            public void UnloadAsset(string assetName)
            {
                int count = resourceInfos.Count;
                for (int index = 0; index < count; ++index)
                    resourceInfos[index].UnloadAsset(assetName);
            }

            public void UnusedUnloadAssetBundle()
            {
                int count = resourceInfos.Count;
                for (int index = 0; index < count; ++index)
                {
                    resourceInfos[index].UnusedUnloadAssetBundle();
                }
            }

            public T GetResource(string assetName)
            {
                if (string.IsNullOrEmpty(assetName))
                {
                    return default(T);
                }

                int count = resourceInfos.Count;
                for (int index = 0; index < count; ++index)
                {
                    if (resourceInfos[index].HasAsset(assetName))
                    {
                        return resourceInfos[index].LoadResource(assetName);
                    }
                }
                
                return default(T);
            }
        }

        protected class AssetBundleResourceInfo : AssetBundleBaseInfo
        {
            public List<string> sceneNames = new List<string>();

            public void SetInfo(AssetBundleInfo info)
            {
                assetBundleName = info.AssetBundleName;
                hash = info.hash;
                crc = info.CRC;
                int count = info.assets.Count;

                for (int index = 0; index < count; ++index)
                {
                    sceneNames.Add(info.assets[index]);
                }
            }

            public override bool HasAsset(string assetName)
            {
                return sceneNames.Contains(assetName);
            }
        }

        protected class AssetBundleSceneResource
        {
            public List<AssetBundleResourceInfo> sceneResourceInfos = new List<AssetBundleResourceInfo>();

            public void SetInfo(AssetBundleInfo info)
            {
                if (info.AssetBundleName.IndexOf("scene/") < 0)
                {
                    return;
                }
                
                AssetBundleResourceInfo bundleResourceInfo = new AssetBundleResourceInfo();
                bundleResourceInfo.SetInfo(info);
                sceneResourceInfos.Add(bundleResourceInfo);
            }

            public IEnumerator LoadScene(string sceneName)
            {
                int count = sceneResourceInfos.Count;
                for (int i = 0; i < count; ++i)
                {
                    if (sceneResourceInfos[i].HasAsset(sceneName))
                    {
                        if (sceneResourceInfos[i].IsLoadedAssetBundle())
                        {
                            break;
                        }
                        
                        using (UnityWebRequest uwr = UnityWebRequestAssetBundle.GetAssetBundle(
                                   AssetBundleURL + sceneResourceInfos[i].GetAssetBundleName(),
                                   Hash128.Parse(sceneResourceInfos[i].GetAssetBundleHash())))
                        {
                            yield return uwr.SendWebRequest();
                            sceneResourceInfos[i].SetAssetBundle(DownloadHandlerAssetBundle.GetContent(uwr));
                        }
                        
                        break;
                    }
                }
            }
        }
    }
}