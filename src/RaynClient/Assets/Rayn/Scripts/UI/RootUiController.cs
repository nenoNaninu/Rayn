using System;
using System.Threading;
using Cysharp.Threading.Tasks;
using Kirurobo;
using TMPro;
using UniRx;
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
        [SerializeField] private Button _minimizeButton;

        [SerializeField] private Slider _fontSizeSlider;
        [SerializeField] private TMP_InputField _urlInputField;
        [SerializeField] private TMP_InputField _proxyInputField;

        [SerializeField] private TextMeshProUGUI _connectionStatusText;

        [SerializeField] private MainWindow _mainWindow;
        [SerializeField] private MinimumWindow _minimumWindow;
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

            _minimizeButton.onClick
                .AsObservable()
                .Subscribe(_ => this.SwitchWindow().Forget());

            _minimumWindow.OnDoubleClick()
                .Subscribe(_ => this.SwitchWindow().Forget());

            _flowingTextSettings = await ServiceLocator.GetServiceAsync<IFlowingTextSettings>();

            _fontSizeSlider.onValueChanged
                .AsObservable()
                .Subscribe(this.OnFontSizeChange)
                .AddTo(this);

            await UniTask.NextFrame();

            _flowingTextSettings.FontSize = _fontSizeSlider.value;

            _minimumWindow.gameObject.SetActive(false);
        }

        private async UniTask SwitchWindow()
        {
            var (currentWindow, nextWindow) = _minimumWindow.gameObject.activeSelf
                ? (_minimumWindow.gameObject, _mainWindow.gameObject)
                : (_mainWindow.gameObject, _minimumWindow.gameObject);

            // 座標合わせて
            var nextRectTransform = nextWindow.GetComponent<RectTransform>();
            var currentRectTransform = currentWindow.GetComponent<RectTransform>();

            nextRectTransform.anchoredPosition = currentRectTransform.anchoredPosition;

            // アニメーション仕込んで切り替える
            await this.SwitchWindowWithAnimationAsync(currentRectTransform, nextRectTransform);
        }

        private async UniTask SwitchWindowWithAnimationAsync(RectTransform current, RectTransform next)
        {
            // 0.5秒で切り替えるので、それぞれ0.25秒で1->0, 0->1のスケール変換をする。

            var startTime = Time.time; // 秒

            var min = new Vector3(1f, 0, 1);
            var normal = new Vector3(1f, 1f, 1);

            current.localScale = normal;
            next.localScale = min;

            float timeSpan = 0.25f;

            while (Time.time < startTime + timeSpan)
            {
                var delta = Time.time - startTime;

                var scale = Vector3.Lerp(normal, min, delta / timeSpan);

                current.localScale = scale;

                await UniTask.NextFrame(PlayerLoopTiming.Update);
            }

            current.localScale = min;

            current.gameObject.SetActive(false);
            next.gameObject.SetActive(true);

            var startTime2 = Time.time; // 秒

            while (Time.time < startTime2 + timeSpan)
            {
                var delta = Time.time - startTime2;

                var scale = Vector3.Lerp(min, normal, delta / timeSpan);

                next.localScale = scale;

                await UniTask.NextFrame(PlayerLoopTiming.Update);
            }

            next.localScale = normal;
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
                //await _viewModel.ConnectToServerAsync(_urlInputField.text, "http://proxy.uec.ac.jp:8080", this.gameObject.GetCancellationTokenOnDestroy());
                await _viewModel.ConnectToServerAsync(_urlInputField.text, _proxyInputField.text, this.gameObject.GetCancellationTokenOnDestroy());
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