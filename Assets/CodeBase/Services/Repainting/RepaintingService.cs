using System;
using System.Collections.Generic;
using System.Linq;
using CodeBase.Fish;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Services.Repainting
{
    public class RepaintingService : IRepaintingService
    {
        private const string Initial = "Initial";
        private const string GameEnd = "GameEnd";
        private const float ScaleMultiple = 2.1f;

        public Material Colorless { get; set; }
        public Material Colored { get; set; }
        
        public List<Repaintable> ColorlessObjs { get; set; }
        public List<Repaintable> ColoredObjs { get; set;}
        public List<RepaintingData> RepaintingDatas { get; set; }
        public event Action PickUpFish;
        
        private ScalerPaintingMask.Factory _maskFactory;
        private IRepaintingService _repaintingServiceImplementation;
        private IRepaintingService _repaintingServiceImplementation1;
        private IRepaintingService _repaintingServiceImplementation2;
        private GameObject _hero;


        public void Init(Material colorless, Material colored, ScalerPaintingMask.Factory maskFactory)
        {
            Colored = colored;
            Colorless = colorless;
            _maskFactory = maskFactory;
        }

        public void Restart()
        {
            CleanUp();

            var repaintables = GameObject.FindObjectsOfType<Repaintable>();

            if (repaintables.Length == 0 && IsInitialOrEndScene()) 
                Debug.LogError($"RepaintableObjs counts = {repaintables.Length}, add \"Repaintable\" Component");

            foreach (Repaintable repaintable in repaintables)
            {
                FillingRepaintingData(repaintable);

                //repaintable.Painting(Colorless);
                ColorlessObjs.Add(repaintable);
            }
        }

        private void FillingRepaintingData(Repaintable repaintable)
        {
            Vector2 posC = repaintable.GetComponent<Renderer>().bounds.center;
            Vector2 size = repaintable.GetComponent<Renderer>().bounds.size;

            RepaintingData repaintingData = new RepaintingData();
            repaintingData.AnglesCoordinates = new Dictionary<int, Vector2>()
            {
                {1, new Vector2(posC.x + size.x / 2, posC.y + size.y / 2)},
                {2, new Vector2(posC.x + size.x / -2, posC.y + size.y / -2)},
                {3, new Vector2(posC.x + size.x / 2, posC.y + size.y / -2)},
                {4, new Vector2(posC.x + size.x / -2, posC.y + size.y / 2)}
            };
            repaintingData.RepaintableObjects = repaintable;

            RepaintingDatas.Add(repaintingData);
        }

        private static bool IsInitialOrEndScene() => 
            SceneManager.GetActiveScene().name == Initial 
            || SceneManager.GetActiveScene().name == GameEnd;

        public void AddFish(ColoredFish fish)
        {
            Painting(fish);
            PickUpFish?.Invoke();
        }

        private void Painting(ColoredFish fish)
        {
            ScalerPaintingMask mask = CreateMask(fish);

            PaintOtherColors(fish, mask);
            mask.StartScaling();
            fish.gameObject.SetActive(false);
            
            PaintRainbowColor(fish);
        }

        private ScalerPaintingMask CreateMask(ColoredFish fish)
        {
            ScalerPaintingMask mask = _maskFactory.Create();
            mask.transform.position = fish.transform.position;
            mask.GetComponent<SpriteMask>().frontSortingOrder = -1 - (int) fish.FishColorType;
            mask.GetComponent<SpriteMask>().backSortingOrder = -2 - (int) fish.FishColorType;
            return mask;
        }
        private ScalerPaintingMask CreateMask()
        {
            ScalerPaintingMask mask = _maskFactory.Create();
            mask.transform.position = Vector2.zero;
            mask.GetComponent<SpriteMask>().isCustomRangeActive = false;
            return mask;
        }

        private void PaintRainbowColor(ColoredFish fish)
        {
            float maxDistancePainting = 0;
            bool isOnlyRainbowColor = true;
            
            foreach (Repaintable colorlessObj in ColorlessObjs)
            {
                if (colorlessObj.ColorType != ColorType.Rainbow)
                {
                    isOnlyRainbowColor = false;
                    break;
                }
            }

            if (fish.FishColorType == ColorType.Rainbow)
                isOnlyRainbowColor = true;

            if (isOnlyRainbowColor)
            {
                var mask = CreateMask();
                
                foreach (RepaintingData data in RepaintingDatas.ToList())
                {
                    for (int i = 1; i < 5; i++)
                        {
                            float distance = Vector2.Distance(
                                Vector2.zero,
                                 data.AnglesCoordinates[i]);
                            if (maxDistancePainting < distance)
                            {
                                maxDistancePainting = distance;
                                mask.IncreaseScale = maxDistancePainting * ScaleMultiple;
                            }
                        }
                    RepaintingDatas.Remove(data);
                    
                }
                mask.StartScaling();
            }
        }

        private void PaintOtherColors(ColoredFish fish,  ScalerPaintingMask mask)
        {
            float maxDistancePainting = 0;
            foreach (Repaintable colorlessObj in ColorlessObjs.ToList())
            {
                if (fish.FishColorType == colorlessObj.ColorType
                    || fish.FishColorType == ColorType.Rainbow)
                {
                    // colorlessObj.Painting(Colored);
                    foreach (RepaintingData data in RepaintingDatas.ToList())
                    {
                        if (data.RepaintableObjects == colorlessObj)
                        {
                            for (int i = 1; i < 5; i++)
                            {
                                float distance = Vector2.Distance(
                                    fish.transform.position
                                    , data.AnglesCoordinates[i]);
                                if (maxDistancePainting < distance)
                                {
                                    maxDistancePainting = distance;
                                    mask.IncreaseScale = maxDistancePainting * ScaleMultiple;
                                }
                            }

                            RepaintingDatas.Remove(data);
                        }
                    }

                    ColoredObjs.Add(colorlessObj);
                    ColorlessObjs.Remove(colorlessObj);
                }
            }
        }

        private void CleanUp()
        {
            RepaintingDatas = new List<RepaintingData>();
            ColorlessObjs = new List<Repaintable>();
            ColoredObjs = new List<Repaintable>();
        }
    }
}