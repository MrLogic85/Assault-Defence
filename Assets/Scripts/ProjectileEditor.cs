using UnityEngine;
using System.Collections;
using UnityEditor;
using System;

[CustomEditor(typeof(Weapon))]
public class WeaponScriptEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Weapon weapon = (Weapon)target;
        float speed = weapon.projectileSpeed;
        EditorGUILayout.FloatField("Approximal Damage", weapon.CalculateDamagePotential());
    }
}
