using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TurretBasePos))]
public class TurretBasePosEditor : Editor
{
    private TurretBasePos turretBase;
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        turretBase = target as TurretBasePos;
        if (turretBase.gameObject.activeInHierarchy)
        {
            turretBase.DebugCreateNewTurret();
        }
    }
}
