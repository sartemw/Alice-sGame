using UnityEngine;

namespace _Project.CodeBase.StaticData
{
    [CreateAssetMenu(fileName = "ConfigData", menuName = "Static Data/Config")]
    public class ConfigStaticData : ScriptableObject
    {
        public bool IsDebug = true;
        public bool Sound = true;
        public string Language;

        public Gradient Rainbow;
        public Gradient Red;
        public Gradient Green;
        public Gradient Blue;
        public Gradient Yellow;
        public Gradient Cyan;
        public Gradient Purple;

        public float GetDeltaSpeed()
        {
            switch (Application.platform)
            {
                case RuntimePlatform.WindowsEditor:
                case RuntimePlatform.WindowsPlayer:
                    Debug.Log("Unity Editor");
                    return 1.0f; // Например, для Windows
            
                case RuntimePlatform.Android:
                    Debug.Log("android");
                    return 0.55f; // Например, для Android
            
                case RuntimePlatform.WebGLPlayer:
                    Debug.Log("web");
                    return 1.2f; // Например, для Web
            
                default:
                    return 1.0f; // Значение по умолчанию
            }
        }
    }
}