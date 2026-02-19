using _Project.CodeBase.StaticData;

namespace _Project.CodeBase.Services.SaveLoad
{
    public interface ISavedConfig : ISavedConfigReader
    {
        void UpdateConfig(ConfigData progress);
    }
    
    public interface ISavedConfigReader
    {
        void LoadConfig(ConfigData config);
    }
    
    public class ConfigData
    {
        public string Language;
        public bool Sound;
    }
}