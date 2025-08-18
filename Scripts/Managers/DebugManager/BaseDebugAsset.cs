using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System;
using Sirenix.OdinInspector;

namespace SongLib
{
    public abstract class BaseDebugAsset<T> : ScriptableObjectSingleton<T>, IDebugAsset where T : BaseDebugAsset<T>
    {
        #region << =========== SERIALIZED FIELDS =========== >>
        
        [Header("Debug Settings")]
        [SerializeField] private bool _isEnabled = true;
        
        [Header("Base Tag Settings")]
        [ListDrawerSettings(
            NumberOfItemsPerPage = 100,
            ShowPaging = true,
            ShowItemCount = true
        )]
        [SerializeField] protected List<DebugTagSetting> _tagSettings = new();
        
        #endregion

        #region << =========== PROPERTIES =========== >>
        
        public bool IsEnabled => _isEnabled;
        public IReadOnlyList<DebugTagSetting> TagSettings => _tagSettings.AsReadOnly();
        
        #endregion

        #region << =========== UNITY LIFECYCLE =========== >>

        [HorizontalGroup("DebugLevelButtons")]
        [Button("Disabled", ButtonSizes.Medium)]
        [GUIColor(0.6f, 0.6f, 0.6f)]
        private void SetAllToDisabled()
        {
            SetAllTagsToLevel(EDebugLevel.Disabled);
        }

        [HorizontalGroup("DebugLevelButtons")]
        [Button("Editor Only", ButtonSizes.Medium)]
        [GUIColor(0.4f, 0.8f, 1.0f)]
        private void SetAllToEditorOnly()
        {
            SetAllTagsToLevel(EDebugLevel.EditorOnly);
        }

        [HorizontalGroup("DebugLevelButtons")]
        [Button("Development", ButtonSizes.Medium)]
        [GUIColor(1.0f, 0.9f, 0.3f)]
        private void SetAllToDevelopment()
        {
            SetAllTagsToLevel(EDebugLevel.Development);
        }

        [HorizontalGroup("DebugLevelButtons")]
        [Button("Always", ButtonSizes.Medium)]
        [GUIColor(1.0f, 0.5f, 0.5f)]
        private void SetAllToAlways()
        {
            SetAllTagsToLevel(EDebugLevel.Always);
        }

        private void SetAllTagsToLevel(EDebugLevel level)
        {
            foreach (var tagSetting in _tagSettings)
            {
                tagSetting.Level = level;
            }
        }

        [Button("Refresh Tag Settings", ButtonSizes.Medium)]
        [GUIColor(0.7f, 1f, 0.7f)]
        private void RefreshTagSettings()
        {
            _tagSettings.Clear();
            
            SetTagSettings();
        }
        
        #endregion

        #region << =========== PUBLIC METHODS =========== >>
        
        public bool IsTagEnabled(IDebugTag tag)
        {
            if (!_isEnabled) return false;
            
            var setting = GetTagSetting(tag);
            if (setting == null) return false;
            
            return IsDebugLevelEnabled(setting.Level);
        }

        public bool IsTagEnabled(EDebugType type)
        {
            DebugTag<EDebugType> debugTag = type;
            return IsTagEnabled(debugTag);
        }

        #endregion

        #region << =========== PRIVATE METHODS =========== >>

        private void SetTagSettings()
        {
            InitCustomTags();
            InitTags();
            
            RemoveObsoleteTags();
            
            _tagSettings = _tagSettings.ToList();
        }

        private DebugTagSetting GetTagSetting(IDebugTag tag)
        {
            return _tagSettings.FirstOrDefault(
                setting => setting.TagName == tag.Name && setting.TagValue == tag.Value);
        }

        private bool IsDebugLevelEnabled(EDebugLevel level)
        {
            return level switch
            {   
                EDebugLevel.Disabled => false,
                EDebugLevel.EditorOnly => Application.isEditor,
                EDebugLevel.Development => Application.isEditor || Debug.isDebugBuild,
                EDebugLevel.Always => true,
                _ => false
            };
        }

        protected virtual void InitTags()
        {
            foreach (EDebugType type in Enum.GetValues(typeof(EDebugType)))
            {
                DebugTag<EDebugType> baseTag = type;

                var existingSetting = _tagSettings.FirstOrDefault(s =>
                    s.TagName == baseTag.Name && s.TagValue == baseTag.Value);

                if (existingSetting == null)
                {
                    _tagSettings.Add(DebugTagSetting.Create(baseTag));
                }
                else
                {
                    existingSetting.UpdateIcon(baseTag.Icon);
                }
            }
        }

        private void RemoveObsoleteTags()
        {
            var validBaseTagValues = new HashSet<int>();
            foreach (EDebugType type in Enum.GetValues(typeof(EDebugType)))
            {
                validBaseTagValues.Add((int)type);
            }
            
            _tagSettings.RemoveAll(s => s.TagValue < 1000 && !validBaseTagValues.Contains(s.TagValue));
        }

        protected abstract void InitCustomTags();
        
        #endregion

    }
}