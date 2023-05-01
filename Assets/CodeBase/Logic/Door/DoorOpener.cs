using CodeBase.Services.Repainting;
using UnityEngine;

namespace CodeBase.Logic.Door
{
    public class DoorOpener : MonoBehaviour
    {
        private GameObject _door;
        private IRepaintingService _repaintingService;
        public void Construct(IRepaintingService repaintingService)
        {
            _repaintingService = repaintingService;
            _repaintingService.PickUpFish += OpenDoor;
            OpenDoor();
        }

        private void OnDisable()
        {
            _repaintingService.PickUpFish -= OpenDoor;
        }

        private void OpenDoor()
        {
            if (_repaintingService.ColorlessObjs.Count == 0)
            {
                gameObject.GetComponent<BoxCollider2D>().enabled = true;
                
            }
        }
    }
}