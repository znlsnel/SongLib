using System.Collections.Generic;
using UnityEngine;

namespace SongLib
{
    public abstract class BaseTimeManager<T> : SingletonWithMono<T>, IBaseManager, ITimeManager where T : BaseTimeManager<T>
    {
        protected Dictionary<TimeLayer, IBaseTime> timeSourceDict = new();
        
        public bool IsInitialized { get; set; }
        public void Init()
        {
            Global.Init(this);
            AddTimeSource();
            IsInitialized = true;
            
            DebugHelper.Log(EDebugType.Init, $"🟢 - [ TimeManager ] Initialize Completed!");
        }

        protected abstract void AddTimeSource();

        private void Update()
        {
            if (!IsInitialized) return;
            
            float unscaledDelta = Time.unscaledDeltaTime;

            foreach (var pair in timeSourceDict)
            {
                pair.Value.Update(unscaledDelta);
            }
        }
        
        public IBaseTime Get(TimeLayer layer)
        {
            if (!timeSourceDict.TryGetValue(layer, out var time))
            {
                DebugHelper.LogWarning(EDebugType.System, $"[TimeManager] TimeLayer {layer} not registered.");
            }
            return time;
        }    
    }
}