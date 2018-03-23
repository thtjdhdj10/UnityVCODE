using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternMultiple : Pattern
{
    [Serializable]
    public struct PatternTimeStamp
    {
        public float waitForActivate;
        public Pattern pattern;
    }

    public List<PatternTimeStamp> patternTimeStamps = new List<PatternTimeStamp>();

    protected override IEnumerator PatternFramework()
    {
        for(int i = 0; i < patternTimeStamps.Count;++i)
        {
            yield return new WaitForSeconds(patternTimeStamps[i].waitForActivate);

            if (i < patternTimeStamps.Count)
            {
                if (patternTimeStamps[i].pattern != null)
                {
                    if (i > 0)
                        patternTimeStamps[i].pattern.PatternActivate(patternTimeStamps[i - 1].pattern);
                    else
                        patternTimeStamps[i].pattern.PatternActivate();
                }
            }
        }
    }
}
