using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UniRx;

namespace Rayn
{
    public class FlowingTextController : MonoBehaviour
    {
        [SerializeField] private FlowingText _flowingTextPrefab;
        [SerializeField] private Canvas _rootCanvas;

        private FlowingTextPool _flowingTextPool;

        private IMessageReceiver<string> _messageReceiver;
        private IFlowingTextSettings _flowingTextSettings;
        private float _fontSize = 50f;

        private CancellationToken _cancellationToken;

        private async void Start()
        {
            _cancellationToken = this.GetCancellationTokenOnDestroy();
            _flowingTextPool = new FlowingTextPool(_flowingTextPrefab, _rootCanvas.transform);

            _flowingTextSettings = await ServiceLocator.GetServiceAsync<IFlowingTextSettings>(_cancellationToken);

            _flowingTextSettings.OnFontSizeChange()
                .Subscribe(x => { _fontSize = x; }).AddTo(this);

            var server = await ServiceLocator.GetServiceAsync<IServer<string>>(_cancellationToken);

            await server.WaitUntilConnectAsync(_cancellationToken);

            _messageReceiver = await server.GetMessageReceiverAsync(_cancellationToken);

            _messageReceiver
                .OnMessage()
                .ObserveOnMainThread()
                .Subscribe(x => { this.FlowMessage(x).Forget(); })
                .AddTo(this);
        }

        private async UniTask FlowMessage(string message)
        {
            var obj = _flowingTextPool.Rent();

            obj.Init(_rootCanvas, message, fontSize: _fontSize);
            obj.gameObject.SetActive(true);

            await obj.PlayAnimation(_cancellationToken);

            obj.gameObject.SetActive(false);

            _flowingTextPool.Return(obj);
        }
    }
}