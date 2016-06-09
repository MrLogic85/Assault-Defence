using UnityEngine;

public class TurretBasePos : MonoBehaviour {
    private Base turretBase;

    // Use this for initialization
    void Start () {
	}

    // Update is called once per frame
    void Update () {

    }

    private void DestroyTurret()
    {
        if (turretBase != null)
        {
            Destroy(turretBase.gameObject);
        }
    }

    private void InstantiateBase(Base baseComponent)
    {
        turretBase = Instantiate(baseComponent, transform.position, transform.rotation) as Base;
        turretBase.transform.parent = transform;
    }

    // ==== DEBUG ====

    internal void DebugCreateNewTurret()
    {
        DestroyTurret();
        BuildRandom();
    }

    private void BuildRandom()
    {
        if (turretBase != null)
        {
            DestroyTurret();
        }

        Base[] bases = TurretComponentLibrary.instance.GetBases();
        if (bases.Length > 0)
        {
            Base baseComponent = bases[Random.Range(0, bases.Length)];
            InstantiateBase(baseComponent);
            turretBase.BuildRandom();
        }
    }
}
