using UnityEngine;
using UnityEngine.EventSystems;

namespace _Project.CodeBase.Services.Input
{
    public class FoolScreenInputService: InputService
    {
        private Vector2 _axis = Vector2.zero;
        public override Vector2 Axis
        {
            get
            {
                return GetAsix(SimpleInput.GetClickPosition);
            }
        }

        private Vector2 GetAsix(Vector2 clickPosition)
        {
            if (clickPosition == Vector2.zero)
                return _axis = Vector2.zero;
            
            Vector2 axis = new Vector2(clickPosition.x/ Screen.width, clickPosition.y / Screen.height);

            if (axis.x < 0.5f && axis != Vector2.zero)
            {
                if (_axis.x > -1)
                    _axis.x -= 0.01f;
            }
            else
            {
                if (_axis.x < 1)
                    _axis.x += 0.01f;
            }
            
            return _axis;
        }
    }
}