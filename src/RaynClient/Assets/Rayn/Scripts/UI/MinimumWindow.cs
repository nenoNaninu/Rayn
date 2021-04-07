using System;
using UniRx;
using UnityEngine;

public class MinimumWindow : MonoBehaviour
{
    private DoubleClickObservable _doubleClickObservable;

    // Start is called before the first frame update
    void Start()
    {
        _doubleClickObservable = this.GetComponentInChildren<DoubleClickObservable>();

        OnDoubleClick()
            .Subscribe(_ => Debug.Log("double click!"));
    }

    public IObservable<Unit> OnDoubleClick() => _doubleClickObservable.OnDoubleClick();

}
