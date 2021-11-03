using UnityEngine;
using SF = UnityEngine.SerializeField;

public abstract class Trigger : MonoBehaviour
{

    [SF] private PhysicalObject PO;

    private void Awake()
    {
        PO = GetComponent<PhysicalObject>();

        if (PO)
        {
            PO.TriggerEnter += OnEnter;
            PO.TriggerStay += OnStay;
            PO.TriggerExit += OnExit;
        }
        else
        {
            Debug.LogError(gameObject.name + " is Trigger but no PhysicalObject found on");
            return;
        }          
    }

    virtual protected void OnEnter(PhysicalObject collision) { Debug.Log("OnTriggerEnter: " + collision.name + " enter " + PO.name); }
    virtual protected void OnStay(PhysicalObject collision) { Debug.Log("OnTriggerStay: " + collision.name + " stay in " + PO.name); }
    virtual protected void OnExit(PhysicalObject collision) { Debug.Log("OnTriggerExit: " + collision.name + " exit " + PO.name); }
}