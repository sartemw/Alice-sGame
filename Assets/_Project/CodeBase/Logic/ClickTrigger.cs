using System;
using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.CodeBase.Logic
{
    public class ClickTrigger : MonoBehaviour, IPointerUpHandler
    {
        public event Action TriggeredClick;
        private void Update()
        {
            if (Input.GetMouseButtonUp(0))
                OnClickUp();
        }

        private void OnClickUp()
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit.collider)
                if (hit.collider.gameObject.GetComponent<ClickTrigger>())
                    TriggeredClick?.Invoke();
        }

        public void OnPointerUp(PointerEventData eventData) => 
            OnClickUp();
    }
}