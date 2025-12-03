using System;
using _Project.CodeBase.Services.Analytics;
using UnityEngine;
using YandexMobileAds;
using YandexMobileAds.Base;

namespace _Project.CodeBase.Services.Ads
{
    public class AdsRewarded
    {
        private readonly string _id;

        private IAnalyticsService _analyticsService;

        private RewardedAdLoader rewardedAdLoader;
        private RewardedAd rewardedAd;
        
        public AdsRewarded(IAnalyticsService analyticsService, string id)
        {
            _analyticsService = analyticsService;
            _id = id;
        }

        public void Initialized()
        {
            SetupLoader();
            RequestRewardedAd();
        }

        private void RequestRewardedAd()
        {
            string adUnitId = _id; // замените на "R-M-XXXXXX-Y"
            AdRequestConfiguration adRequestConfiguration = new AdRequestConfiguration.Builder(adUnitId).Build();
            rewardedAdLoader.LoadAd(adRequestConfiguration);
        }

        private void SetupLoader()
        {
            rewardedAdLoader = new RewardedAdLoader();
            rewardedAdLoader.OnAdLoaded += HandleAdLoaded;
            rewardedAdLoader.OnAdFailedToLoad += HandleAdFailedToLoad;
            // ...
        }
        public void ShowRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Show();
            }
        }
        
        public void DestroyRewardedAd()
        {
            if (rewardedAd != null)
            {
                rewardedAd.Destroy();
                rewardedAd = null;
            }
            
            RequestRewardedAd();
        }
        public void HandleAdLoaded(object sender, RewardedAdLoadedEventArgs args)
        {
            // Rewarded ad was loaded successfully. Now you can handle it.
            rewardedAd = args.RewardedAd;
            
            rewardedAd.OnAdClicked += HandleAdClicked;
            rewardedAd.OnAdShown += HandleAdShown;
            rewardedAd.OnAdFailedToShow += HandleAdFailedToShow;
            rewardedAd.OnAdImpression += HandleImpression;
            rewardedAd.OnAdDismissed += HandleAdDismissed;
            rewardedAd.OnRewarded += HandleRewarded;
        }

        public void HandleAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
        {
            // Ad {args.AdUnitId} failed for to load with {args.Message}
            // Attempting to load new ad from the OnAdFailedToLoad event is strongly discouraged.
        }
        
        public void HandleAdClicked(object sender, EventArgs args)
        {
            // Called when a click is recorded for rewarded ad.
        }

        public void HandleAdShown(object sender, EventArgs args)
        {
            // Called when an ad is shown.
        }

        public void HandleAdFailedToShow(object sender, AdFailureEventArgs args)
        {
            DestroyRewardedAd();
        }

        public void HandleAdDismissed(object sender, EventArgs args)
        {
            DestroyRewardedAd();
        }

        public void HandleImpression(object sender, ImpressionData impressionData)
        {
            // Called when an impression is recorded for an ad.
        }

        public void HandleRewarded(object sender, Reward args)
        {
            // Called when the user can be rewarded with {args.type} and {args.amount}.
            _analyticsService.Send("AdClicked Rewarded");
        }
    }
}