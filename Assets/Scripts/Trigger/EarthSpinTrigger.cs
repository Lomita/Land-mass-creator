using UnityEngine;
using SF = UnityEngine.SerializeField;

public class EarthSpinTrigger : Trigger
{
    [SF] private Transform Earth;
    [SF] private float SpinSpeed = 60.0f;
    [SF] private Vector3 SpinDirection = Vector3.up;

    protected override void OnStay(PhysicalObject collision)
    {
        if(Earth) Earth.Rotate(SpinDirection, SpinSpeed * Time.fixedDeltaTime);
        base.OnStay(collision);
    }
}