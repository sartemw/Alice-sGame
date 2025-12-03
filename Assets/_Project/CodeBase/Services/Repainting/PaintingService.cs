using System.Collections.Generic;
using System.Linq;
using _Project.CodeBase.Events;
using _Project.CodeBase.Fish;
using _Project.CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.Tilemaps;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.CodeBase.Services.Repainting
{
    public class PaintingService : IPaintingService
    {
        private readonly DiContainer _diContainer;
        private IGameFactory _gameFactory;

        private BaseOnEvent<FishPickupSignal>  _onFishPickup  = new BaseOnEvent<FishPickupSignal>();
        private BaseOnEvent<BootstrapFinishedSignal>  _onBootstrapFinished  = new BaseOnEvent<BootstrapFinishedSignal>();
        private BaseOnEvent<PaintingCompletedSignal>  _onPaintingCompleted  = new BaseOnEvent<PaintingCompletedSignal>();
        private BaseOnEvent<AllLevelColoringSignal>  _onLevelLoad  = new BaseOnEvent<AllLevelColoringSignal>();
        private BaseOnEvent<StartFadeSignal>  _onFadeMaterialSignal  = new BaseOnEvent<StartFadeSignal>();
        
        private BaseOnEvent<StartPaintingSignal>  _onStartPaintingSignal  = new BaseOnEvent<StartPaintingSignal>();
        private BaseOnEvent<StartPaintingInstantlySignal>  _StartPaintingInstantlySignal  = new BaseOnEvent<StartPaintingInstantlySignal>();

        public Material ColorlessMaterial {get;}
        public Material Colored {get;}

        public List<Paintable> ColorlessObjs { get; private set; }
        public List<Paintable> ColoredObjs { get; private set;}
        
        private bool _isStart = true;
        private bool _completedLevelFlag = false;
        private List<Paintable> _removeList = new List<Paintable>();
        private int _startSignalCount, _completeSignalCount;

        public PaintingService(Material colorless, Material colored, DiContainer diContainer)
        {
            Colored = colored;
            ColorlessMaterial = colorless;
            _diContainer = diContainer;
            
            EventBus.Subscribe(_onFishPickup.SetOnInvoke(Painting));
            EventBus.Subscribe(_onBootstrapFinished.SetOnInvoke(ConstructBootstrap));
            EventBus.Subscribe(_onPaintingCompleted.SetOnInvoke(IsLevelCompleted));
            EventBus.Subscribe(_onLevelLoad.SetOnInvoke(AllLevelColored));
            EventBus.Subscribe(_onFadeMaterialSignal.SetOnInvoke(Fade));
            EventBus.Subscribe(_onStartPaintingSignal.SetOnInvoke(TickStart));
            EventBus.Subscribe(_StartPaintingInstantlySignal.SetOnInvoke(TickStart));
        }

        private void TickStart(StartPaintingInstantlySignal obj) => 
            _startSignalCount++;

        private void TickStart(StartPaintingSignal obj) => 
            _startSignalCount++;

        private void ConstructBootstrap(BootstrapFinishedSignal obj) => 
            _gameFactory = _diContainer.Resolve<IGameFactory>();

        public void StartLevel()
        {
            CleanUp();
            
            Paintable[] paintables = GameObject.FindObjectsOfType<Paintable>();
            
            foreach (Paintable paintable in paintables)
            {
                ColoredObjs.Add(paintable);
                paintable.Initialize();
            }
        }

        public void CheckLevelCompletedOnStart()
        {
            if (ColoredObjs.Count == 0 && _isStart) 
                EventBus.Invoke(new LevelCompletedSignals());
        }

        public void IsStart(bool value) => 
            _isStart = value;

        public void SetColorless(Paintable paintable) => 
            ColorlessObjs.Add(paintable);

        public async void Painting(FishPickupSignal fishSignal)
        {
            ColoredFish fish = fishSignal.ColoredFish;
            Debug.Log($"<color={fish.ColorType}> Picked {fish.ColorType} fish</color>");

            foreach (Paintable colorlessObj in ColorlessObjs.ToList())
            {
                if (fish.ColorType == colorlessObj.ColorType
                    || fish.ColorType == ColorType.Rainbow)
                {
                    _removeList.Add(colorlessObj);
                }
            }
            
            foreach (Paintable coloredObj in ColoredObjs.ToList())
            {
                if (fish.ColorType == coloredObj.ColorType
                    || fish.ColorType == ColorType.Rainbow)
                {
                    await _gameFactory.CreateInk(fish.Position, CreatePointInk(coloredObj), coloredObj, this);

                    _removeList.Add(coloredObj);
                }
            }
            
            fish.gameObject.SetActive(false);
        }

        private Vector2 CreatePointInk(Paintable paintable)
        {
            Vector2 positionInk = paintable.transform.position;
            
            if (paintable is TilemapPaintable)
            {
                var boundTile =paintable.GetComponent<TilemapRenderer>().bounds;
                positionInk = new Vector2(Random.Range(boundTile.center.x - boundTile.size.x, boundTile.center.x + boundTile.size.x)/3,
                    Random.Range(boundTile.center.y - boundTile.size.y,boundTile.center.y + boundTile.size.y)/3);
            }

            return positionInk;
        }

        private void CleanUp()
        {
            ColorlessObjs = new List<Paintable>();
            ColoredObjs = new List<Paintable>();
            _removeList = new List<Paintable>();
            _isStart = true;
            _completedLevelFlag = false;
            _completeSignalCount = 0;
            _startSignalCount = 0;
        }

        private void IsLevelCompleted(PaintingCompletedSignal signal)
        {
            _completeSignalCount++;
            
            foreach (Paintable paintable in _removeList)
            {
                ColoredObjs.Remove(paintable);
                ColorlessObjs.Remove(paintable);
            }
            
            if (ColorlessObjs.Count == 0 
                && ColoredObjs.Count == 0 
                && !_completedLevelFlag 
                && _completeSignalCount == _startSignalCount)
            {
                _completedLevelFlag = true;
                EventBus.Invoke(new LevelCompletedSignals());
            }
        }

        private void Fade(StartFadeSignal obj)
        {
            foreach (Paintable coloredObj in ColoredObjs)
            {
                EventBus.Invoke(new FadeMaterialSignal {Target = coloredObj});
            }
        }

        private void AllLevelColored(AllLevelColoringSignal signal)
        {
            foreach (Paintable coloredObj in ColoredObjs)
            {
                EventBus.Invoke(new StartPaintingInstantlySignal {Target = coloredObj});
            }
        }
    }
}