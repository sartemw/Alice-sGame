using System;
using System.Collections.Generic;
using System.Linq;
using _Project.CodeBase.Events;
using _Project.CodeBase.Fish;
using _Project.CodeBase.Infrastructure.Factory;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;
using UnityEngine.UIElements;
using Zenject;
using Random = UnityEngine.Random;

namespace _Project.CodeBase.Services.Repainting
{
    public class PaintingService : IPaintingService
    {
        private const string Initial = "Initial";
        private const string GameEnd = "GameEnd";
        
        private readonly IFishDataService _fishDataService;
        private readonly DiContainer _diContainer;
        private IGameFactory _gameFactory;

        private BaseOnEvent<FishPickupSignal>  _onFishPickup  = new BaseOnEvent<FishPickupSignal>();
        private BaseOnEvent<BootstrapFinishedSignal>  _onBootstrapFinished  = new BaseOnEvent<BootstrapFinishedSignal>();
        public Material Colorless {get;}
        public Material Colored {get;}

        public List<Paintable> ColorlessObjs { get; private set; }
        public List<Paintable> ColoredObjs { get; private set;}

        public event Action LevelOver;

        public PaintingService(Material colorless, Material colored, 
            IFishDataService fishDataService, DiContainer diContainer)
        {
            Colored = colored;
            Colorless = colorless;
            _fishDataService = fishDataService;
            _diContainer = diContainer;
            EventBus.Subscribe(_onFishPickup.SetOnInvoke(Painting));
            EventBus.Subscribe(_onBootstrapFinished.SetOnInvoke(ConstructBootstrap));
        }

        private void ConstructBootstrap(BootstrapFinishedSignal obj) => 
            _gameFactory = _diContainer.Resolve<IGameFactory>();

        public void StartLevel()
        {
            CleanUp();
            
            Paintable[] paintables = GameObject.FindObjectsOfType<Paintable>();

            //на сцене только требующие раскраску объекты,
            //затемненные создаются и добавляются из Paintable
            foreach (Paintable paintable in paintables)
            {
                ColoredObjs.Add(paintable);
                paintable.Initialize();
            } 
                

            if (_fishDataService.FishOnLevel == 0)
                PaintingLevelOver();
        }

        public void SetColorless(Paintable paintable) => 
            ColorlessObjs.Add(paintable);

        public void Painting(FishPickupSignal fishSignal)
        {
            ColoredFish fish = fishSignal.ColoredFish;
            Debug.Log($"<color={fish.ColorType}> Picked {fish.ColorType} fish</color>");

            foreach (Paintable colorlessObj in ColorlessObjs.ToList())
            {
                if (fish.ColorType == colorlessObj.ColorType
                    || fish.ColorType == ColorType.Rainbow)
                {
                    ColorlessObjs.Remove(colorlessObj);
                    colorlessObj.Fade();
                    
                    //_gameFactory.CreateInk(colorlessObj.transform.position, colorlessObj.transform.parent.GetComponent<Paintable>());
                }
            }
            
            
            
            foreach (Paintable coloredObj in ColoredObjs.ToList())
            {
                if (fish.ColorType == coloredObj.ColorType
                    || fish.ColorType == ColorType.Rainbow)
                {
                    _gameFactory.CreateInk(CreatePointInk(coloredObj), coloredObj);

                    ColoredObjs.Remove(coloredObj);
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
                positionInk = new Vector2(Random.Range(boundTile.center.x - boundTile.size.x, boundTile.center.x + boundTile.size.x)/2, Random.Range(boundTile.center.y - boundTile.size.y,boundTile.center.y + boundTile.size.y)/2);
            }

            return positionInk;
        }

        private void PaintingLevelOver()
        {
            if (IsInitialOrEndScene())
                return;
            
            LevelOver?.Invoke();
        }

        private bool IsInitialOrEndScene() => 
            SceneManager.GetActiveScene().name == Initial 
            || SceneManager.GetActiveScene().name == GameEnd;

        private void CleanUp()
        {
            ColorlessObjs = new List<Paintable>();
            ColoredObjs = new List<Paintable>();
        }
    }
}