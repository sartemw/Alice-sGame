using CodeBase.Data;
using UnityEngine;
using Zenject;

namespace CodeBase.Fish
{
    public class FishSpawnPoint : MonoBehaviour
    {
        public string Id { get; set; }
        public ColorType ColorType;
        public FishBehaviourEnum FishBehaviour;

        private ColoredFish.Factory _fishFactory;

        [Inject]
        public void Construct(ColoredFish.Factory fishFactory)
        {
            _fishFactory = fishFactory;
        }

        public void Start()
        {
            SpawnFish();
        }

        private void SpawnFish()
        {
            ColoredFish fish = _fishFactory.Create();
            fish.transform.position = transform.position;
            fish.transform.parent = transform;
            fish.ColorType = ColorType;
            fish.Color = fish.ColorType.SwitchColor();
            fish.GetComponent<SpriteRenderer>().color = fish.Color;
        }
    }
}