using System.Collections.Generic;
using System.Linq;
using CodeBase.Fish;
using UnityEngine;

namespace CodeBase.Services.FishCollectorService
{
    public class RepaintingService : IRepaintingService
    {
        public List<GameObject> ColorlessObjs { get; set; }
        public List<GameObject> ColorledObjs { get; set;}

        public void Init()
        {
            CleanUp();
            
            var repaintableObjs = GameObject.FindObjectsOfType<Repaintable>();

            if (repaintableObjs.Length == 0) 
                Debug.LogError($"RepaintableObjs counts = {repaintableObjs.Length}, add \"Repaintable\" Component");

            foreach (Repaintable repaintable in repaintableObjs)
            {
                GameObject obj = repaintable.gameObject;
                repaintable.PaintingColorless();
                ColorlessObjs.Add(obj);
            }
        }

        public void AddFish(ColoredFish fish)
        {
            Painting(fish);
        }

        public bool IsAllFishCollected()
        {
            if (ColorlessObjs.Count == 0)
                return true;
            
            return false;
        }

        private void Painting(ColoredFish fish)
        {
            foreach (GameObject colorlessObj in ColorlessObjs.ToList())
            {
                if (fish.FishColorType == colorlessObj.GetComponent<Repaintable>().ColorType
                    || fish.FishColorType == ColorType.Rainbow)
                {
                    colorlessObj.GetComponent<Repaintable>().PaintingColored();
                    ColorledObjs.Add(colorlessObj);
                    ColorlessObjs.Remove(colorlessObj);
                }
            }
            fish.gameObject.SetActive(false);
        }

        private void CleanUp()
        {
            ColorlessObjs = new List<GameObject>();
            ColorledObjs = new List<GameObject>();
        }
    }
}