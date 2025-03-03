using System.Collections.Generic;
using UnityEngine;

namespace _Project.CodeBase.StaticData.Windows
{
  [CreateAssetMenu(menuName = "Static Data/Window static data", fileName = "WindowStaticData")]
  public class WindowStaticData : ScriptableObject
  {
    public List<WindowConfig> Configs;
  }
}