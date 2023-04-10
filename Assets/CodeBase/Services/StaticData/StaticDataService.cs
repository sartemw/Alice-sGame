using System.Collections.Generic;
using System.Linq;
using CodeBase.Fish;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string MonstersDataPath = "Static Data/Monsters";
    private const string LevelsDataPath = "Static Data/Levels";
    private const string StaticDataWindowPath = "Static Data/UI/WindowStaticData";
    private const string PoolObjectsDataPath = "Static Data/PoolObjects";
    private const string FishsDataPath = "Static Data/Fishs";
    
    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
    private Dictionary<FishBehaviourEnum, FishStaticData> _fishs;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WindowId, WindowConfig> _windowConfigs;
    private Dictionary<PoolObjectsTypeId, PoolObjectStaticData> _poolObjects;

    public void Load()
    {
      _monsters = Resources
        .LoadAll<MonsterStaticData>(MonstersDataPath)
        .ToDictionary(x => x.MonsterTypeId, x => x);

      _levels = Resources
        .LoadAll<LevelStaticData>(LevelsDataPath)
        .ToDictionary(x => x.LevelKey, x => x);

      _windowConfigs = Resources
        .Load<WindowStaticData>(StaticDataWindowPath)
        .Configs
        .ToDictionary(x => x.WindowId, x => x);
      
      _poolObjects = Resources
        .LoadAll<PoolObjectStaticData>(PoolObjectsDataPath)
        .ToDictionary(x => x.PoolObjectTypeId, x => x);
      
      _fishs = Resources
        .LoadAll<FishStaticData>(FishsDataPath)
        .ToDictionary(x => x.FishBehaviour, x => x);
    }

    public MonsterStaticData ForMonster(MonsterTypeId typeId) =>
      _monsters.TryGetValue(typeId, out MonsterStaticData staticData)
        ? staticData
        : null;

    public FishStaticData ForFish(ColorType color, FishBehaviourEnum behaviour) =>
      _fishs.TryGetValue(behaviour, out FishStaticData staticData)
        ? staticData
        : null;

    public LevelStaticData ForLevel(string sceneKey) =>
      _levels.TryGetValue(sceneKey, out LevelStaticData staticData)
        ? staticData
        : null;

    public WindowConfig ForWindow(WindowId windowId) =>
      _windowConfigs.TryGetValue(windowId, out WindowConfig windowConfig)
        ? windowConfig
        : null;

    public PoolObjectStaticData ForPoolObjects(PoolObjectsTypeId poolObjectType) =>
      _poolObjects.TryGetValue(poolObjectType, out PoolObjectStaticData staticData)
        ? staticData
        : null;
  }
}