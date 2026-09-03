using System.Collections.Generic;

namespace _Project.CodeBase.Services.PersistentConfig
{
    public interface IPersistentConfigService : IService
    {
        public List<ISavedConfigReader> ConfigReaders { get; set;}
        public ConfigData ConfigData{ get; set; }
    }
}