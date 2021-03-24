using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenOverwriter
{
    public class ConnectionSettingUi : MonoBehaviour
    {
        [SerializeField] private Canvas _connectionSettingCanvas;

        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _genUrlButton;

        [SerializeField] private TextMeshProUGUI _connectionStatusText;

        private IServer _server;
        private INotificator _notificator;

        // Start is called before the first frame update
        void Start()
        {
            _server = ServiceLocator.Server;
            _notificator = ServiceLocator.Notificator;

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.U))
                .Subscribe(_ => _connectionSettingCanvas.gameObject.SetActive(!_connectionSettingCanvas.gameObject.activeSelf));


            _connectButton
                .onClick
                .AsObservable()
                .Subscribe(_ =>
                {
                    this.OnClickConnection().Forget();
                });
        }

        private async UniTaskVoid OnClickConnection()
        {
            var receiver = await _server.ConnectAsync();
            _connectionStatusText.text = "Connect";
            _notificator.SetMessageReceiver(receiver);
        }
    }
}

