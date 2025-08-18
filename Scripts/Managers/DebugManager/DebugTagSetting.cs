using UnityEngine;
using Sirenix.OdinInspector;

namespace SongLib
{
    [System.Serializable]
    public class DebugTagSetting
    {
        #region << =========== FIELDS =========== >>
        
        [SerializeField]
        [HideInInspector]
        private string _tagName;
        
        private int _tagValue;
        private string _tagIcon;
        
        [SerializeField]
        [GUIColor("GetLevelColor")]
        [EnumToggleButtons]
        [LabelWidth(50)]
        [HorizontalGroup("Setting", 0.7f)]
        private EDebugLevel _level = EDebugLevel.EditorOnly;
        
        #endregion

        #region << =========== PROPERTIES =========== >>
        
        public string TagName
        { 
            get => _tagName; 
            set => _tagName = value; 
        }

        [ShowInInspector]
        [PropertyOrder(-1)]
        [LabelText("Tag")]
        [LabelWidth(30)]
        [HorizontalGroup("Setting", 0.3f)]
        [GUIColor("GetColorByType")]
        [ReadOnly]
        private string DisplayName => $"{_tagIcon} {_tagName}";
        
        public int TagValue
        { 
            get => _tagValue; 
            set => _tagValue = value; 
        }
        
        public string TagIcon
        { 
            get => _tagIcon; 
            set => _tagIcon = value; 
        }
        
        public EDebugLevel Level 
        { 
            get => _level; 
            set => _level = value; 
        }
        
        #endregion

        #region << =========== PUBLIC METHODS =========== >>
        
        public static DebugTagSetting Create(IDebugTag tag, EDebugLevel level = EDebugLevel.EditorOnly)
        {
            return new DebugTagSetting
            {
                _tagName = tag.Name,
                _tagValue = tag.Value,
                _tagIcon = tag.Icon,
                _level = level
            };
        }

        public void UpdateIcon(string newIcon)
        {
            _tagIcon = newIcon;
        }
        
        #endregion

        #region << =========== UTILS =========== >>
        
        private Color GetLevelColor()
        {
            return _level switch
            {
                EDebugLevel.Disabled => new Color(0.6f, 0.6f, 0.6f),
                EDebugLevel.EditorOnly => new Color(0.4f, 0.8f, 1.0f),
                EDebugLevel.Development => new Color(1.0f, 0.9f, 0.3f),
                EDebugLevel.Always => new Color(1.0f, 0.5f, 0.5f),
                _ => Color.white
            };
        }

        private Color GetColorByType()
        {
            if (_tagValue < 1000)
            {
                return Color.yellow;
            }
            
            return Color.white;
        }
        
        #endregion
    }
}