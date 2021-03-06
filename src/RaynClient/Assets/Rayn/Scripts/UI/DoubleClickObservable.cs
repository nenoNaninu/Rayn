using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoubleClickObservable : MonoBehaviour, IPointerClickHandler
{
    private readonly Subject<Unit> _clickSubject = new Subject<Unit>();

    public IObservable<Unit> OnDoubleClick()
    {
        return _clickSubject.Buffer(_clickSubject.Throttle(TimeSpan.FromMilliseconds(250)))
            .Where(xs => xs.Count >= 2)
            .Select(_ => Unit.Default);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        _clickSubject.OnNext(Unit.Default);
    }
}
