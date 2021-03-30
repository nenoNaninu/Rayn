using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace ScreenOverwriter
{
    public class DraggableCanvas : MonoBehaviour, IDragHandler
    {
        private RectTransform _rectTransform;

        // Start is called before the first frame update
        async void Start()
        {
            _rectTransform = this.GetComponent<RectTransform>();
            var cancellationToken = this.GetCancellationTokenOnDestroy();

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    await UniTask.DelayFrame(120, cancellationToken: cancellationToken);
                    Debug.Log($"Canvas pos : {_rectTransform.position}, Canvas local Pos : {_rectTransform.localPosition}");
                }
                catch
                {
                }

            }
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
        }


    }
}

