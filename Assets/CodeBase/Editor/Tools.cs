using System.Collections.Generic;
using CodeBase.Data;
using CodeBase.Fish;
using CodeBase.Services.Repainting;
using UnityEditor;
using UnityEngine;

namespace CodeBase.Editor
{
  public class Tools : EditorWindow
  {
    private string _myString = "Hello World";
    private bool _groupEnabled;
    private bool _myBool = true;
    private float _myFloat = 1.23f;
    
    private static Repaintable[] _outlineObjects { get; set; }
    private static GameObject _parent;
    public static List<ColorType> ColorTypesObjects;

    [MenuItem("Tools/ClearPrefs")]
    public static void ClearPrefs()
    {
      PlayerPrefs.DeleteAll();
      PlayerPrefs.Save();
    }

    [MenuItem("Tools/Outline/AddOutline")]
    static void Init()
    {
      Tools window = (Tools)EditorWindow.GetWindow(typeof(Tools));
      window.Show();
      
      AddOutline();
    }
    
    void OnGUI()
    {
      GUILayout.Label("Base Settings", EditorStyles.boldLabel);
      _myString = EditorGUILayout.TextField("Text Field", _myString);

      _groupEnabled = EditorGUILayout.BeginToggleGroup("Optional Settings", _groupEnabled);
      _myBool = EditorGUILayout.Toggle("Toggle", _myBool);
      _myFloat = EditorGUILayout.Slider("Slider", _myFloat, -3, 3);
      EditorGUILayout.EndToggleGroup();
    }

    [MenuItem("Tools/Outline/ClearOutline")]
    public static void ClearOutline() => 
      GameObject.DestroyImmediate(_parent);

    private static void AddOutline()
    {
      _outlineObjects = GameObject.FindObjectsOfType<Repaintable>();
      _parent = GameObject.CreatePrimitive(PrimitiveType.Cube);
      
      foreach (var comp in _parent.GetComponents<Component>())
      {
        if (!(comp is Transform))
        {
          GameObject.DestroyImmediate(comp);
        }
      }
      _parent.AddComponent<Grid>();
      Material material = new Material(Shader.Find("Shader Graphs/Outline"));

      foreach (Repaintable repaintable in _outlineObjects)
      {
        Renderer outline = GameObject.Instantiate(repaintable.gameObject, repaintable.gameObject.transform.position,
          repaintable.gameObject.transform.rotation, _parent.transform).GetComponent<Renderer>();
        outline.material = material;
        Color color = repaintable.ColorType.SwitchColor();
        outline.material.SetColor("_OuterGlowColor", color);
      }
    }
  }
}
