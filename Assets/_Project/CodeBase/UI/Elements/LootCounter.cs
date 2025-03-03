using _Project.CodeBase.Data;
using TMPro;
using UnityEngine;

namespace _Project.CodeBase.UI.Elements
{
  public class LootCounter : MonoBehaviour
  {
    public GameObject Counter;
    public GameObject Pickup;
    private WorldData _worldData;

    public void Construct(WorldData worldData)
    {
      _worldData = worldData;
      _worldData.LootData.Changed += UpdateCounter;
    }

    private void UpdateCounter()
    {
      Instantiate(Pickup, Counter.transform);
    }

    private void ClearCounter()
    {
      foreach (Transform g in Counter.transform.GetComponentsInChildren<Transform>())
      {
        Debug.Log(g.name);
      }
    }
  }
}