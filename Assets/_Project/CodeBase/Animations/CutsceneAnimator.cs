using _Project.CodeBase.Enemy;
using _Project.CodeBase.Events;
using _Project.CodeBase.Infrastructure.Effects;
using _Project.CodeBase.Infrastructure.Factory;
using Ami.BroAudio;
using DG.Tweening;
using UnityEngine;
using Random = UnityEngine.Random;

namespace _Project.CodeBase.Animations
{
    public class CutsceneAnimator : MonoBehaviour
    {
        public Transform[] MovementPoints;
        public Transform[] ActiveObjects;

        public GameObject Ink;
        private int _iterator = 0;

        private void Update()
        {
            if (Input.GetMouseButtonDown(0))
            {
                InkSpawnPoint();
            }
        }
        
        public void AllLevelColoring() =>
            EventBus.Invoke(new AllLevelColoringSignal());

        public void FadeMaterial() => 
            EventBus.Invoke(new StartFadeSignal());

        public void SendInkToObject(Transform activeObject)
        {
            for (int i = 0; i < 4; i++)
            {
                CreateInkToBlot(InkSpawnPoint(), activeObject.position);
            }
        }

        private void CreateInkToBlot(Vector2 from, Vector2 to)
        {
            GameObject ink = Instantiate(Ink, from, RotateTo(from,to));
            
            ink.transform.DOMove(to, 1).OnComplete(() => DestroyInk(ink));
        }

        private void DestroyInk(GameObject ink)
        {
            ink.GetComponent<Ink>().CreateSparks();
            Destroy(ink, 0.5f);
        }


        //EventBus.Invoke(new SandInkToBlotSignals(){At = CreateInkSpawnPoint(), To = BlotTransform});

        public void TranslateTo(Transform activeObject)
        {
            if (_iterator == MovementPoints.Length)
                ToNextScene();
            
            activeObject.transform.DOMove(MovementPoints[_iterator].position, 2);
            _iterator++;
        }

        private void ToNextScene()
        {
            _iterator = 0;
        }

        private Vector2 InkSpawnPoint()
        {
            Vector3 boundsCenter = Camera.main.transform.position;
            float boundsX = Camera.main.orthographicSize;
            float boundsY = Camera.main.orthographicSize / Screen.height * Screen.width;
                
            Vector2 positionInk = new Vector2(Random.Range(boundsCenter.y - boundsY,boundsCenter.y + boundsY)
                , Random.Range(boundsCenter.x - boundsX, boundsCenter.x + boundsX));

            return positionInk;
        }

        private Quaternion RotateTo(Vector2 from, Vector2 to)
        {
            Vector2 inkPos = from;
            Vector2 target = to;

            target.x -= inkPos.x;
            target.y -= inkPos.y;
            float angle = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
            
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, -angle-180));
            
            return targetRotation;
        }
    }
}