using System.Collections.Generic;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public class PhysicsManager : MonoBehaviour
{
    [SF] private Vector3 Gravity;                               //World gravity

    private List<PhysicalObject> StaticObjects;                 //a list of all static Physicalobjects
    private List<PhysicalObject> DynamicObjects;                //a list of all Dynamic Physicalobjects

    private void Awake()
    {
        StaticObjects = new List<PhysicalObject>();
        DynamicObjects = new List<PhysicalObject>();     
    }

    private void Start()
    {
        GetPhysicalObjects();
    }

    private void FixedUpdate()
    {
        Move();

        foreach (PhysicalObject PO_Dynamic in DynamicObjects)
        {
            foreach (PhysicalObject PO_Static in StaticObjects)
            {
                Vector3 normal = Vector3.zero;
                if (PO_Dynamic.CheckForCollision(PO_Static, out normal))
                {
                    if (PO_Static.IsTrigger) PO_Static.Triggered(PO_Dynamic);
                    if (PO_Dynamic.IsTrigger) PO_Dynamic.Triggered(PO_Static);

                    if (PO_Static.IgnoreCollision || PO_Dynamic.IgnoreCollision)
                        continue;

                    Debug.DrawRay(PO_Dynamic.transform.position, normal, Color.red);
                    float bouncinessFactor = PO_Dynamic.Bounciness * PO_Static.Bounciness;
                    PO_Dynamic.VelocityChangeCollision(normal, bouncinessFactor);

                    //unstuck
                    PO_Dynamic.transform.position += (normal * 0.1f) * Time.fixedDeltaTime;
                }
                else
                {
                    if (PO_Static.IsTrigger && PO_Static.IsInsideTrigger) PO_Static.ExitTrigger(PO_Dynamic);
                    if (PO_Dynamic.IsTrigger && PO_Static.IsInsideTrigger) PO_Dynamic.ExitTrigger(PO_Static);
                }
            }
        }
    }

    /*
     * Adds Gravity to all Dynamic Objects and moves them according to their velocity
     */
    private void Move()
    { 
        foreach (PhysicalObject PO_Dynamic in DynamicObjects)
        {
            PO_Dynamic.Velocity += Gravity * Time.fixedDeltaTime;
            PO_Dynamic.Move();
        }
    }

    /*
     * Get all PhysicalObjects in the scene
     */
    private void GetPhysicalObjects()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PhysicalObject");
        if (objs.Length.Equals(0))
        {
            Debug.LogError("No PhysicalObject Found!");
            return;
        }
        
        foreach (GameObject o in objs)
        {
            PhysicalObject po = o.GetComponent<PhysicalObject>();
            if (!po)
            {
                Debug.LogError("GameObject has no PhysicalObject Assigned: " + o.ToString());
                continue;
            }

            if (po.MotionType.Equals(MotionTypes.Dynamic))
                DynamicObjects.Add(po);
            else
                StaticObjects.Add(po);
        }
    }
}