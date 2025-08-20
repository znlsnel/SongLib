using System;

namespace SongLib
{
    public interface ISceneTransition
    {
        void Open();
        void Close();
        void StartTransition(Action onComplete = null);
        void EndTransition(Action onComplete = null);
    }
}