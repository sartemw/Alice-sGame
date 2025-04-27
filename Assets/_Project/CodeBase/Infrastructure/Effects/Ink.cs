using System;
using System.Collections;
using _Project.CodeBase.Events;
using _Project.CodeBase.Fish;
using _Project.CodeBase.Services.Repainting;
using _Project.CodeBase.StaticData;
using DG.Tweening;
using UnityEngine;
using UnityEngine.VFX;

namespace _Project.CodeBase.Infrastructure.Effects
{
    public class Ink : MonoBehaviour
    {
        private const string BaseGradient = "BaseGradient";
        private const string BaseVelocity = "BaseVelocity";
        private InkData _data;
        private IPaintingService _paintingService;
        private VisualEffect _effect;

        [SerializeField] private float _speed;
        [SerializeField] private GameObject _sparks;
        [SerializeField] private GameObject _ink;
        private ConfigStaticData _config;


        public void Construct(Vector2 moveTo, Paintable coloredObj, IPaintingService paintingService, ConfigStaticData config)
        {
            _data = new InkData
            {
                Target = coloredObj,
                MoveTo = moveTo
            };
            CreateSparks();
            _paintingService = paintingService;
            _config = config;
            _effect = GetComponent<VisualEffect>();
            
            _effect.SetGradient(BaseGradient, SetGradient(coloredObj.ColorType));

            StartCoroutine(MoveEffect());
            
            MoveTo(moveTo);
        }

        private Gradient SetGradient(ColorType colorType)
        {
            Gradient gradient = new Gradient();  
            switch (colorType)
            {
                case ColorType.Rainbow:
                    gradient = _config.Rainbow;
                    break;
                case ColorType.Red:
                    gradient = _config.Red;
                    break;
                case ColorType.Green:
                    gradient = _config.Green;
                    break;
                case ColorType.Blue:
                    gradient = _config.Blue;
                    break;
                case ColorType.Yellow:
                    gradient = _config.Yellow;
                    break;
                case ColorType.Cyan:
                    gradient = _config.Cyan;
                    break;
                case ColorType.Purple:
                    gradient = _config.Purple;
                    break;
                default:
                    gradient = _config.Rainbow;
                    break;
            }

            return gradient;
        }

        private void CreateSparks() => 
            Instantiate(_sparks, transform.position, transform.rotation);

        private void CreateInkEffect() => 
            Instantiate(_sparks, transform.position, transform.rotation);
        
        private void MoveTo(Vector2 moveTo) => 
            transform.DOMove(moveTo, _speed).SetEase(Ease.InQuad).OnComplete(StartPainting);

        private IEnumerator MoveEffect()
        {
            while (true)
            {
                _effect.SetVector2(BaseVelocity, _effect.GetVector2(BaseVelocity) + _data.MoveTo.normalized/10);

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void StartPainting()
        {
            EventBus.Invoke(new StartPaintingSignal {Target = _data.Target});

            CreateSparks();
            Destroy(gameObject);
        }
    }
}