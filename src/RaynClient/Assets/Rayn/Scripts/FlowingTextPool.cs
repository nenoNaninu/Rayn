using UniRx.Toolkit;
using UnityEngine;

namespace Rayn
{
    public class FlowingTextPool : ObjectPool<FlowingText>
    {
        private readonly FlowingText _flowingTextPrefab;
        private readonly Transform _parent;

        public FlowingTextPool(FlowingText prefab, Transform parent)
        {
            _flowingTextPrefab = prefab;
            _parent = parent;
        }

        protected override FlowingText CreateInstance()
        {
            var flowingText = Object.Instantiate(_flowingTextPrefab);

            flowingText.transform.SetParent(_parent);

            return flowingText;
        }
    }
}