using System.Collections;
using System.Collections.Generic;
using Kirurobo;
using UnityEngine;

namespace Rayn.Sandbox
{
    public class InputForTest : MonoBehaviour
    {
        [SerializeField] private UniWindowController _uniWindowController;

        // Start is called before the first frame update
        void Start()
        {
        }

        // Update is called once per frame
        void Update()
        {
#if !UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _uniWindowController.isTransparent = !_uniWindowController.isTransparent;
            _uniWindowController.isTopmost = true;
            _uniWindowController.isClickThrough = true;
        }
#endif
        }
    }
}
