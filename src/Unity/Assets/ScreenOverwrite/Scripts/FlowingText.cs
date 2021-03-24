using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

namespace ScreenOverwriter
{
    public class FlowingText : MonoBehaviour
    {
        private string _text;
        private TextMeshProUGUI _textMesh;
        private Canvas _canvas;
        private RectTransform _rectTransform;
        private Vector2 _textSize;
        private float _flowingTimeSpan = 0.5f;
        private float _flowingSpeedPerSec;

        public void Init(Canvas canvas, string text, float flowingTimeSpan = 5f)
        {
            _textMesh ??= this.GetComponent<TextMeshProUGUI>();
            _rectTransform ??= this.GetComponent<RectTransform>();

            var displaySize = canvas.renderingDisplaySize;

            _canvas = canvas;
            _flowingTimeSpan = flowingTimeSpan;

            _flowingSpeedPerSec = displaySize.x / _flowingTimeSpan;            

            _text = text;
            _textMesh.text = text;
            _textSize = new Vector2(_textMesh.preferredWidth, _textMesh.preferredHeight);

            _rectTransform.localPosition = new Vector3(displaySize.x / 2 + _textSize.x / 2, Random.Range(-displaySize.y / 2f, displaySize.y / 2f), 0);
        }

        public async UniTask PlayAnimation()
        {
            while (true)
            {
                if (_rectTransform.localPosition.x < -_canvas.renderingDisplaySize.x / 2 - _textSize.x / 2)
                {
                    return;
                }

                _rectTransform.localPosition += new Vector3(-_flowingSpeedPerSec * Time.deltaTime, 0, 0);
                await UniTask.NextFrame(PlayerLoopTiming.Update);
            }
        }
    }
}

