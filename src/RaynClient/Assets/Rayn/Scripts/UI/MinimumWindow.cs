using System;
using UniRx;
using UnityEngine;

public class MinimumWindow : MonoBehaviour
{
    private DoubleClickObservable _doubleClickObservable;

    // Start is called before the first frame update
    void Awake()
    {
        _doubleClickObservable = this.GetComponentInChildren<DoubleClickObservable>();
    }

    public IObservable<Unit> OnDoubleClick() => _doubleClickObservable.OnDoubleClick();

}
