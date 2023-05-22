using System.Collections;
using System.Collections.Generic;
using CodeBase.Services.Repainting;
using UnityEngine;

namespace CodeBase.Logic.Door
{
    public class DoorOpener : MonoBehaviour
    {
        public GameObject Door;
        public GameObject DoorFrame;
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
                
                
                Door.GetComponent<SpriteRenderer>().material = _repaintingService.Colored;
                StartCoroutine(RotateY());
                
                DoorFrame.GetComponent<SpriteRenderer>().material = _repaintingService.Colored;
            }
        }

        private IEnumerator RotateY()
        {
            float timer = 0;
            Vector3 rotateY = new Vector3(0,60,0);
            while (timer < 1)
            {
                timer += Time.deltaTime;
                yield return null;
                Door.transform.Rotate(rotateY * Time.deltaTime);
            }
        }
    }
}