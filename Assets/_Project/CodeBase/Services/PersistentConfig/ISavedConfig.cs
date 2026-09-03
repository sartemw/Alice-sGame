namespace _Project.CodeBase.Services.PersistentConfig
{
    public interface ISavedConfig : ISavedConfigReader
    {
        void UpdateConfig(ConfigData config);
    }
}