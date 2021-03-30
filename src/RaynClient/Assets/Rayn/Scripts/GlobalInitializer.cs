using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace ScreenOverwriter
{
    public class GlobalInitializer : MonoBehaviour
    {
        // Start is called before the first frame update
        void Awake()
        {
            this.RegisterForDebug();
        }

        private void RegisterForDebug()
        {
            //ServiceLocator.Register<IServer<string>, DummyServer>(new DummyServer());
            ServiceLocator.Register<IServer<string>, Server>(new Server());

        }

        // Update is called once per frame
        void Update()
        {

        }
    }

}
