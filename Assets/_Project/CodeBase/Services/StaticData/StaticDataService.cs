using System.Collections.Generic;
using System.Linq;
using _Project.CodeBase.Fish;
using _Project.CodeBase.Logic.Door;
using _Project.CodeBase.StaticData;
using _Project.CodeBase.StaticData.Windows;
using _Project.CodeBase.UI.Services.Windows;
using UnityEngine;

namespace _Project.CodeBase.Services.StaticData
{
  public class StaticDataService : IStaticDataService
  {
    private const string ConfigDataPath = "Static Data/Config/ConfigData";
    private const string AudioDataPath = "Static Data/Audio/AudioData";
    private const string MonstersDataPath = "Static Data/Monsters";
    private const string LevelsDataPath = "Static Data/Levels";
    private const string StaticDataWindowPath = "Static Data/UI/WindowStaticData";
    private const string PoolObjectsDataPath = "Static Data/PoolObjects";
    private const string FishsDataPath = "Static Data/Fishs";
    private const string HeroDataPath = "Static Data/Heroes";
    private const string DoorsDataPath = "Static Data/Doors";

    private ConfigStaticData _config;
    private AudioStaticData _audio;
    private Dictionary<MonsterTypeId, MonsterStaticData> _monsters;
    private Dictionary<FishBehaviourEnum, FishStaticData> _fishs;
    private Dictionary<string, LevelStaticData> _levels;
    private Dictionary<WindowId, WindowConfig> _windowConfigs;
    private Dictionary<PoolObjectsTypeId, PoolObjectStaticData> _poolObjects;
    private Dictionary<HeroTypeId, HeroStaticData> _hero;
    private Dictionary<DoorStyles, DoorStaticData> _door;


    public void Load()
    {
      _config = Resources.Load<ConfigStaticData>(ConfigDataPath);
      
      _audio = Resources.Load<AudioStaticData>(AudioDataPath);

      _hero = Resources
        .LoadAll<HeroStaticData>(HeroDataPath)
        .ToDictionary(x => x.HeroTypeId, x => x);
      
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
      
      _door = Resources
        .LoadAll<DoorStaticData>(DoorsDataPath)
        .ToDictionary(x => x.DoorStyle, x => x);
    }

    public ConfigStaticData ForConfig() =>
      _config;
    
    public AudioStaticData ForAudio() =>
      _audio;
    
    public HeroStaticData ForHero(HeroTypeId typeId) =>
      _hero.TryGetValue(typeId, out HeroStaticData staticData)
        ? staticData
        : null;
    
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
    
    public DoorStaticData ForDoor(DoorStyles doorStyles) =>
      _door.TryGetValue(doorStyles, out DoorStaticData staticData)
        ? staticData
        : null;
  }
}