using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pattern_360Fire : Pattern
{
    public Pattern_360Fire(float _direction, float _deltaAngle, int _count, float _delay)
    {
        direction = _direction;
        deltaAngle = _deltaAngle;
        count = _count;
        delay = _delay;
    }

    public float direction;
    public float deltaAngle;
    public int count = 36;
    public float delay = 0.1f;

    private void Awake()
    {
        asdf();
        asdf(qwe: 4);

        
    }

    public void asdf(int qwe = 1, int zxc = 2)
    {

    }

    protected override IEnumerator OnStartPattern()
    {
        patternActivating = true;

//        for(int i = 0; i < )

//        yield return new WaitForSeconds

        patternActivating = false;
    }

}