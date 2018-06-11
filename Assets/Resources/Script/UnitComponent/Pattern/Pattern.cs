using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Pattern
{
    public Unit owner;

    public bool isPatternRunning = false;

    public bool blockBulletFire = false;

    protected Pattern prevPattern;

    private Pattern()
    {

    }

    public Pattern(Unit _owner)
    {
        owner = _owner;
    }

    public void PatternActivate(Pattern _prevPattern)
    {
        prevPattern = _prevPattern;
    }

    protected abstract IEnumerator PatternFramework();

    //public void ForceStopPattern()
    //{
    //    isPatternRunning = false;
    //    blockBulletFire = false;
    //    StopCoroutine(PatternFramework());
    //}

    protected virtual void OnPatternStop()
    {
        // 총알 없애거나 뭐 그런거
    }
}