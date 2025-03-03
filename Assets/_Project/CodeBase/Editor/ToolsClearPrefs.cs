using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
    public class ToolsClearPrefs : Tools
    {
        [MenuItem("Tools/ClearPrefs")]
        public static void ClearPrefs()
        {
            PlayerPrefs.DeleteAll();
            PlayerPrefs.Save();
            
            Debug.Log("<color=yellow> Progress clear</color>");
        }
    }
}