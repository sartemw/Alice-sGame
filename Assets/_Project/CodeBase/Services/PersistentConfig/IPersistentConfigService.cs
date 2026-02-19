namespace _Project.CodeBase.Services.PersistentConfig
{
    public interface IPersistentConfigService : IService
    {
        public ConfigData ConfigData{ get; set; }
    }
}