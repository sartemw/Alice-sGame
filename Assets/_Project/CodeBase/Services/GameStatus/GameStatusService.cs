using _Project.CodeBase.Events;
using _Project.CodeBase.Services.StaticData;
using UnityEngine.SceneManagement;

namespace _Project.CodeBase.Services.GameStatus
{
    public class GameStatusService : IGameStatusService
    {
        private BaseOnEvent<AllFishPickupSignal>  _onAllFishPickupSignal  = new BaseOnEvent<AllFishPickupSignal>();
        private IStaticDataService _staticDataService;

        public GameStatusService(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
            
            EventBus.Subscribe(_onAllFishPickupSignal.SetOnInvoke(LevelCompleted));
        }

        private void LevelCompleted(AllFishPickupSignal obj)
        {
            if (_staticDataService.ForLevel(SceneManager.GetActiveScene().name).IsBossFight == false)
                EventBus.Invoke(new LevelCompletedSignals());
        }
    }
}