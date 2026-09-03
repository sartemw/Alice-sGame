using _Project.CodeBase.Services.PersistentConfig;
using _Project.CodeBase.Services.SaveLoad;

namespace _Project.CodeBase.Services.Localization
{
    public interface ILocalizationService : ISavedConfig
    {
        public void SetLanguage(string language);
    }
}