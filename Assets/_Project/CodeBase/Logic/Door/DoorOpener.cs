using System.Collections;
using _Project.CodeBase.Services.Repainting;
using UnityEngine;

namespace _Project.CodeBase.Logic.Door
{
    public class DoorOpener : MonoBehaviour
    {
        public GameObject Door;
        public GameObject DoorFrame;
        private IPaintingService _paintingService;
        public void Construct(IPaintingService paintingService)
        {
                if (paintingService != null)
                {
                    _paintingService = paintingService;
                    _paintingService.LevelOver += OpenDoor;
                    Door.GetComponent<SpritePaintable>().Construct(paintingService);
                    DoorFrame.GetComponent<SpritePaintable>().Construct(paintingService);
                }
        }

        private void OnDisable()
        {
            if (_paintingService != null)
                _paintingService.LevelOver -= OpenDoor;
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