using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(TurretBasePos))]
public class TurretBasePosEditor : Editor
{
    private TurretBasePos turretBase;
    public override void OnInspectorGUI()
    {
        if (DrawDefaultInspector())
        {
            Rebuild();
        }

        if (GUILayout.Button("Rebuild"))
        {
            Rebuild();
        }
    }

    private void Rebuild()
    {
        turretBase = target as TurretBasePos;
        if (turretBase.gameObject.activeInHierarchy)
        {
            turretBase.DebugCreateNewTurret();
        }
    }
}
