namespace SongLib
{
    public enum EDebugType
    {
        None = 0,
        System,
        Init,
        UI,
        Popup,
        Audio,
        Table,
        Info,
        Test,
        Object,
        Tutorial,
        Event,
        Localize,
        Backend,
    }   
    

    public enum EDebugLevel
    {
        Disabled = 0,
        EditorOnly = 1,
        Development = 2,
        Always = 3
    }
}