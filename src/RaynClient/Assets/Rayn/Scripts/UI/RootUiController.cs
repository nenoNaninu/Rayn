using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kirurobo;
using TMPro;
using UniRx;
using UniRx.Triggers;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Rayn
{
    public class RootUiController : MonoBehaviour
    {
        [SerializeField] private Canvas _connectionSettingCanvas;

        [SerializeField] private Button _connectButton;
        [SerializeField] private Button _transparentButton;
        [SerializeField] private Button _closeButton;
        [SerializeField] private Slider _fontSizeSlider;
        [SerializeField] private TMP_InputField _urlInputField;

        [SerializeField] private TextMeshProUGUI _connectionStatusText;

        [SerializeField] private UniWindowController _uniWindowController;

        private ConnectionSettingUiViewModel _viewModel;
        private readonly CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private IFlowingTextSettings _flowingTextSettings;

        private async void Start()
        {
            _connectButton.enabled = false;

            var server = await ServiceLocator.GetServiceAsync<IServer<string>>();

            _connectButton.enabled = true;

            _viewModel = new ConnectionSettingUiViewModel(server);

            _connectButton
                .onClick
                .AsObservable()
                .Subscribe(_ => { this.OnClickConnectionButton().Forget(); }).AddTo(this);

            _transparentButton.onClick
                .AsObservable()
                .Subscribe(_ => { this.OnClickTransparentButton(); }).AddTo(this);

            _closeButton.onClick
                .AsObservable()
                .Subscribe(_ => this.Close().Forget())
                .AddTo(this);

            _flowingTextSettings = await ServiceLocator.GetServiceAsync<IFlowingTextSettings>();

            _fontSizeSlider.onValueChanged
                .AsObservable()
                .Subscribe(this.OnFontSizeChange)
                .AddTo(this);

            await UniTask.NextFrame();

            _flowingTextSettings.FontSize = _fontSizeSlider.value;
        }

        private void OnClickTransparentButton()
        {
#if !UNITY_EDITOR
            _uniWindowController.isTransparent = !_uniWindowController.isTransparent;
            _uniWindowController.isTopmost = !_uniWindowController.isTopmost;
            _uniWindowController.isClickThrough = !_uniWindowController.isClickThrough;
#endif
        }

        private async UniTask OnClickConnectionButton()
        {
            try
            {
                await _viewModel.ConnectToServerAsync(_urlInputField.text, this.gameObject.GetCancellationTokenOnDestroy());
                _connectionStatusText.text = "Connect";
            }
            catch (Exception e)
            {
                Debug.Log("Catch Exception!!!!!!");
                Debug.LogError(e.Message);
                _connectionStatusText.text = "Exception occur!";
            }
        }

        private async UniTask Close()
        {
            try
            {
                _cancellationTokenSource.CancelAfter(TimeSpan.FromSeconds(20));
                await _viewModel.CloseAsync(_cancellationTokenSource.Token);
            }
            finally
            {
#if UNITY_EDITOR
                EditorApplication.isPlaying = false;
#else
                Application.Quit();
#endif
            }
        }

        private void OnFontSizeChange(float fontSize)
        {
            _flowingTextSettings.FontSize = fontSize;
        }
    }
}