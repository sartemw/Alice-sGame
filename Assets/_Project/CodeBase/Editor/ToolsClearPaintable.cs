using _Project.CodeBase.Services.Repainting;
using UnityEditor;
using UnityEngine;
using Tools = CodeBase.Editor.Tools;

namespace _Project.CodeBase.Editor
{
    public class ToolsClearPaintable : Tools
    {
        [MenuItem("Tools/Paintable/ClearPaintable!!!!!!")]
        public static void ClearPrefs()
        {
            var temp = FindObjectsByType<Paintable>(FindObjectsSortMode.None);
            
            foreach (var paintable in temp)
            {
                Object.DestroyImmediate(paintable);
            }
        }
    }
}