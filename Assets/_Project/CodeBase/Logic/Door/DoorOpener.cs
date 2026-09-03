using System.Collections;
using _Project.CodeBase.Events;
using _Project.CodeBase.Services.Audio;
using _Project.CodeBase.Services.Repainting;
using _Project.CodeBase.Services.StaticData;
using _Project.CodeBase.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;

namespace _Project.CodeBase.Logic.Door
{
    public class DoorOpener : MonoBehaviour
    {
        private BaseOnEvent<LevelCompletedSignals>  _onLevelCompleted  = new BaseOnEvent<LevelCompletedSignals>();

        public GameObject DoorOpen;
        public GameObject DoorClosed;
        public GameObject DoorFrame;
        public GameObject AreaStar;

        private bool _flag = false;
        private IAudioService _audioService;
        private IStaticDataService _staticDataService;
        private DoorStaticData _doorStaticData;

        public void Construct(IPaintingService paintingService, IAudioService audioService, IStaticDataService staticDataService)
        {
            if (paintingService != null)
            {
                paintingService.CheckLevelCompletedOnStart();

                paintingService.IsStart(false);

                _audioService = audioService;
                _staticDataService = staticDataService;

                DoorStyles doorStyles = _staticDataService.ForLevel(SceneManager.GetActiveScene().name).DoorStyles;
                _doorStaticData = _staticDataService.ForDoor(doorStyles);
                
                DoorOpen.GetComponent<SpriteRenderer>().sprite = _doorStaticData.OpenDoor;
                DoorClosed.GetComponent<SpriteRenderer>().sprite = _doorStaticData.ClosedDoor;
                DoorFrame.GetComponent<SpriteRenderer>().sprite = _doorStaticData.Frame;

                EventBus.Subscribe(_onLevelCompleted.SetOnInvoke(OpenDoor));
            }
        }

        private void OpenDoor(LevelCompletedSignals signals)
        {
            if (_flag) return;

            _flag = true;
            
            AreaStar.SetActive(true);
            
            if(_audioService != null)
                _audioService.PlayOpenDoor();
            
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
                DoorOpen.transform.Rotate(rotateY * Time.deltaTime);
            }
        }
    }
}