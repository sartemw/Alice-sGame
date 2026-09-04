using _Project.CodeBase.Logic.Helpers.Flighter;
using UnityEditor;
using UnityEngine;

namespace _Project.CodeBase.Editor
{
    [CustomEditor(typeof(Flighter)), CanEditMultipleObjects]
    public class FixFlighterEditor : UnityEditor.Editor
    {
        private const string LayerName = "Ground";
        
        Flighter _flighterScript;
        
        SerializedObject _serializedFlighterScript;
        
        SerializedProperty _serializedPositionPointA;
        SerializedProperty _serializedPositionPointB;
        
        SerializedProperty _serializedButterflyA;
        SerializedProperty _serializedButterflyB;
        
        private void OnEnable()
        {
            _flighterScript = target as Flighter;
            
            _serializedFlighterScript = new SerializedObject(_flighterScript);
            
            _serializedPositionPointA = _serializedFlighterScript.FindProperty("PositionPointA");
            _serializedPositionPointB = _serializedFlighterScript.FindProperty("PositionPointB");
            
            _serializedButterflyA = _serializedFlighterScript.FindProperty("ButterflyPointA");
            _serializedButterflyB = _serializedFlighterScript.FindProperty("ButterflyPointB");
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            _serializedFlighterScript.Update();

            DrawCustomInspector(); 

            if (GUI.changed)
            {
                EditorUtility.SetDirty(_flighterScript);
                _serializedFlighterScript.ApplyModifiedProperties();
            }
        }
        private void DrawCustomInspector()
        {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("", GUI.skin.horizontalSlider);
            
            _serializedPositionPointA.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Point A", "Переменная типа Vector2"), _flighterScript.PointA.transform.position);
            _serializedPositionPointB.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Point B", "Переменная типа Vector2"), _flighterScript.PointB.transform.position);
            _flighterScript.PointA.transform.position = _serializedPositionPointA.vector2Value;
            _flighterScript.PointB.transform.position = _serializedPositionPointB.vector2Value;
            
            EditorGUILayout.Space();
            
            _serializedButterflyA.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Butterfly A", "Переменная типа Vector2"), _flighterScript.ButterflyPointA);
            _serializedButterflyB.vector2Value = EditorGUILayout.Vector2Field(new GUIContent("Butterfly B", "Переменная типа Vector2"), _flighterScript.ButterflyPointB);
            
            EditorGUILayout.Space();
            
            if (GUILayout.Button("Отладить позиции", new GUIStyle(GUI.skin.button) { alignment = TextAnchor.MiddleCenter, fixedHeight = 30 }))
            {
                Fix(_flighterScript);
            }
        }
        
        private void Fix(Flighter flighter)
        {
            flighter.ButterflyPointA = FixPosition(flighter.PointA.transform, Constants.HeightButterfly);
            flighter.ButterflyPointB = FixPosition(flighter.PointB.transform, Constants.HeightButterfly);
            
            flighter.PointA.transform.position = FixPosition(flighter.PointA.transform, Constants.HeightHero);
            flighter.PointB.transform.position = FixPosition(flighter.PointB.transform, Constants.HeightHero);
            
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