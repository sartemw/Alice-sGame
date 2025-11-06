using System.Collections.Generic;
using _Project.CodeBase.Data;
using _Project.CodeBase.Enemy;
using _Project.CodeBase.Infrastructure.Factory;
using _Project.CodeBase.Services.PersistentProgress;
using _Project.CodeBase.StaticData;
using UnityEngine;

namespace _Project.CodeBase.Logic.EnemySpawners
{
  public class EnemySpawnPoint : MonoBehaviour, ISavedProgress
  {
    public MonsterTypeId MonsterTypeId;
    
    public string Id { get; set; }

    private IGameFactory _factory;
    
    private EnemyDeath _enemyDeath;

    private bool _slain;
    private bool _isCutscene;

    public void Construct(IGameFactory gameFactory, bool isCutscene = false)
    {
      _factory = gameFactory;
      _isCutscene = isCutscene;
    }

    private void OnDestroy()
    {
      if (_enemyDeath != null)
        _enemyDeath.Happened -= Slay;
    }

    public void LoadProgress(PlayerProgress progress)
    {
      if (progress.KillData.ClearedSpawners.Contains(Id))
        _slain = true;
      else 
        if (!_isCutscene)
          Spawn();
        else
          SpawnCutscene();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
      List<string> slainSpawnersList = progress.KillData.ClearedSpawners;
      
      if(_slain && !slainSpawnersList.Contains(Id))
        slainSpawnersList.Add(Id);
    }

    private async void Spawn()
    {
      GameObject monster = await _factory.CreateMonster(MonsterTypeId, transform);
      _enemyDeath = monster.GetComponent<EnemyDeath>();
      _enemyDeath.Happened += Slay;
    }
    
    private async void SpawnCutscene()
    {
      await _factory.CreateCutsceneMonster(MonsterTypeId, transform);
    }

    private void Slay()
    {
      if (_enemyDeath != null)
        _enemyDeath.Happened -= Slay;
      
      _slain = true;
    }
  }
}