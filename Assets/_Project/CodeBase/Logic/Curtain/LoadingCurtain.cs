using System.Collections;
using _Project.CodeBase.Events;
using UnityEngine;

namespace _Project.CodeBase.Logic.Curtain
{
  public class LoadingCurtain : MonoBehaviour
  {
    public CanvasGroup Curtain;

    private void Awake()
    {
      DontDestroyOnLoad(this);
    }

    public void Show()
    {
      //gameObject.SetActive(true);
      Curtain.alpha = 1;
      EventBus.Invoke(new ShowCurtainSignal());
    }
    
    public void Hide() => StartCoroutine(DoFadeIn());
    
    private IEnumerator DoFadeIn()
    {
      while (Curtain.alpha > 0)
      {
        Curtain.alpha -= Constants.CurtainAlphaFade;
        yield return new WaitForSeconds(0.03f);
      }
      
      //gameObject.SetActive(false);
    }
  }
}