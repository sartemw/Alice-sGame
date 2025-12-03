using System;
using _Project.CodeBase.Events.Example;
using UnityEngine;
using UnityEngine.UI;
using YandexMobileAds.Base;

namespace _Project.CodeBase.Services.Ads
{
    public class RewardedButton : MonoBehaviour
    {
        public Button ShowAdButton;
      
        public Reward Value;


        private void Start()
        {
            ShowAdButton.onClick.AddListener(Show);
        }

        private void Show() =>
            EventBus.Invoke(new ClickShowRewardedSignal() {Reward = Value});
    }
}