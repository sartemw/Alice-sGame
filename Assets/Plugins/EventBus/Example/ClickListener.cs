using Assets.EventBus.Example.Events;
using UnityEngine;

namespace Assets.EventBus.Example
{
    public class ClickListener : MonoBehaviour
    {
        private BaseOnEvent<GreenClickSignal>  _onGreenButtonClick  = new BaseOnEvent<GreenClickSignal>();
        private BaseOnEvent<RedClickSignal>  _onRedButtonClick  = new BaseOnEvent<RedClickSignal>();
        private BaseOnEvent<MoveClickSignal>  _onMoveButtonClick  = new BaseOnEvent<MoveClickSignal>();

        private void Start()
        {
            EventBus.Subscribe(_onGreenButtonClick.SetOnInvoke(OnGreenClick));
            EventBus.Subscribe(_onRedButtonClick.SetOnInvoke(OnRedClick));
            EventBus.Subscribe(_onMoveButtonClick.SetOnInvoke(OnMoveClick));
        }

        private void OnGreenClick(GreenClickSignal obj) => 
            gameObject.GetComponent<MeshRenderer>().material.color = Color.green;

        private void OnRedClick(RedClickSignal obj) =>
            gameObject.GetComponent<MeshRenderer>().material.color = Color.red;

        private void OnMoveClick(MoveClickSignal obj) =>
            gameObject.transform.Translate(obj.direction);

    }
}
