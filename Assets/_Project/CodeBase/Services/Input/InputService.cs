using UnityEngine;

namespace _Project.CodeBase.Services.Input
{
  public abstract class InputService : IInputService
  {
    protected const string Horizontal = "Horizontal";
    protected const string Vertical = "Vertical";
    private const string Button = "Fire";
    
    protected Transform _heroPosition;

    public abstract Vector2 Axis { get; }
    public bool IsAttackButtonUp()
    {
      return SimpleInput.GetButtonUp(Button);
    }

    public void Initialize(Transform hero)
    {
      _heroPosition = hero;
    }

    protected static Vector2 SimpleInputAxis()
    {
      return new Vector2(SimpleInput.GetAxis(Horizontal), SimpleInput.GetAxis(Vertical));
    }
  }
}