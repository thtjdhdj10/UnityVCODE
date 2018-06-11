using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternBoss1_1 : PatternMultiple {





    private void Awake()
    {
        PatternCircleFire circle_1 = new PatternCircleFire();
//        circle_1.Init()

        PatternTimeStamp patternSchedule_1 = new PatternTimeStamp();

        patternTimeStamps.Add(patternSchedule_1);


    }

    protected override IEnumerator PatternFramework()
    {




        yield break;
    }
}
