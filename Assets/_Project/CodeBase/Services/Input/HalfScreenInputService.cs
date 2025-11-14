using UnityEngine;

namespace _Project.CodeBase.Services.Input
{
    public class HalfScreenInputService: InputService
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
            
            if (clickPosition.x < _heroPosition.position.x)
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