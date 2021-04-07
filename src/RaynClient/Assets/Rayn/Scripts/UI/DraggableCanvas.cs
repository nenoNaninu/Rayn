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
            _rectTransform.anchoredPosition += new Vector2(eventData.position.x, eventData.position.y);
        }
    }
}

