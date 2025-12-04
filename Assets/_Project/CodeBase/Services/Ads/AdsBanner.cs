using System;
using _Project.CodeBase.Services.Analytics;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace _Project.CodeBase.Services.Ads
{
    public class AdsBanner
    {
        private readonly string _id;
        
        private Banner _banner;
        private IAnalyticsService _analyticsService;

        public AdsBanner(IAnalyticsService analyticsService, string id)
        {
            _analyticsService = analyticsService;
            _id = id;
        }

        public void Initialized() => 
            RequestInlineBanner();

        private void RequestInlineBanner()
        {   
            string adUnitId = _id; // замените на "R-M-XXXXXX-Y"
            BannerAdSize bannerMaxSize = BannerAdSize.InlineSize(GetScreenWidthDp() / 2, GetScreenWidthDp() / 20);
            _banner = new Banner(adUnitId, bannerMaxSize, AdPosition.TopCenter);
            
            AdRequest request = new AdRequest.Builder().Build();
            _banner.LoadAd(request);
            
            // Вызывается, когда реклама с вознаграждением была загружена
            _banner.OnAdLoaded += HandleAdLoaded;

            // Вызывается, если во время загрузки произошла ошибка
            _banner.OnAdFailedToLoad += HandleAdFailedToLoad;

            // Вызывается, когда приложение становится неактивным, так как пользователь кликнул на рекламу и сейчас перейдет в другое приложение (например, браузер).
            _banner.OnLeftApplication += HandleLeftApplication;

            // Вызывается, когда пользователь возвращается в приложение после клика
            _banner.OnReturnedToApplication += HandleReturnedToApplication;

            // Вызывается, когда пользователь кликнул на рекламу
            _banner.OnAdClicked += HandleAdClicked;

            // Вызывается, когда зарегистрирован показ
            _banner.OnImpression += HandleImpression;
        }
        
        private int GetScreenWidthDp()
        {
            int screenWidth = (int)Screen.safeArea.width;
            return ScreenUtils.ConvertPixelsToDp(screenWidth);
        }
        
        private void HandleAdLoaded(object sender, EventArgs args)
        {
            Debug.Log("AdLoaded event received");
            _banner.Show();
        }

        private void HandleAdFailedToLoad(object sender, AdFailureEventArgs args)
        {
            Debug.Log($"AdFailedToLoad event received with message: {args.Message}");
            // Настоятельно не рекомендуется пытаться загрузить новое объявление с помощью этого метода
        }

        private void HandleLeftApplication(object sender, EventArgs args)
        {
            Debug.Log("LeftApplication event received");
        }

        private void HandleReturnedToApplication(object sender, EventArgs args)
        {
            Debug.Log("ReturnedToApplication event received");
        }

        private void HandleAdClicked(object sender, EventArgs args)
        {
            Debug.Log("AdClicked event received");
            _analyticsService.Send("AdClicked banner");
        }

        private void HandleImpression(object sender, ImpressionData impressionData)
        {
            var data = impressionData == null ? "null" : impressionData.rawData;
            Debug.Log($"HandleImpression event received with data: {data}");
        }
    }
}