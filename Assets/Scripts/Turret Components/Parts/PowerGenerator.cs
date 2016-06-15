using UnityEngine;
using System.Collections;

[RequireComponent (typeof (TurretComponent))]
public class PowerGenerator : MonoBehaviour
{
    public float production = 10;

    private TurretComponent component;

    void Start()
    {
        component = GetComponent<TurretComponent>();
    }

    // Update is called once per frame
    void Update () {
        Turret turret;
        if (component.GetTurret(out turret))
        {
            turret.ProducePower(production * Time.deltaTime);
        }
	}
}
