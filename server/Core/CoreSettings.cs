namespace server.core
{
    //Loglevels: 
    /*
        0: naked, warning, error, info
        1: warning / error only
        2: info / warning / error
    */

    public struct CoreSettings
    {
        public string _SettingFile;
        public int LogLevel;
    }
}