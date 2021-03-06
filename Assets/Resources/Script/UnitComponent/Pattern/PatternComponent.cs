﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PatternComponent : UnitComponent
{
    public List<Pattern> patterns = new List<Pattern>();
    public Queue<Pattern> patternQueue = new Queue<Pattern>();

    private Pattern lastActivatedPattern;

    private void FixedUpdate()
    {
        PatternProcess();
    }

    private void PatternProcess()
    {
        if (patternQueue.Count == 0) // 패턴을 다 실행했으면 새로 채워넣음
        {
            Pattern selectPattern = patterns[Random.Range(0, patterns.Count - 1)];

            while (selectPattern != null &&
                selectPattern == lastActivatedPattern) // 마지막에 발동한 패턴이 연속으로 발동되지 않도록 처리
            {
                selectPattern = patterns[Random.Range(0, patterns.Count - 1)]; // select random pattern
            }

            patternQueue.Enqueue(selectPattern);
            patterns.Remove(selectPattern);

            while (patterns.Count > 0)
            {
                selectPattern = patterns[Random.Range(0, patterns.Count - 1)]; // select random pattern

                patternQueue.Enqueue(selectPattern);
                patterns.Remove(selectPattern);
            }
        }
        else
        {
            if (patternQueue.Peek().isPatternRunning == false)
            {
                if(patternQueue.Peek() != lastActivatedPattern)
                {
                    lastActivatedPattern = patternQueue.Peek();
                    lastActivatedPattern.PatternActivate();
                }
                else
                {
                    patterns.Add(lastActivatedPattern);
                    patternQueue.Dequeue();
                }
            }
        }
    }
}
