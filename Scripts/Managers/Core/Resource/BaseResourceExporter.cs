#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace SongLib
{
    public abstract class BaseResourceExporter
    {
        protected abstract string GetBasePath();

        public void ExportAllAudioPaths()
        {
            string basePath = GetBasePath();
            string[] guids = AssetDatabase.FindAssets("t:AudioClip", new[] { $"{basePath}/Sounds" });
            HashSet<string> folders = new HashSet<string>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                string extension = Path.GetExtension(path).ToLower();

                if (extension == ".ogg" || extension == ".wav" || extension == ".mp3")
                {
                    int resourceIdx = path.IndexOf("Resources/");
                    if (resourceIdx != -1)
                    {
                        string relativePath = path.Substring(resourceIdx + "Resources/".Length);
                        string directory = Path.GetDirectoryName(relativePath).Replace("\\", "/");

                        if (!string.IsNullOrEmpty(directory))
                        {
                            folders.Add(directory);
                        }
                    }
                }
            }

            string result = string.Join(",", folders);
            File.WriteAllText("Assets/Resources/audio_folders.txt", result);
            DebugHelper.Log(EDebugType.System, "Audio folders exported with .ogg, .wav, .mp3");
        }
        
        public void ExportAllPrefabPaths()
        {
            string basePath = GetBasePath();
            string[] guids = AssetDatabase.FindAssets("t:Prefab", new[] { $"{basePath}/Prefabs" });
            HashSet<string> folders = new HashSet<string>();

            foreach (string guid in guids)
            {
                string path = AssetDatabase.GUIDToAssetPath(guid);
                int resourceIdx = path.IndexOf("Resources/");
                if (resourceIdx != -1)
                {
                    string relativePath = path.Substring(resourceIdx + "Resources/".Length);
                    string directory = Path.GetDirectoryName(relativePath).Replace("\\", "/");

                    if (!string.IsNullOrEmpty(directory))
                    {
                        folders.Add(directory);
                    }
                }
            }

            string result = string.Join(",", folders);
            File.WriteAllText("Assets/Resources/prefab_folders.txt", result);
            DebugHelper.Log(EDebugType.System, "Prefab folders exported.");
        }
    }
}
#endif