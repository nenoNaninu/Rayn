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
            this.Register();
        }

        private void Register()
        {
            //ServiceLocator.Register<IServer<string>, DummyServer>(new DummyServer());
            ServiceLocator.Register<IServer<string>, Server>(new Server());
            ServiceLocator.Register<IFlowingTextSettings, FlowingTextSettings>(new FlowingTextSettings());
        }
    }
}