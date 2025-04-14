using _Project.CodeBase.Events;
using _Project.CodeBase.Services.Repainting;
using DG.Tweening;
using UnityEngine;

namespace _Project.CodeBase.Infrastructure.Effects
{
    public class Ink : MonoBehaviour
    {
        private InkData _data;

        public void Construct(Vector2 moveTo, Paintable coloredObj)
        {
            _data = new InkData
            {
                Target = coloredObj,
                MoveTo = moveTo
            };
            
            MoveTo(moveTo);
        }

        private void MoveTo(Vector2 moveTo) =>
            transform.DOMove(moveTo, 2).OnComplete(StartPainting);


        private void StartPainting()
        {
            EventBus.Invoke(new StartPaintingSignal {Target = _data.Target});
            Destroy(gameObject);
        }
    }
}