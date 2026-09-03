using _Project.CodeBase.Fish;
using _Project.CodeBase.Logic.Door;
using _Project.CodeBase.StaticData;
using _Project.CodeBase.StaticData.Windows;
using _Project.CodeBase.UI.Services.Windows;

namespace _Project.CodeBase.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    ConfigStaticData ForConfig();
    AudioStaticData ForAudio();
    HeroStaticData ForHero(HeroTypeId typeId);
    MonsterStaticData ForMonster(MonsterTypeId typeId);
    FishStaticData ForFish(ColorType color, FishBehaviourEnum behaviour);
    LevelStaticData ForLevel(string sceneKey);
    WindowConfig ForWindow(WindowId shop);
    PoolObjectStaticData ForPoolObjects(PoolObjectsTypeId poolObjectType);
    DoorStaticData ForDoor(DoorStyles doorStyles);
  }
}