using System;
using CodeBase.Services.FishCollectorService;
using UnityEngine;
using Zenject;

namespace CodeBase.Logic
{
    public class OpenOrCloseDoor : MonoBehaviour
    {
        public Sprite OpenDoor;
        public Sprite CloseDoor;
        public bool IsOpen;
        public SpriteRenderer DoorSpriteRenderer;
        private IRepaintingService _repaintingService;

        [Inject]
        public void Construct(IRepaintingService repaintingService)
        {
            _repaintingService = repaintingService;
        }
        private void Start()
        {
            IsOpen = false;
            _repaintingService.PickUpFish += SwitchDoorStatus;
            DoorSpriteRenderer.sprite = CloseDoor;
        }

        private void SwitchDoorStatus()
        {
            IsOpen = true;
            DoorSpriteRenderer.sprite = OpenDoor;
        }

        private void OnEnable()
        {
            _repaintingService.PickUpFish -= SwitchDoorStatus;
        }
    }
}