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
        if (component.GetTurret() != null)
        {
            component.GetTurret().ProducePower(production * Time.deltaTime);
        }
	}
}
