using UnityEditor;
using UnityEngine;
using UnityEngine.Serialization;

namespace _Project.CodeBase.Enemy.Base
{
    public class MovingMarker : MonoBehaviour
    {
        public PointTypeId PointTypeId;

        private void OnDrawGizmos() =>
            Handles.Label(gameObject.transform.position + new Vector3(0, 1f, 0),
                TextInGizmo(PointTypeId));

        private static string TextInGizmo(PointTypeId pointTypeId)
        {
            string text = "Мой текст";
            switch (pointTypeId)
            {
                case PointTypeId.StartPosition:
                    text = "Start";
                    break;
                case PointTypeId.EndPosition:
                    text = "End";
                    break;
                case PointTypeId.Position_1:
                    text = "1";
                    break;
                case PointTypeId.Position_2:
                    text = "2";
                    break;
                case PointTypeId.Position_3:
                    text = "3";
                    break;
                case PointTypeId.Position_4:
                    text = "4";
                    break;
                case PointTypeId.Position_5:
                    text = "5";
                    break;
                default:
                    break;
            }
            return text;
        }
    }
}