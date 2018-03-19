using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pattern : MonoBehaviour
{
    public bool patternActivating = false;

    public void PatternActivate()
    {
        StartCoroutine(StartPattern());
    }

    protected virtual IEnumerator StartPattern()
    {
        patternActivating = true;

        yield return new WaitForSeconds(3f);

        patternActivating = false;
    }


}