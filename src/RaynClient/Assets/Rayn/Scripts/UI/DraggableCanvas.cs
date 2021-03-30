using Cysharp.Threading.Tasks;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Rayn
{
    public class DraggableCanvas : MonoBehaviour, IDragHandler
    {
        private RectTransform _rectTransform;

        // Start is called before the first frame update
        void Start()
        {
            _rectTransform = this.GetComponent<RectTransform>();
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rectTransform.position += new Vector3(eventData.delta.x, eventData.delta.y, 0);
        }


    }
}

