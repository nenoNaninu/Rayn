using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ScreenOverwriter.Sandbox
{
    public class Rotater : MonoBehaviour
    {
        private Quaternion _quaternion;
        // Start is called before the first frame update
        void Start()
        {
            var rotate = this.gameObject.transform.rotation;
            _quaternion = Quaternion.AngleAxis(0.2f, new Vector3(1, 1, 2));
        }

        // Update is called once per frame
        void Update()
        {
            this.gameObject.transform.rotation *= _quaternion;
        }
    }
}

