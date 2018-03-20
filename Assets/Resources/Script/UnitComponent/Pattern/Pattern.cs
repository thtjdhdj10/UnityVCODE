using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Pattern : MonoBehaviour
{
    public bool patternActivating = false;

    public void PatternActivate()
    {
        StartCoroutine(OnStartPattern());
    }

    protected abstract IEnumerator OnStartPattern();
    //{
    //    patternActivating = true;

    //    yield return new WaitForSeconds(3f);

    //    patternActivating = false;
    //}


}