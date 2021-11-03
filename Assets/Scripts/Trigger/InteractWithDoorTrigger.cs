using UnityEngine;
using SF = UnityEngine.SerializeField;

public class InteractWithDoorTrigger : Trigger
{
    [SF] private Transform DoorPivot;
    [SF] private float RotAngle;
    [SF] private AudioSource EnterSound;
    [SF] private AudioSource ExitSound;

    private Vector3 RotAxis = Vector3.up;
    private bool isOpen = false;

    protected override void OnEnter(PhysicalObject collision)
    {
        if(EnterSound) EnterSound.Play();
        base.OnEnter(collision);
    }

    protected override void OnStay(PhysicalObject collision)
    {
        if (DoorPivot && Input.GetKey(KeyCode.F) && !isOpen)
        {
            DoorPivot.Rotate(isOpen ? -RotAxis : RotAxis, RotAngle);
            isOpen = true;
        } 

        base.OnStay(collision);
    }

    protected override void OnExit(PhysicalObject collision)
    {
        if (ExitSound) ExitSound.Play();
        base.OnExit(collision);
    }
}