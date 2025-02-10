using System.Collections.Generic;
using System.Linq;
using CodeBase.Data;
using CodeBase.Fish;
using CodeBase.Services.Repainting;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    public class ToolsLevelSettings : Tools
    {
      private static ToolsLevelSettings _window;
      private static Repaintable[] _outlineObjects { get; set; }
      private static FishSpawnMarker[] _fishSpawner;
      private static GameObject _parent;
      private static Dictionary<ColorType, int> _colorTypesObjects = new Dictionary<ColorType, int>();
      private static Dictionary<ColorType, int> _colorTypesFishs = new Dictionary<ColorType, int>();
      
      //Buttons
      private bool _colorButton;
      private bool _clearButton;

      //Settings
      private string _myString = "Hello World";
      private bool _groupEnabled;
      private bool _myBool = true;
      private float _myFloat = 1.23f;

      

      [MenuItem("Tools/LevelSettings")]
      public static void Init()
      {
        _window = (ToolsLevelSettings)EditorWindow.GetWindow(typeof(ToolsLevelSettings));
        _window.Show();
      }
      
      void OnGUI()
      {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        
        //кнопки
        EditorGUILayout.BeginHorizontal("Button",GUILayout.Width(200));
        if (GUILayout.Button("Color", GUILayout.Width(100)) && !_parent) 
          AddOutline();

        if (GUILayout.Button("Clear", GUILayout.Width(100))) 
          ClearOutline();
        EditorGUILayout.EndHorizontal();
        
        //подкрашеные обекты
        EditorGUILayout.BeginHorizontal("box");
        
        var originalFontColor = GUI.contentColor;
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Objects");
        foreach (KeyValuePair<ColorType,int> colorTypesObject in _colorTypesObjects.OrderBy(x => x.Key))
        {
          EditorGUILayout.BeginVertical();
          GUI.contentColor = colorTypesObject.Key.SwitchColor();
          GUILayout.TextArea($"{colorTypesObject.Key} = {colorTypesObject.Value}");
          EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        GUI.contentColor = originalFontColor;
        
        //рыбы
        EditorGUILayout.BeginVertical();
        GUILayout.Label("Fishs");
        foreach (KeyValuePair<ColorType,int> _colorTypesFish in _colorTypesFishs.OrderBy(x => x.Key))
        {
          EditorGUILayout.BeginVertical();
          GUI.contentColor = _colorTypesFish.Key.SwitchColor();
          GUILayout.TextArea($"{_colorTypesFish.Key} = {_colorTypesFish.Value}");
          EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
        GUI.contentColor = originalFontColor;
        
        EditorGUILayout.EndHorizontal();
        
        //данные для сохранения
        _groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", _groupEnabled);
        _myString = EditorGUILayout.TextField("Scene name", _myString = SceneManager.GetActiveScene().name);
        _myBool = EditorGUILayout.Toggle("Toggle", _myBool);
        _myFloat = EditorGUILayout.Slider("Slider", _myFloat, -3, 3);
        EditorGUILayout.EndToggleGroup();
      }

      private static void AddOutline()
      {
        InitOutline();

        Material material = new Material(Shader.Find("Shader Graphs/Outline"));

        foreach (Repaintable repaintable in _outlineObjects) 
          SetMaterialAndCounting(repaintable, material);

        foreach (FishSpawnMarker fishSpawnMarker in _fishSpawner) 
          CountingFish(fishSpawnMarker);
      }

      private static void ClearOutline()
      {
        GameObject.DestroyImmediate(_parent);
        _colorTypesObjects.Clear();
        _colorTypesFishs.Clear();
      }

      void OnDestroy() => 
        ClearOutline();

      private static void InitOutline()
      {
        _outlineObjects = GameObject.FindObjectsOfType<Repaintable>();
        _fishSpawner = GameObject.FindObjectsOfType<FishSpawnMarker>();

        _parent = GameObject.CreatePrimitive(PrimitiveType.Cube);

        foreach (var comp in _parent.GetComponents<Component>())
        {
          if (!(comp is Transform))
          {
            GameObject.DestroyImmediate(comp);
          }
        }

        _parent.AddComponent<Grid>();
      }

      private static void SetMaterialAndCounting(Repaintable repaintable, Material material)
      {
        Renderer outline = GameObject.Instantiate(repaintable.gameObject, repaintable.gameObject.transform.position,
          repaintable.gameObject.transform.rotation, _parent.transform).GetComponent<Renderer>();
        outline.material = material;
        Color color = repaintable.ColorType.SwitchColor();
        outline.material.SetColor("_OuterGlowColor", color);

        if (!_colorTypesObjects.ContainsKey(repaintable.ColorType))
          _colorTypesObjects.Add(repaintable.ColorType, 1);
        else
          _colorTypesObjects[repaintable.ColorType]++;
      }

      private static void CountingFish(FishSpawnMarker fishSpawnMarker)
      {
        if (!_colorTypesFishs.ContainsKey(fishSpawnMarker.ColorType))
          _colorTypesFishs.Add(fishSpawnMarker.ColorType, 1);
        else
          _colorTypesFishs[fishSpawnMarker.ColorType]++;
      }
    }
}