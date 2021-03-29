using System.Threading;
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


        private async void Start()
        {
            _flowingTextPool = new FlowingTextPool(_flowingTextPrefab, _rootCanvas.transform);

            var server = await ServiceLocator.GetServiceAsync<IServer<string>>();

            await server.WaitUntilConnectAsync(this.GetCancellationTokenOnDestroy());

            _messageReceiver = await server.GetMessageReceiverAsync(this.GetCancellationTokenOnDestroy());

            _messageReceiver
                .OnMessage()
                .ObserveOnMainThread()
                .Subscribe(x =>
                {
                    this.FlowMessage(x).Forget();
                })
                .AddTo(this);
        }

        private async UniTask FlowMessage(string message)
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
