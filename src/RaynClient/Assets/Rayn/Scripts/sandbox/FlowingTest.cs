//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Cysharp.Threading.Tasks;
//using ScreenOverwriter;
//using UnityEngine;

//public class FlowingTest : MonoBehaviour
//{
//    [SerializeField]
//    private FlowingTextController _flowingTextController;

//    private readonly string[] _sampleTextArray = new string[]
//    {
//        "あ", "a", "メロンパン", "エヴァンゲリオン", "みんなシェルノサージュやろう!!!!!!😎", "😂"
//    };

//    // Start is called before the first frame update
//    async void  Start()
//    {
//        await UniTask.Delay(TimeSpan.FromSeconds(1));

//        foreach (string message in _sampleTextArray)
//        {
//            _flowingTextController.FlowMessage(message).Forget();

//            await UniTask.Delay(TimeSpan.FromSeconds(1));
//        }

//        await UniTask.Delay(TimeSpan.FromSeconds(20));

//        foreach (string message in _sampleTextArray)
//        {
//            _flowingTextController.FlowMessage(message).Forget();

//            await UniTask.Delay(TimeSpan.FromSeconds(1));
//        }
//    }
//}
