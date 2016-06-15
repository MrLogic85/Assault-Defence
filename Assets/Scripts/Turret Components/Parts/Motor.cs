using UnityEngine;
using System;
using System.Collections.Generic;

[RequireComponent(typeof(TurretComponent))]
[ExecuteInEditMode]
public class Motor : MonoBehaviour
{
    public Joint[] engineJoints;
    [Header("Motor")]
    [Tooltip("Degrees per second")]
    public float turnSpeed = 20;
    [Tooltip("Max weight in kilos before the motor stops")]
    public float maxWeight = 90;
    [Tooltip("The power needed per second to turn the motor")]
    public float powerConsumption;

    private TurretComponent component;
    private float lastDierction = 1;

    void Awake()
    {
        component = GetComponent<TurretComponent>();
        component.onComponenGetAimWeightEvent.Add(GetAimOffsetWeight);

        if (component.parentComponent != null)
        {
            Fitted(component.parentComponent, component.fittedOnArmamanet);
        }
        else
        {
            component.onFittedEvent += Fitted;
        }
    }

    // Update is called once per frame
    void Update()
    {
        RotateTowardsTarget();
    }

    private void ConsumePower()
    {
        Turret turret;
        if (component.GetTurret(out turret))
        {
            turret.ConsumePower(powerConsumption * Time.deltaTime);
        }
    }

    private void RotateTowardsTarget()
    {
        for (int i = 0; i < engineJoints.Length; i++)
        {
            Turret turret;
            if (component.GetTurret(out turret) && turret.GetStoredPower() < powerConsumption * Time.deltaTime)
            {
                break;
            }
            Joint engineJoint = engineJoints[i];
            float step = turnSpeed * Time.deltaTime;
            float currentRotation = engineJoint.joint.localRotation.eulerAngles.y;
            while (currentRotation > 180)
            {
                currentRotation -= 360;
            }
            while (currentRotation < -180)
            {
                currentRotation += 360;
            }

            float offset = 0;
            if (!component.GetAimOffsetWeight(out offset))
            {
                // Reset rotation to forward
                if (Math.Abs(currentRotation) > step)
                {
                    engineJoint.joint.Rotate(0, -Math.Sign(currentRotation) * step, 0);
                    ConsumePower();
                }
                continue;
            }

            // Try to rotate in lastDirection
            if (engineJoint.maxAngle >= 360 || Math.Abs(currentRotation + lastDierction * step) <= engineJoint.maxAngle)
            {
                engineJoint.joint.Rotate(0, lastDierction * step, 0);
                float newOffset;
                component.GetAimOffsetWeight(out newOffset);
                if (newOffset < offset)
                {
                    ConsumePower();
                    continue;
                }
                else
                {
                    engineJoint.joint.Rotate(0, -lastDierction * step, 0);
                }
            }

            // Try the other direction
            if (engineJoint.maxAngle >= 360 || Math.Abs(currentRotation - lastDierction * step) <= engineJoint.maxAngle)
            {
                engineJoint.joint.Rotate(0, -lastDierction * step, 0);
                float newOffset;
                component.GetAimOffsetWeight(out newOffset);
                if (newOffset < offset)
                {
                    lastDierction *= -1;
                    ConsumePower();
                    continue;
                }
                else
                {
                    engineJoint.joint.Rotate(0, lastDierction * step, 0);
                }
            }
        }
    }

    // A motor has the same armament as the slot it was fitted on.
    private void Fitted(TurretComponent parentComponent, Armament armament)
    {
        component.slots[0].armament = armament;
    }

    public KeyValuePair<float, bool> GetAimOffsetWeight(float childrenOffset, bool childrenIsAiming)
    {
        // Return a smaller value for motors to decrease the weight of weapons which has their own motors
        if (childrenIsAiming)
        {
            return new KeyValuePair<float, bool>(childrenOffset / 2f, true);
        }
        return new KeyValuePair<float, bool>(childrenOffset, false);
    }

    private Vector3 ProjectPointOnPlane(Vector3 planeNormal, Vector3 planePoint, Vector3 point)
    {
        var dist = Vector3.Dot(planeNormal, (point - planePoint));
        return point + planeNormal * -dist;
    }
}

[System.Serializable]
public class Joint
{
    public Transform joint;
    [Range(0, 360)]
    public int maxAngle = 360;
}
