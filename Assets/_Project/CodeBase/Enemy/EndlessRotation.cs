using UnityEngine;

namespace _Project.CodeBase.Enemy
{
  public class EndlessRotation : MonoBehaviour
  {
    public float Speed = 1.0f;

    private void Update() => 
      transform.rotation *= Quaternion.Euler(0, Speed*Time.deltaTime, 0);
  }
}