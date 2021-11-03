using UnityEngine;
using SF = UnityEngine.SerializeField;

//TriggerBox Template
public class TriggerBox : Trigger
{
    protected override void OnEnter(PhysicalObject collision)
    {
        base.OnEnter(collision);
    }

    protected override void OnExit(PhysicalObject collision)
    {
        base.OnExit(collision);
    }

    protected override void OnStay(PhysicalObject collision)
    {
        base.OnStay(collision);
    }
}