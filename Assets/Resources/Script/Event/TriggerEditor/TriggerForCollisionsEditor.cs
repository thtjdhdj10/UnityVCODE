﻿using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(TriggerForCollisions))]
[CanEditMultipleObjects]
public class TriggerForCollisionsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        serializedObject.Update();

        // target 은 값을 가져오는 용도로만 사용할 것
        TriggerForCollisions trigger = target as TriggerForCollisions;

        trigger.RefreshTriggerAttribute();
    }

}
