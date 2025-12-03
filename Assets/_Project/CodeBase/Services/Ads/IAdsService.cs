using System;
using _Project.CodeBase.Events;

namespace _Project.CodeBase.Services.Ads
{
  public interface IAdsService : IService
  {
    bool IsRewardedVideoReady { get; }
    int Reward { get; }
    void Initialize();
    void LoadAd();
  }
}