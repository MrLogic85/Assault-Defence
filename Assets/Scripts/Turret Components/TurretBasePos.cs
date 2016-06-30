using System;
using System.Collections.Generic;
using UnityEngine;

public class TurretBasePos : MonoBehaviour {
    public int generateSeed;
    public int[] generateIds;

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
    void Update()
    {
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

    private Base InstantiateBase(Base baseComponent)
    {
        turretBase = Instantiate(baseComponent, transform.position, transform.rotation) as Base;
        turretBase.transform.parent = transform;
        return turretBase;
    }

    // ==== DEBUG ====

    public void DebugCreateNewTurret()
    {
        if (generateSeed == 0)
        {
            generateSeed = new System.Random().Next();
        }
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

        TurretComponentLibrary library = FindObjectOfType<TurretComponentLibrary>();
        if (library != null)
        {
            if (generateIds.Length > 0)
            {
                UnityEngine.Random.seed = generateSeed;
                Base[] bases = library.GetBases();
                if (bases.Length > 0 && generateIds[0] < bases.Length && generateIds[0] >= 0)
                {
                    Base baseComponent = bases[generateIds[0]];
                    Base instance = InstantiateBase(baseComponent);
                    BuildRandom(Util.Subset(generateIds, 1), new TurretComponent[] { instance });
                }
            }
            else
            {
                UnityEngine.Random.seed = generateSeed;
                Base[] bases = library.GetBases();
                if (bases.Length > 0)
                {
                    Base baseComponent = bases[UnityEngine.Random.Range(0, bases.Length)];
                    InstantiateBase(baseComponent);
                    turretBase.BuildRandom();
                }
            }
        }
    }

    private void BuildRandom(int[] ids, TurretComponent[] components)
    {
        TurretComponentLibrary library = FindObjectOfType<TurretComponentLibrary>();
        int count = 0;
        List<TurretComponent> newComponents = new List<TurretComponent>();
        for (int i = 0; i < components.Length; i++)
        {
            for (int j = 0; j < components[i].slots.Length && j < ids.Length; j++)
            {
                TurretComponent[] prefabs = library.GetComponentsMatching(components[i].slots[j]);
                if (prefabs.Length > 0 && ids[count] < prefabs.Length && ids[count] >= 0)
                {
                    TurretComponent turretComponent = prefabs[ids[count]];
                    TurretComponent newComponent = components[i].InstantiateComponentAt(turretComponent, components[i].slots[j]);
                    newComponents.Add(newComponent);
                }
                count++;
                if (count >= generateIds.Length)
                {
                    return;
                }
            }
        }
        if (newComponents.Count > 0)
        {
            BuildRandom(Util.Subset(ids, count), newComponents.ToArray());
        }
    }
}
