using UnityEngine;

namespace _Project.CodeBase.Enemy.Base
{
  public abstract class Follow : MonoBehaviour
  {
    public float Velocity;
    public float Speed;
    public abstract void MoveToTarget(Vector2 position);
  }
}