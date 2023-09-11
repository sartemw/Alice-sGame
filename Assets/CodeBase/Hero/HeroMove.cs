using CodeBase.Data;
using CodeBase.Services;
using CodeBase.Services.Input;
using CodeBase.Services.PersistentProgress;
using CodeBase.Services.StaticData;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace CodeBase.Hero
{
  [RequireComponent(typeof(Rigidbody2D), typeof(BoxCollider2D))]
  public class HeroMove : MonoBehaviour, ISavedProgress
  {
    public bool IsPlatformer;
    public Vector2 MovementVector => _movementVector;
    [SerializeField] public Rigidbody2D Rigidbody2D;
    [SerializeField] private BoxCollider2D _collider;
    public float _movementSpeed;


    private Vector2 _movementVector;
    private IInputService _inputService;
    private Camera _camera;
    
    public void Construct(IInputService inputService)
    {
      _inputService = inputService;
    }

    private void Start()
    {
      _camera = Camera.main;
      Rigidbody2D = GetComponent<Rigidbody2D>();
      _collider = GetComponent<BoxCollider2D>();
    }

    private void Update()
    {
      if(_inputService == null)
        return;
      
     _movementVector = Vector2.zero;
      if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
      {
        _movementVector = _camera.transform.TransformDirection(_inputService.Axis);
        _movementVector.Normalize();
        
        if (IsPlatformer)
          _movementVector = _movementVector.BlockYAxis();

        FlipHero();
      }
      Rigidbody2D.MovePosition(_movementSpeed * _movementVector * Time.deltaTime  + Rigidbody2D.position);

    }
  
    public void UpdateProgress(PlayerProgress progress)
    {
      progress.WorldData.PositionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (CurrentLevel() != progress.WorldData.PositionOnLevel.Level) return;

      Vector3Data savedPosition = progress.WorldData.PositionOnLevel.Position;
      if (savedPosition != null) 
        Warp(to: savedPosition);
    }

    private static string CurrentLevel() => 
      SceneManager.GetActiveScene().name;

    private void Warp(Vector3Data to)
    {
      gameObject.SetActive(false);
      transform.position = to.AsUnityVector().AddY(_collider.size.y);
      gameObject.SetActive(true);
    }

    

    private void FlipHero() => 
      transform.right = _movementVector;
    
    public void FlipHero(Vector2 direction) => 
      transform.right = direction;
  }
}