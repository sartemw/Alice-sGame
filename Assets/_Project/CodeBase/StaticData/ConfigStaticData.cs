using UnityEngine;

namespace _Project.CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "ConfigData", menuName = "Static Data/Config")]
    public class ConfigStaticData : ScriptableObject
    {
        public bool Sound = true;
    }
}