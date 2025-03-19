using System;
using System.Collections.Generic;
using System.Linq;
using _Project.CodeBase.EventBus;
using _Project.CodeBase.EventBus.Events;
using _Project.CodeBase.Fish;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace _Project.CodeBase.Services.Repainting
{
    public class PaintingService : IPaintingService
    {
        private const string Initial = "Initial";
        private const string GameEnd = "GameEnd";
        
        private readonly IFishDataService _fishDataService;
        private BaseOnEvent<FishPickupSignal>  _onFishPickup  = new BaseOnEvent<FishPickupSignal>();
        public Material Colorless {get;}
        public Material Colored {get;}

        public List<Paintable> ColorlessObjs { get; private set; }
        public List<Paintable> ColoredObjs { get; private set;}

        public event Action LevelOver;

        public PaintingService(Material colorless, Material colored, 
            IFishDataService fishDataService)
        {
            Colored = colored;
            Colorless = colorless;
            _fishDataService = fishDataService;
            EventBus.EventBus.Subscribe(_onFishPickup.SetOnInvoke(Painting));
        }

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
                }
            }
            
            
            
            foreach (Paintable coloredObj in ColoredObjs.ToList())
            {
                if (fish.ColorType == coloredObj.ColorType
                    || fish.ColorType == ColorType.Rainbow)
                {
                    ColoredObjs.Remove(coloredObj);
                    coloredObj.Brightening();
                }
            }
            
            //рыба плывет
            fish.gameObject.SetActive(false);
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