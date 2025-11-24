using System.Collections.Generic;
using System.Linq;
using _Project.CodeBase;
using _Project.CodeBase.Data;
using _Project.CodeBase.Events;
using _Project.CodeBase.Fish;
using _Project.CodeBase.Logic;
using _Project.CodeBase.Logic.EnemySpawners;
using _Project.CodeBase.Services.Repainting;
using _Project.CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
    public class ToolsLevelSettings : Tools
    {
      private static ToolsLevelSettings _window;
      private static Paintable[] _outlineObjects { get; set; }
      private static FishSpawnMarker[] _fishSpawnersToOutline;
      private static GameObject _parent;
      private static Dictionary<ColorType, int> _colorTypesObjects = new Dictionary<ColorType, int>();
      private static Dictionary<ColorType, int> _colorTypesFishs = new Dictionary<ColorType, int>();
      
      //Buttons
      private bool _colorButton;
      private bool _clearButton;

      //Collect
      private const string LevelsDataPath = "Static Data/Levels/";
      private const string InitialPointTag = "InitialPoint";
      private const string LevelTransferInitialPointTag = "LevelTransferInitialPoint";
      private bool _toggleGroup;
      private static bool _reloadLevelKey;
      private static string _levelKey;
      private static string _transferTo;
      private static List<EnemySpawnerStaticData> _enemySpawners = new List<EnemySpawnerStaticData>();
      private static List<FishSpawnerStaticData> _fishSpawners = new List<FishSpawnerStaticData>();
      private static Vector3 _initialHeroPosition;
      private static LevelTransferStaticData _levelTransfer;
      private static string _message;
      private static bool _containsKey;


      [MenuItem("Tools/LevelSettings")]
      public static void Init()
      {
        _window = (ToolsLevelSettings)EditorWindow.GetWindow(typeof(ToolsLevelSettings));
        _window.Show();
      }
      
      void OnGUI()
      {
        GUILayout.Label("Base Settings", EditorStyles.boldLabel);
        
        IsNewLevel();
        
        if (!_reloadLevelKey) 
          CountRepaintableAndFish();

        //кнопки
        EditorGUILayout.BeginHorizontal("Button",GUILayout.Width(200));
        if (GUILayout.Button("Outline", GUILayout.Width(100)) && !_parent) 
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

        if (_containsKey)
          GUI.contentColor = Color.green;
        else
          GUI.contentColor = Color.red;
        GUILayout.TextArea($"{_message}");
        GUI.contentColor = originalFontColor;
        
        //данные для сохранения
        _toggleGroup = EditorGUILayout.BeginToggleGroup("Collect", _toggleGroup);
        
        _levelKey = SceneManager.GetActiveScene().name;

        Dictionary<string, LevelStaticData> levels = Resources
          .LoadAll<LevelStaticData>(LevelsDataPath)
          .ToDictionary(x => x.LevelKey, x => x);
        
        _containsKey = levels.ContainsKey(_levelKey);

        if (!_containsKey)
        {
          _reloadLevelKey = true;
          _message = "Level is not found";
          EditorGUILayout.EndToggleGroup();
          return;
        }
        
        Vector3 levelTransferInitialPoint = CollectLevelData();

        if (!_reloadLevelKey)
        {
          _reloadLevelKey = true;
          _message = "Level is found";
          LevelStaticData level = levels[_levelKey];
          _transferTo = level.LevelTransfer.TransferTo;
        }

          EditorGUILayout.BeginHorizontal("box");
        GUILayout.TextArea("Enemy spawners", GUILayout.Width(120));
        GUILayout.TextArea($"{_enemySpawners.Count.ToString()}", GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal("box");
        GUILayout.TextArea("Fish spawners", GUILayout.Width(120));
        GUILayout.TextArea($"{_fishSpawners.Count.ToString()}", GUILayout.Width(30));
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.TextField("Scene name", _levelKey);
        
        EditorGUILayout.TextField("Initial hero position", _initialHeroPosition.ToString()); 
        
        _transferTo = EditorGUILayout.TextField("Transfer to", _transferTo);
        _levelTransfer = new LevelTransferStaticData(_transferTo, levelTransferInitialPoint);
        EditorGUILayout.TextField("Level transfer initial point", _levelTransfer.Position.ToString());
        
        if (GUILayout.Button("Save", GUILayout.Width(100))) 
          SaveLevel();

        EditorGUILayout.EndToggleGroup();
      }

      private static void CountRepaintableAndFish()
      {
        _colorTypesObjects.Clear();
        _colorTypesFishs.Clear();

        _outlineObjects = GameObject.FindObjectsByType<Paintable>(FindObjectsSortMode.None);
        _fishSpawnersToOutline = GameObject.FindObjectsByType<FishSpawnMarker>(FindObjectsSortMode.None);

        foreach (Paintable repaintable in _outlineObjects)
          CountingRepaintable(repaintable);
        foreach (FishSpawnMarker fishSpawnMarker in _fishSpawnersToOutline)
          CountingFish(fishSpawnMarker);
      }

      private static void IsNewLevel()
      {
        if (_levelKey != SceneManager.GetActiveScene().name)
          _reloadLevelKey = false;
      }

      private static void AddOutline()
      {
        InitOutline();

        Material material = new Material(Shader.Find("Shader Graphs/Outline"));

        foreach (Paintable repaintable in _outlineObjects) 
          SetMaterial(repaintable, material);
      }

      private static void ClearOutline() => 
        GameObject.DestroyImmediate(_parent);

      void OnDestroy() => 
        ClearOutline();

      private static void InitOutline()
      {
        _parent = GameObject.CreatePrimitive(PrimitiveType.Cube);

        foreach (var comp in _parent.GetComponents<Component>())
          if (!(comp is Transform))
            GameObject.DestroyImmediate(comp);

        _parent.AddComponent<Grid>();
      }

      private static void SetMaterial(Paintable paintable, Material material)
      {
        Renderer outline = GameObject.Instantiate(paintable.gameObject, paintable.gameObject.transform.position,
          paintable.gameObject.transform.rotation, _parent.transform).GetComponent<Renderer>();
        outline.material = material;
        Color color = paintable.ColorType.SwitchColor();
        outline.material.SetColor("_OuterGlowColor", color);
      }

      private static void CountingRepaintable(Paintable paintable)
      {
        if (!_colorTypesObjects.ContainsKey(paintable.ColorType))
          _colorTypesObjects.Add(paintable.ColorType, 1);
        else
          _colorTypesObjects[paintable.ColorType]++;
      }

      private static void CountingFish(FishSpawnMarker fishSpawnMarker)
      {
        if (!_colorTypesFishs.ContainsKey(fishSpawnMarker.ColorType))
          _colorTypesFishs.Add(fishSpawnMarker.ColorType, 1);
        else
          _colorTypesFishs[fishSpawnMarker.ColorType]++;
      }

      private static Vector3 CollectLevelData()
      {
        _enemySpawners =  FindObjectsByType<SpawnMarker>(FindObjectsSortMode.None)
          .Select(x => new EnemySpawnerStaticData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position))
          .ToList();
        
        _fishSpawners = FindObjectsByType<FishSpawnMarker>(FindObjectsSortMode.None)
          .Select(x =>
            new FishSpawnerStaticData(x.GetComponent<UniqueId>().Id, x.ColorType, x.FishBehaviour, x.transform.position))
          .ToList();
        
        _initialHeroPosition = GameObject.FindWithTag(InitialPointTag).transform.position;
        
        Vector3 levelTransferInitialPoint = GameObject.FindWithTag(LevelTransferInitialPointTag).transform.position;
        
        return levelTransferInitialPoint;
      }

      private static void SaveLevel()
      {
        EventBus.Invoke(new ClickCollectStaticDataSignal());
        
        LevelStaticData level = ScriptableObject.CreateInstance<LevelStaticData>();

        level.EnemySpawners = _enemySpawners;
        level.FishSpawners = _fishSpawners;
        level.LevelKey = _levelKey;
        level.LevelTransfer = _levelTransfer;
        level.InitialHeroPosition = _initialHeroPosition;

        string levelPath = "Assets/Resources/" + LevelsDataPath + _levelKey + ".asset";

        AssetDatabase.DeleteAsset(levelPath);
        AssetDatabase.CreateAsset(level, levelPath);
      }
    }
}