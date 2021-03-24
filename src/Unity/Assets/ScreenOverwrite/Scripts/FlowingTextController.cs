using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

namespace ScreenOverwriter
{
    public class FlowingTextController : MonoBehaviour
    {
        [SerializeField] private FlowingText _flowingTextPrefab;
        [SerializeField] private Canvas _rootCanvas;

        private FlowingTextPool _flowingTextPool;

        private IMessageReceiver<string> _messageReceiver;

        private void Start()
        {
            _flowingTextPool = new FlowingTextPool(_flowingTextPrefab, _rootCanvas.transform);
            ServiceLocator.Notificator.OnSetMessageReceiver().Subscribe(x =>
            {
                _messageReceiver = x;
                _messageReceiver.OnMessage().Subscribe(x => FlowMessage(x).Forget());
            });
        }

        public async UniTask FlowMessage(string message)
        {
            var obj = _flowingTextPool.Rent();

            obj.Init(_rootCanvas, message);
            obj.gameObject.SetActive(true);

            await obj.PlayAnimation();

            obj.gameObject.SetActive(false);

            _flowingTextPool.Return(obj);
        }
    }
}
