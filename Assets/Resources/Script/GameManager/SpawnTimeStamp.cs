using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnTimeStamp : MonoBehaviour
{
    public int currentPhase = 0;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        currentPhase = 0;
    }

    public IEnumerator SpawnTimeline()
    {
        // do phase 1

        yield return new WaitForSeconds(5f);

        while (CheckCondition(currentPhase))
        {
            yield return new WaitForFixedUpdate();
        }

        // do phase 2

        yield return new WaitForSeconds(5f);

        while (CheckCondition(currentPhase))
        {
            yield return new WaitForFixedUpdate();
        }


    }

    public bool CheckCondition(int phaseIndex)
    {
        // phase 에 따른 조건 처리

        return false;
    }

}
