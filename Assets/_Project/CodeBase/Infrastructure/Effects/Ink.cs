using System.Collections;
using System.Data;
using _Project.CodeBase.Data;
using _Project.CodeBase.Events;
using _Project.CodeBase.Fish;
using _Project.CodeBase.Services.Audio;
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
        private ParticleSystem _particle;

        [SerializeField] private float _speed;
        [SerializeField] private GameObject _sparks;
        private ConfigStaticData _config;
        private IAudioService _audioService;


        public void Construct(Vector2 moveTo, Paintable coloredObj, IPaintingService paintingService,
            ConfigStaticData config, IAudioService audioService)
        {
            _audioService = audioService;
            _data = new InkData
            {
                Target = coloredObj,
                MoveTo = moveTo
            };
            CreateSparks();
            _paintingService = paintingService;
            _config = config;
            //_effect = GetComponent<VisualEffect>();
            _particle = GetComponent<ParticleSystem>();

            SetColorOverLifeTime(coloredObj);
            GetComponent<SpriteRenderer>().color = coloredObj.ColorType.SwitchColor();
            //_effect.SetGradient(BaseGradient, SetGradient(coloredObj.ColorType));

            RotateTo(moveTo);
            //StartCoroutine(MoveEffect());
            
            MoveTo(moveTo);
        }

        private void RotateTo(Vector2 moveTo)
        {
            Vector2 target = moveTo;
            Vector2 inkPos = transform.position;
            target.x -= inkPos.x;
            target.y -= inkPos.y;
            float angle = Mathf.Atan2(target.x, target.y) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(new Vector3(0, 0, -angle-180));
            gameObject.transform.rotation = targetRotation;
        }

        private void SetColorOverLifeTime(Paintable coloredObj)
        {
            ParticleSystem.ColorOverLifetimeModule col = _particle.colorOverLifetime;
            col.enabled = true;
            col.color = SetGradient(coloredObj.ColorType);
        }

        private void MoveTo(Vector2 moveTo) => 
            transform.DOMove(moveTo, _speed).SetEase(Ease.InQuad).OnComplete(StartPainting);

        private void StartPainting()
        {
            EventBus.Invoke(new StartPaintingSignal {Target = _data.Target});

            Death();
        }

        private void Death()
        {
            _audioService.PlayBlobs();
            CreateSparks();
            Destroy(gameObject);
        }

        public void CreateSparks() => 
            Instantiate(_sparks, transform.position, transform.rotation);

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

        private IEnumerator MoveEffect()
        {
            while (true)
            {
                _effect.SetVector2(BaseVelocity, _effect.GetVector2(BaseVelocity) + _data.MoveTo.normalized/10);

                yield return new WaitForSeconds(0.5f);
            }
        }

        private void CreateInkEffect() => 
            Instantiate(_sparks, transform.position, transform.rotation);
    }
}