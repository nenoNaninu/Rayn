using System.Threading;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Rayn
{
    // �A���J�[��canvas�̒��S�ɂ��邱�Ƒz�肵�ď����Ă�B
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

        public void Init(Canvas canvas, string text, float flowingTimeSpan = 5f, float fontSize = 50f)
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
            _textMesh.fontSize = fontSize;
            _textSize = new Vector2(_textMesh.preferredWidth, _textMesh.preferredHeight) * 1.1f;

            _rectTransform.anchoredPosition = new Vector3(_canvasResolution.x / 2 + _textSize.x / 2, Random.Range(-_canvasResolution.y / 2f, _canvasResolution.y / 2f) * 0.8f, 0);
        }

        public async UniTask PlayAnimation(CancellationToken cancellationToken)
        {
            var bound = (-_canvasResolution.x / 2 - _textSize.x / 2) * 1.1;
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_rectTransform.anchoredPosition.x < bound)
                {
                    return;
                }

                _rectTransform.anchoredPosition += new Vector2(-_flowingSpeedPerSec * Time.deltaTime, 0);
                await UniTask.NextFrame(PlayerLoopTiming.Update, cancellationToken);
            }
        }
    }
}