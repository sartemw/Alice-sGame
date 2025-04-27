using UnityEngine;

namespace _Project.CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "ConfigData", menuName = "Static Data/Config")]
    public class ConfigStaticData : ScriptableObject
    {
        public bool Sound = true;
        
        public Gradient Rainbow;
        public Gradient Red;
        public Gradient Green;
        public Gradient Blue;
        public Gradient Yellow;
        public Gradient Cyan;
        public Gradient Purple;
    }
}