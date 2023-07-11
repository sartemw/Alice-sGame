using System;
using System.Collections.Generic;
using CodeBase.Fish;
using CodeBase.Services.Repainting;
using TMPro;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace CodeBase.Infrastructure
{
    public class Debuger : MonoBehaviour
    {
        public GameObject ActiveText;
        public GameObject NameText;
        public TMP_Text Text;
        
        public List<string> ColoredObjects = new List<string>();
        public List<string> ColorlessObjects = new List<string>();
        
        private List<GameObject> repaint =  new List<GameObject>();
        private IRepaintingService _repaintingService;
        private IFishDataService _fishDataService;

        [Inject]
        public void Construct(IRepaintingService repaintingService, IFishDataService fishDataService)
        {
            _repaintingService = repaintingService;
            _fishDataService = fishDataService;
            _fishDataService.FishPickedUp += Repaint;
        }

        private void Repaint(ColoredFish obj)
        {
            Clear();
            Collect();
        }

        private void Start()
        {
            Collect();
            
            DontDestroyOnLoad(gameObject);
        }

        private void Painting(List<string> objs, Color color)
        {
            foreach (string pair in objs)
            {
                var textName = Object.Instantiate(Text.gameObject, NameText.transform);
                textName.GetComponent<TMP_Text>().text = pair;
                textName.GetComponent<TMP_Text>().color = color;
                repaint.Add(textName);
            }
        }

        private void Clear()
        {
            foreach (GameObject o in repaint)
            {
                Destroy(o);
            }
            repaint.Clear();
            ColoredObjects.Clear();
            ColorlessObjects.Clear();
        }

        private void Collect()
        {
            foreach (Repaintable repaintable in _repaintingService.ColoredObjs)
            {
                ColoredObjects.Add(repaintable.name);
            }
            
            foreach (Repaintable repaintable in _repaintingService.ColorlessObjs)
            {
                ColorlessObjects.Add(repaintable.name);
            }

            Painting(ColoredObjects, Color.red);
            Painting(ColorlessObjects, Color.green);
        }

        private void OnDisable()
        {
            _fishDataService.FishPickedUp -= Repaint;
        }
    }
}