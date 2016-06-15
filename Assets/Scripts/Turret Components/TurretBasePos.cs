using UnityEngine;

public class TurretBasePos : MonoBehaviour {
    public int generateSeed;

    private Base turretBase;

    // Use this for initialization
    void Start () {
        if (transform.childCount > 0)
        {
            // We are already built, rebuild a new turret
            DebugCreateNewTurret();
        }
        //FindBase();
	}

    // Update is called once per frame
    void Update () {

    }

    private void DestroyTurret()
    {
        if (turretBase != null)
        {
            if (Application.isEditor)
            {
                DestroyImmediate(turretBase.gameObject);
            }
            else
            {
                Destroy(turretBase.gameObject);
            }
        }
    }

    private void InstantiateBase(Base baseComponent)
    {
        turretBase = Instantiate(baseComponent, transform.position, transform.rotation) as Base;
        turretBase.transform.parent = transform;
    }

    // ==== DEBUG ====

    public void DebugCreateNewTurret()
    {
        FindBase();
        DestroyTurret();
        BuildRandom();
    }

    private void FindBase()
    {
        foreach (Transform child in transform)
        {
            turretBase = child.gameObject.GetComponent<Base>();
            if (turretBase != null)
            {
                break;
            }
        }
    }

    public void BuildRandom()
    {
        if (turretBase != null)
        {
            DestroyTurret();
        }
        
        Random.seed = generateSeed;

        TurretComponentLibrary library = FindObjectOfType<TurretComponentLibrary>();
        if (library != null)
        {
            Base[] bases = library.GetBases();
            if (bases.Length > 0)
            {
                Base baseComponent = bases[Random.Range(0, bases.Length)];
                InstantiateBase(baseComponent);
                turretBase.BuildRandom();
            }
        }
    }
}
