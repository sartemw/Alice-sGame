using _Project.CodeBase.Logic.Door;
using UnityEngine;

namespace _Project.CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "DoorData", menuName = "Static Data/Door")]
    public class DoorStaticData : ScriptableObject
    {
        public DoorStyles DoorStyle;
        public Sprite OpenDoor;
        public Sprite ClosedDoor;
        public Sprite Frame;
    }
}