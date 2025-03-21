using System.Collections.Generic;
using _Project.CodeBase.Events;
using _Project.CodeBase.Services.Repainting;
using TMPro;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace _Project.CodeBase.Infrastructure
{
    public class Debuger : MonoBehaviour
    {
        public GameObject ActiveText;
        public GameObject NameText;
        public TMP_Text Text;
        
        private BaseOnEvent<FishPickupSignal>  _onFishPickup  = new BaseOnEvent<FishPickupSignal>();
        
        public List<string> ColoredObjects = new List<string>();
        public List<string> ColorlessObjects = new List<string>();
        
        private List<GameObject> repaint =  new List<GameObject>();
        private IPaintingService _paintingService;
        private IFishDataService _fishDataService;

        [Inject]
        public void Construct(IPaintingService paintingService, IFishDataService fishDataService)
        {
            _paintingService = paintingService;
            _fishDataService = fishDataService;
            
            EventBus.Subscribe(_onFishPickup.SetOnInvoke(Repaint));
        }

        private void Repaint(FishPickupSignal obj)
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
            foreach (Paintable repaintable in _paintingService.ColoredObjs)
            {
                ColoredObjects.Add(repaintable.name);
            }
            
            foreach (Paintable repaintable in _paintingService.ColorlessObjs)
            {
                ColorlessObjects.Add(repaintable.name);
            }

            Painting(ColoredObjects, Color.red);
            Painting(ColorlessObjects, Color.green);
        }
    }
}