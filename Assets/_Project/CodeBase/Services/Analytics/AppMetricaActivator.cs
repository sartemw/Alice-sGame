using Io.AppMetrica;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace _Project.CodeBase.Services.Analytics
{
    public static class AppMetricaActivator {
        private const string ApiKey = "854f73b3-922e-46ff-90a0-9add7f28eb95";

        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
        private static void Activate() {
            AppMetrica.Activate(new AppMetricaConfig(ApiKey) {
                FirstActivationAsUpdate = !IsFirstLaunch(),
            });
        }
    
        private static bool IsFirstLaunch() {
            AppMetrica.ReportEvent("IsFirstLaunch AppMetricaActivator");
            return true;
        }

        public static void Init() => 
            Activate();
    }
}