using System.Collections.Generic;

namespace _Project.CodeBase.Services.PersistentConfig
{
    public class PersistentConfigService : IPersistentConfigService
    {
        public List<ISavedConfigReader> ConfigReaders { get; set;} = new List<ISavedConfigReader>();
        public ConfigData ConfigData{ get; set; }
    }
}