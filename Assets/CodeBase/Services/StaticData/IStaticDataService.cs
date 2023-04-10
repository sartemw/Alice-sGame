using CodeBase.Fish;
using CodeBase.StaticData;
using CodeBase.StaticData.Windows;
using CodeBase.UI.Services.Windows;
using UnityEngine;

namespace CodeBase.Services.StaticData
{
  public interface IStaticDataService : IService
  {
    void Load();
    MonsterStaticData ForMonster(MonsterTypeId typeId);
    FishStaticData ForFish(ColorType color, FishBehaviourEnum behaviour);
    LevelStaticData ForLevel(string sceneKey);
    WindowConfig ForWindow(WindowId shop);
    PoolObjectStaticData ForPoolObjects(PoolObjectsTypeId poolObjectType);
  }
}