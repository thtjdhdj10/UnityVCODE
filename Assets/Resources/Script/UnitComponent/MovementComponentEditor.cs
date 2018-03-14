using UnityEngine;
using UnityEditor;
using System.Collections.Generic;

[CustomEditor(typeof(MovementComponent))]
[CanEditMultipleObjects]
public class MovementComponentEditor : Editor
{
    public bool advancedFold = false;

    SerializedProperty movementTypeProp;
    SerializedProperty speedProp;

    SerializedProperty turnFactorProp;

    SerializedProperty disFactorProp;
    SerializedProperty maxTurnByDisProp;
    SerializedProperty minTurnByDisProp;

    // Advanced

    SerializedProperty minTurnFactorProp;
    SerializedProperty initRotateToTargetProp;
    SerializedProperty sprAngleToDirProp;



    string movementTypeName = "movementType";
    string speedName = "speed";

    string turnFactorName = "turnFactor";

    string disFactorName = "disFactor";
    string maxTurnByDisName = "maxTurnByDis";
    string minTurnByDisName = "minTurnByDis";

    // Advanced

    string minTurnFactorName = "minTurnFactor";
    string initRotateToTargetName = "initRotateToTarget";
    string sprAngleToDirName = "sprAngleToDir";


    void OnEnable()
    {
        movementTypeProp = serializedObject.FindProperty(movementTypeName);
        speedProp = serializedObject.FindProperty(speedName);

        turnFactorProp = serializedObject.FindProperty(turnFactorName);

        disFactorProp = serializedObject.FindProperty(disFactorName);
        maxTurnByDisProp = serializedObject.FindProperty(maxTurnByDisName);
        minTurnByDisProp = serializedObject.FindProperty(minTurnByDisName);

        minTurnFactorProp = serializedObject.FindProperty(minTurnFactorName);
        initRotateToTargetProp = serializedObject.FindProperty(initRotateToTargetName);
        sprAngleToDirProp = serializedObject.FindProperty(sprAngleToDirName);

    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(movementTypeProp);
        
        //

        // target 은 값을 가져오는 용도로만 사용할 것
        MovementComponent movable = target as MovementComponent;

        EditorGUILayout.PropertyField(speedProp);

        EditorGUILayout.Space();

        switch (movable.movementType)
        {
            case MovementComponent.MovementType.STRAIGHT:
                {

                }
                break;
            case MovementComponent.MovementType.TURN_LERP:
                {
                    EditorGUILayout.PropertyField(turnFactorProp, new GUIContent("Ratio by Rot"));

                }
                break;
            case MovementComponent.MovementType.TURN_LERP_BY_DISTANCE:
                {
                    EditorGUILayout.PropertyField(turnFactorProp, new GUIContent("Ratio by Rot"));

                    EditorGUILayout.PropertyField(disFactorProp, new GUIContent("Ratio by Dis"));
                    EditorGUILayout.PropertyField(maxTurnByDisProp);
                    EditorGUILayout.PropertyField(minTurnByDisProp);

                }
                break;
            case MovementComponent.MovementType.TURN_REGULAR:
                {
                    EditorGUILayout.PropertyField(turnFactorProp, new GUIContent("Rotation per Sec"));

                }
                break;
            case MovementComponent.MovementType.TURN_REGULAR_DISTANCE:
                {
                    EditorGUILayout.PropertyField(turnFactorProp, new GUIContent("Rotation per Sec"));

                    EditorGUILayout.PropertyField(disFactorProp, new GUIContent("Ratio by Dis"));
                    EditorGUILayout.PropertyField(maxTurnByDisProp);
                    EditorGUILayout.PropertyField(minTurnByDisProp);

                }
                break;
            case MovementComponent.MovementType.VECTOR:
                {

                }
                break;
        }


        if (advancedFold = EditorGUILayout.Foldout(advancedFold, new GUIContent("Advanced")))
        {
            EditorGUILayout.PropertyField(initRotateToTargetProp);
            EditorGUILayout.PropertyField(sprAngleToDirProp);

            switch (movable.movementType)
            {
                case MovementComponent.MovementType.STRAIGHT:
                    {

                    }
                    break;
                case MovementComponent.MovementType.TURN_LERP:
                    {
                        EditorGUILayout.PropertyField(minTurnFactorProp, new GUIContent("Min Rot"));

                    }
                    break;
                case MovementComponent.MovementType.TURN_LERP_BY_DISTANCE:
                    {
                        EditorGUILayout.PropertyField(minTurnFactorProp, new GUIContent("Min Rot"));

                    }
                    break;
                case MovementComponent.MovementType.TURN_REGULAR:
                    {

                    }
                    break;
                case MovementComponent.MovementType.TURN_REGULAR_DISTANCE:
                    {

                    }
                    break;
                case MovementComponent.MovementType.VECTOR:
                    {

                    }
                    break;
            }
        }

        serializedObject.ApplyModifiedProperties();

        //Rect r = GUILayoutUtility.GetRect(0f, 16f);
        //bool showNext = EditorGUI.PropertyField(r, layerProp, true);
        //bool hasName = layerProp.NextVisible(showNext);

        //        if (layerListFold = EditorGUILayout.Foldout(layerListFold, "Layers"))
    }
}
