using System.Linq;
using _Project.CodeBase.Fish;
using _Project.CodeBase.Logic;
using _Project.CodeBase.Logic.EnemySpawners;
using _Project.CodeBase.StaticData;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace CodeBase.Editor
{
  [CustomEditor(typeof(LevelStaticData))]
  public class LevelStaticDataEditor : UnityEditor.Editor
  {
    private const string InitialPointTag = "InitialPoint";
    private const string LevelTransferInitialPointTag = "LevelTransferInitialPoint";
    
    public override void OnInspectorGUI()
    {
      base.OnInspectorGUI();

      LevelStaticData levelData = (LevelStaticData) target;

      if (GUILayout.Button("Collect"))
      {
        levelData.EnemySpawners = FindObjectsByType<SpawnMarker>(FindObjectsSortMode.None)
          .Select(x => new EnemySpawnerStaticData(x.GetComponent<UniqueId>().Id, x.MonsterTypeId, x.transform.position))
          .ToList();
        
        levelData.FishSpawners = FindObjectsByType<FishSpawnMarker>(FindObjectsSortMode.None)
          .Select(x => new FishSpawnerStaticData(x.GetComponent<UniqueId>().Id, x.ColorType, x.FishBehaviour, x.transform.position))
          .ToList();
        
        levelData.LevelKey = SceneManager.GetActiveScene().name;

        
        if (GameObject.FindWithTag(InitialPointTag) != null)
          levelData.InitialHeroPosition =  GameObject.FindWithTag(InitialPointTag).transform.position;
        else
          levelData.InitialHeroPosition = Vector3.zero;

        if (GameObject.FindWithTag(LevelTransferInitialPointTag) != null)
          levelData.LevelTransfer.Position = GameObject.FindWithTag(LevelTransferInitialPointTag).transform.position;
        else
          levelData.LevelTransfer.Position = Vector3.zero;
      }
      
      EditorUtility.SetDirty(target);
    }
  }
}