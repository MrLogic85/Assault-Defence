using UnityEngine;
using System;

public class Motor : TurretComponent
{
    public Joint[] engineJoints;
    [Header("Motor")]
    [Tooltip ("Degrees per second")]
    public float turnSpeed = 20;
    [Tooltip("Max weight in kilos before the motor stops")]
    public float maxWeight = 90;
    
    private float lastDierction = 1;

    // Update is called once per frame
    void Update () {
        RotateTowardsTarget();
    }

    private void RotateTowardsTarget()
    {
        for (int i = 0; i < engineJoints.Length; i++)
        {
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
            if (!base.GetAimOffsetWeight(out offset))
            {
                // Reset rotation
                if (Math.Abs(currentRotation) > step)
                {
                    engineJoint.joint.Rotate(0, -Math.Sign(currentRotation) * step, 0);
                }
                continue;
            }
            
            // Try to rotate in lastDirection
            if (engineJoint.maxAngle >= 360 || Math.Abs(currentRotation + lastDierction * step) <= engineJoint.maxAngle)
            {
                engineJoint.joint.Rotate(0, lastDierction * step, 0);
                float newOffset;
                base.GetAimOffsetWeight(out newOffset);
                if (newOffset < offset)
                {
                    continue;
                }
                else
                {
                    engineJoint.joint.Rotate(0, -lastDierction * step, 0);
                }
            }

            // Try the other direction
            if (engineJoint.maxAngle >=360 || Math.Abs(currentRotation - lastDierction * step) <= engineJoint.maxAngle)
            {
                engineJoint.joint.Rotate(0, -lastDierction * step, 0);
                float newOffset;
                base.GetAimOffsetWeight(out newOffset);
                if (newOffset < offset)
                {
                    lastDierction *= -1;
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
    internal override void FittedOn(Armament armament)
    {
        slots[0].armament = armament;
    }

    internal override bool GetAimOffsetWeight(out float offset)
    {
        // Return a smaller value for motors to decrease the weight of weapons which has their own motors
        bool hasTarget = base.GetAimOffsetWeight(out offset);
        offset /= 2f;
        return hasTarget;
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
