using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
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
        [SerializeField] private TMP_InputField _urlInputField;

        [SerializeField] private TextMeshProUGUI _connectionStatusText;

        private ConnectionSettingUiViewModel _viewModel;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        private async void Start()
        {
            _connectButton.enabled = false;

            var server = await ServiceLocator.ResolveAsync<IServer<string>>();

            _connectButton.enabled = true;


            _viewModel = new ConnectionSettingUiViewModel(server);

            Observable.EveryUpdate()
                .Where(_ => Input.GetKeyDown(KeyCode.U))
                .Subscribe(_ => _connectionSettingCanvas.gameObject.SetActive(!_connectionSettingCanvas.gameObject.activeSelf));


            _connectButton
                .onClick
                .AsObservable()
                .Subscribe(_ =>
                {
                    this.OnClickConnectionButton().Forget();
                });
        }


        private async UniTask OnClickConnectionButton()
        {
            try
            {
                await _viewModel.ConnectToServerAsync(_urlInputField.text, _cancellationTokenSource.Token);
                Debug.Log("after await");
                _connectionStatusText.text = "Connect";
            }
            catch (Exception e)
            {
                Debug.Log("Catch Exception!!!!!!");
                Debug.LogError(e.Message);
            }
        }
    }
}

