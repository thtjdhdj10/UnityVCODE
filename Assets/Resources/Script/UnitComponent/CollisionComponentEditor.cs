using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Reflection;

[CustomEditor(typeof(CollisionComponent))]
//[CustomPropertyDrawer(typeof(CollisionComponent.Status))]
[CanEditMultipleObjects]
public class CollisionComponentEditor : Editor
{
    SerializedProperty originStatusProp;
    
    SerializedProperty isHittableProp;
    
    SerializedProperty targetTypeProp;
    SerializedProperty damageEffectTypeProp;
    SerializedProperty damageProp;

    SerializedProperty isBeHittableProp;

    SerializedProperty enableStaticDamageProp;

    SerializedProperty staticDamageProp;

    //    SerializedProperty isEnableShieldProp;

    SerializedProperty maxHPProp;
//    SerializedProperty timeToShieldProp;
//    SerializedProperty shieldCountProp;

    void OnEnable()
    {
        //CollisionComponent c = target as CollisionComponent;
        //if (c.originStatus == null)
        //    c.originStatus = CreateInstance<CollisionComponent.Status>();
        
        isHittableProp = serializedObject.FindProperty("isHittable");

        isBeHittableProp = serializedObject.FindProperty("isBeHittable");

        targetTypeProp = serializedObject.FindProperty("targetType");
        damageEffectTypeProp = serializedObject.FindProperty("damageEffectType");

        damageProp = serializedObject.FindProperty("damage");
        maxHPProp = serializedObject.FindProperty("maxHP");

        enableStaticDamageProp = serializedObject.FindProperty("enableStaticDamage");
        staticDamageProp = serializedObject.FindProperty("staticDamage");

        //        originStatusProp = serializedObject.FindProperty("originStatus");

        //        damageProp = originStatusProp.FindPropertyRelative("damage");
        //        damageProp = serializedObject.FindProperty("originStatus.damage");

        //        SerializedObject originStatusPropObj = new SerializedObject(originStatusProp.objectReferenceValue);

        //timeToShieldProp = originStatusPropObj.FindProperty("timeToShield");
        //shieldCountProp = originStatusPropObj.FindProperty("shieldCount");


    }

    public override void OnInspectorGUI()
    {
        serializedObject.Update();

        EditorGUILayout.PropertyField(isHittableProp);

        EditorGUILayout.Space();

        if (isHittableProp.boolValue == true)
        {
            EditorGUILayout.PropertyField(targetTypeProp);
            EditorGUILayout.PropertyField(damageEffectTypeProp);
            EditorGUILayout.PropertyField(damageProp);
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(isBeHittableProp);

        EditorGUILayout.Space();

        if (isBeHittableProp.boolValue == true)
        {
            EditorGUILayout.PropertyField(maxHPProp);
            //EditorGUILayout.PropertyField(isEnableShieldProp);
            //if(isEnableShieldProp.boolValue == true)
            //{
            //    EditorGUILayout.Space();
            //    EditorGUILayout.PropertyField(timeToShieldProp);
            //    EditorGUILayout.PropertyField(shieldCountProp);
            //}
        }

        EditorGUILayout.PropertyField(enableStaticDamageProp);

        EditorGUILayout.Space();

        if (enableStaticDamageProp.boolValue == true)
        {
            EditorGUILayout.PropertyField(staticDamageProp);
        }

        serializedObject.ApplyModifiedProperties();
    }

}
