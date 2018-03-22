using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PatternAttachable : Pattern
{
    public GameObject positionRoot;
    public GameObject directionRoot;

    public bool attachPositionToRoot;
    public bool attachDirectionToRoot;

    protected Vector2 position;
    protected float direction;

    void Awake()
    {
        if (positionRoot == null)
            positionRoot = gameObject;

        position = positionRoot.transform.position;

        if (directionRoot == null)
            directionRoot = gameObject;

        direction = directionRoot.transform.eulerAngles.z;
    }

    private void FixedUpdate()
    {
        if (attachPositionToRoot == true)
            position = positionRoot.transform.position;

        if (attachDirectionToRoot == true)
            direction = directionRoot.transform.eulerAngles.z;
    }
}
