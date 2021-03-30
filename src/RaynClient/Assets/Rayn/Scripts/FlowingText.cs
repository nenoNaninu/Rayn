using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace ScreenOverwriter
{
    // アンカーがcanvasの中心にあること想定して書いてる。
    public class FlowingText : MonoBehaviour
    {
        private string _text;
        private TextMeshProUGUI _textMesh;
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private CanvasScaler _canvasScaler;
        private Vector2 _canvasResolution;
        private Vector2 _textSize;
        private float _flowingTimeSpan = 0.5f;
        private float _flowingSpeedPerSec;

        public void Init(Canvas canvas, string text, float flowingTimeSpan = 5f)
        {
            _textMesh ??= this.GetComponent<TextMeshProUGUI>();
            _rectTransform ??= this.GetComponent<RectTransform>();
            _canvasScaler ??= canvas.GetComponent<CanvasScaler>();

            _canvasResolution = _canvasScaler.referenceResolution;

            _canvas = canvas;
            _flowingTimeSpan = flowingTimeSpan;

            _flowingSpeedPerSec = _canvasResolution.x / _flowingTimeSpan;            

            _text = text;
            _textMesh.text = text;
            _textSize = new Vector2(_textMesh.preferredWidth, _textMesh.preferredHeight);

            _rectTransform.anchoredPosition = new Vector3(_canvasResolution.x / 2 + _textSize.x / 2, Random.Range(-_canvasResolution.y / 2f, _canvasResolution.y / 2f), 0);
        }

        public async UniTask PlayAnimation()
        {
            while (true)
            {
                if (_rectTransform.position.x < -_canvasResolution.x / 2 - _textSize.x / 2)
                {
                    return;
                }

                _rectTransform.anchoredPosition += new Vector2(-_flowingSpeedPerSec * Time.deltaTime, 0);
                await UniTask.NextFrame(PlayerLoopTiming.Update);
            }
        }
    }
}

