using _Project.CodeBase.Logic.Helpers.Jumper;
using UnityEditor;
using UnityEngine;

namespace _Project.CodeBase.Editor
{
    [CustomEditor(typeof(Jumper)), CanEditMultipleObjects]
    public class FixJumperEditor : UnityEditor.Editor
    {
        private const string LayerName = "Ground";
        
        Jumper _jumperScript;
        
        SerializedObject _serializedJumperScript;
        
        SerializedProperty _serializedPositionPointA;
        SerializedProperty _serializedPositionPointB;
        
        SerializedProperty _serializedHopperA;
        SerializedProperty _serializedHopperB;
        
        private void OnEnable()
        {
            _jumperScript = target as Jumper;
            
            _serializedJumperScript = new SerializedObject(_jumperScript);
            
            _serializedPositionPointA = _serializedJumperScript.FindProperty("PositionPointA");
            _serializedPositionPointB = _serializedJumperScript.FindProperty("PositionPointB");
            
            _serializedHopperA = _serializedJumperScript.FindProperty("HopperPointA");
            _serializedHopperB = _serializedJumperScript.FindProperty("HopperPointB");
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _serializedJumperScript.Update();

            DrawCustomInspector(); 

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_jumperScript);
                _serializedJumperScript.ApplyModifiedProperties();
            }
        }
        private void DrawCustomInspector()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            _serializedPositionPointA.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Point A", "Переменная типа Vector2"), _jumperScript.PointA.transform.position);
            _serializedPositionPointB.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Point B", "Переменная типа Vector2"), _jumperScript.PointB.transform.position);
            _jumperScript.PointA.transform.position = _serializedPositionPointA.vector2Value;
            _jumperScript.PointB.transform.position = _serializedPositionPointB.vector2Value;
            
            EditorGUILayout.Space();
            
            _serializedHopperA.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Hopper A", "Переменная типа Vector2"), _jumperScript.HopperPointA);
            _serializedHopperB.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Hopper B", "Переменная типа Vector2"), _jumperScript.HopperPointB);
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Отладить позиции", new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 }))
            {
                Fix(_jumperScript);
            }
        }
        
        private void Fix(Jumper jumper)
        {
            jumper.HopperPointA = FixPosition(jumper.PointA.transform, Constants.HeightHopper);
            jumper.HopperPointB = FixPosition(jumper.PointB.transform, Constants.HeightHopper);
            
            jumper.PointA.transform.position = FixPosition(jumper.PointA.transform, Constants.HeightHero);
            jumper.PointB.transform.position = FixPosition(jumper.PointB.transform, Constants.HeightHero);
            
        }

        private Vector2 FixPosition(Transform point, float height)
        {
            RaycastHit2D hit = Physics2D.Raycast(point.transform.position, Vector2.down, 5, LayerMask.GetMask(LayerName));
            Vector2 newPosition = Vector2.zero;
            if (hit)
            {
                float distance = hit.distance;
                
                if (distance > height)
                {
                    float deltaY = distance - height;
                    newPosition = new Vector2(point.transform.position.x, point.transform.position.y - deltaY);
                }
                else
                {
                    float deltaY = height - distance;
                    newPosition = new Vector2(point.transform.position.x, point.transform.position.y + deltaY);
                }
            }
            return newPosition;
        }
    }
}