using UnityEngine;
using System.Collections;
using System;

public class TurretController : MonoBehaviour {
	
	// Update is called once per frame
	void Update ()
    {
        TurretBasePos[] turrets = FindObjectsOfType<TurretBasePos>();
        for (int i = 0; i < turrets.Length; i++)
        {
            TurretBasePos turret = turrets[i];
            Update(turret);
        }
	}

    private void Update(TurretBasePos turret)
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            turret.DebugCreateNewTurret();
        }
    }
}
