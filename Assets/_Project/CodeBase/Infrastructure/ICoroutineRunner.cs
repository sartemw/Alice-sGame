using System.Collections;
using UnityEngine;

namespace _Project.CodeBase.Infrastructure
{
  public interface ICoroutineRunner
  {
    Coroutine StartCoroutine(IEnumerator coroutine);
  }
}