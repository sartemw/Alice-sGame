using System;
using UnityEngine;

namespace _Project.CodeBase.StaticData
{
  [Serializable]
  public class LevelTransferStaticData
  {
    public string TransferTo;
    public Vector3 Position;

    public LevelTransferStaticData(string transferTo, Vector3 position)
    {
      TransferTo = transferTo;
      Position = position;
    }
  }
}