using _Project.CodeBase.StaticData;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace _Project.CodeBase.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private ConfigStaticData _config;
        public string CurrentLanguage { get; set; }


        public LocalizationService(ConfigStaticData config)
        {
            _config = config;
        }

        public void SetLanguage(string language)
        {
            CurrentLanguage = language;
            
            _config.Language = language;
            
            ChangeLocal(language);
        }
        
        private void ChangeLocal(string localeCode)
        {
            Locale locale =
                LocalizationSettings.AvailableLocales.GetLocale(localeCode);

            LocalizationSettings.SelectedLocale = locale;
        }
    }
}