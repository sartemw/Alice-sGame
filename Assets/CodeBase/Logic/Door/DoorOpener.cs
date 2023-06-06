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
                if (repaintingService != null)
                {
                    _repaintingService = repaintingService;
                    _repaintingService.LevelOver += OpenDoor;
                    Door.GetComponent<SpriteRepaintable>().Construct(repaintingService);
                    DoorFrame.GetComponent<SpriteRepaintable>().Construct(repaintingService);
                }
        }

        private void OnDisable()
        {
            if (_repaintingService != null)
                _repaintingService.LevelOver -= OpenDoor;
        }

        private void OpenDoor()
        {
            gameObject.GetComponent<BoxCollider2D>().enabled = true;
            StartCoroutine(RotateY());
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