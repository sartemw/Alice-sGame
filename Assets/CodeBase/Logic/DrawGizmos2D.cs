using System;
using CodeBase.Data;
using CodeBase.Fish;
using UnityEngine;

namespace CodeBase.Logic
{
  public class DrawGizmos2D : MonoBehaviour
  {
    public BoxCollider2D BoxCollider;
    public CircleCollider2D CircleCollider;
    public float Size;

    public ColorType ColorsGizmo;

    private void OnDrawGizmos()
    {
      Gizmos.color = ColorsGizmo.SwitchColor();

      if (CircleCollider)
        Gizmos
          .DrawSphere
            (GetCenter(CircleCollider.transform, CircleCollider)
              ,CircleCollider.radius);

      if (BoxCollider)
        Gizmos
          .DrawCube
          (GetCenter(BoxCollider.transform, BoxCollider)
            ,BoxCollider.size);
      
      if (!CircleCollider & !BoxCollider)
        Gizmos
          .DrawSphere
          (transform.position, Size);

    }

    private Vector2 GetCenter(Transform colliderObject, Collider2D collider2D)
    {
      return new Vector2(
        colliderObject.position.x + collider2D.offset.x,
        colliderObject.position.y + collider2D.offset.y);
    }
  }
}
