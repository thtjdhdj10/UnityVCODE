using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternComponent : UnitComponent
{
    public Pattern[] pattern;

    public bool patternWork;

    public enum PatternSelectType
    {
        RANDOMLY,
        SEQUENCY,

    }

    private void FixedUpdate()
    {
        if(patternWork == false)
        {
            SelectNextPattern();
        }
    }

    public Pattern SelectNextPattern()
    {

    }


}
