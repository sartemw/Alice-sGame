using Assets.EventBus.Example.Events;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.EventBus.Example
{
    public class Clicker : MonoBehaviour
    {
        [SerializeField] private Button _greenButton;
        [SerializeField] private Button _redButton;
        [SerializeField] private Button _moveButton;

        private void Start()
        {
            _greenButton.onClick.AddListener(GreenClick);
            _redButton.onClick.AddListener(RedClick);
            _moveButton.onClick.AddListener(MoveClick);
        }

        private void GreenClick() =>
            EventBus.Invoke(new GreenClickSignal());

        void RedClick() =>
            EventBus.Invoke(new RedClickSignal());

        private void MoveClick() =>
            EventBus.Invoke(new MoveClickSignal
            {
                direction = new Vector3(Random.Range(-1,1), Random.Range(-1,1), 0)
            });
    }
}
