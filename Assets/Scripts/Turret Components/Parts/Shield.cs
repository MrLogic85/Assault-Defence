using UnityEngine;

public class Shield : TurretComponent {

    private static float AimWeight = 0.01f;

    private bool HasTarget()
    {
        return target != null;
    }

    private Vector3 GetAimDirection()
    {
        if (HasTarget())
        {
            Vector3 targetPos = target.aimPoint.transform.position;
            return targetPos - transform.position;
        }
        return  transform.root.forward;
    }

    internal override bool GetAimOffsetWeight(out float offset)
    {
        bool baseHasTarget = base.GetAimOffsetWeight(out offset);
        if (HasTarget())
        {
            offset += Vector3.Angle(transform.up, GetAimDirection()) * AimWeight;
            return true;
        }
        return baseHasTarget;
    }
}
