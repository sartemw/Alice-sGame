using System.Collections;
using _Project.CodeBase.Events;
using _Project.CodeBase.Services.Repainting;
using UnityEngine;

namespace _Project.CodeBase.Logic.Door
{
    public class DoorOpener : MonoBehaviour
    {
        private BaseOnEvent<LevelCompletedSignals>  _onLevelCompleted  = new BaseOnEvent<LevelCompletedSignals>();
        
        public GameObject Door;
        public GameObject DoorFrame;

        private bool _flag = false;
        public void Construct(IPaintingService paintingService)
        {
                if (paintingService != null)
                {
                    EventBus.Subscribe(_onLevelCompleted.SetOnInvoke(OpenDoor));
                    paintingService.CheckLevelCompleted();
                    
                    Door.GetComponent<SpritePaintable>().Construct(paintingService);
                    DoorFrame.GetComponent<SpritePaintable>().Construct(paintingService);
                }
        }

        private void OpenDoor(LevelCompletedSignals signals)
        {
            if (_flag) return;

            _flag = true;

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