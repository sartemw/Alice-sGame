using _Project.CodeBase.Services.Analytics;
using _Project.CodeBase.Services.PersistentConfig;
using _Project.CodeBase.StaticData;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace _Project.CodeBase.Services.Localization
{
    public class LocalizationService : ILocalizationService
    {
        private readonly ConfigStaticData _config;
        private string _currentLanguage;
        private IAnalyticsService _analyticsService;

        public LocalizationService(ConfigStaticData config, IAnalyticsService analyticsService)
        {
            _config = config;
            _analyticsService = analyticsService;
            
            Init();
        }

        private void Init()
        {
            if (_config.Language == null)
                return;
            
            ChangeLocal(_config.Language);
            
            _analyticsService.Send($"{_config.Language} version");
        }

        public void SetLanguage(string language)
        {
            _config.Language = language;
            
            ChangeLocal(language);
        }
        
        private void ChangeLocal(string localeCode)
        {
            _currentLanguage = localeCode;
            
            Locale locale =
                LocalizationSettings.AvailableLocales.GetLocale(localeCode);

            LocalizationSettings.SelectedLocale = locale;
        }

        public void LoadConfig(ConfigData config) => 
            ChangeLocal(config.Language);

        public void UpdateConfig(ConfigData config)
        {
            config.Language = _currentLanguage;
        }
    }
}