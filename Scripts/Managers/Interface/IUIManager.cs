namespace SongLib
{
    public interface IUIManager
    {
        bool IsInitialized { get; set; }

        void Reset();
        void EnrollUI(UIBase uiBase);
        void UnenrollUI(UIBase uiBase);
        
        UIBase GetUI(int index);
        UIBase GetUI(string tagName);
        T GetUI<T>() where T : UIBase;
        int GetEnrollIndex();
        int GetUICount();
        
        void Localize();
        string GetDebugInfo();
        string GetUIListDebugInfo();
    }
}