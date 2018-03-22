using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Pattern : MonoBehaviour
{
    public Unit owner;

    public bool isPatternRunning = false;

    public bool blockBulletFire = false;

    protected Pattern prevPattern;

    private void Awake()
    {
        if (owner == null)
            owner = GetComponent<Unit>();
    }

    public void PatternActivate()
    {
        StartCoroutine(OnStartPattern());
    }

    public void PatternActivate(Pattern _prevPattern)
    {
        prevPattern = _prevPattern;
        StartCoroutine(OnStartPattern());
    }

    protected abstract IEnumerator OnStartPattern();


}