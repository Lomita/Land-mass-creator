using System;
using UnityEngine;
using SF = UnityEngine.SerializeField;

public enum MotionTypes
{
    Static,
    Dynamic
}

public enum GeometryTypes
{ 
    Sphere,
    Cube
}

public abstract class PhysicalObject : MonoBehaviour
{
    [SF] protected MotionTypes motionType;                      // Motiontype of the Object
    [SF] protected GeometryTypes geometryType;
    [SF] protected float mass = 1.0f;                           // Hold the Mass of the Object
    [SF] protected float bounciness = 1.0f;                     // Bounciness of a Object
    [SF] protected Vector3 InitialVeleocity;                    // Inital Velocity of the Object
    [SF] protected bool isTrigger = false;                      // Defines if the Object is a Trigger or not
    [SF] protected bool ignoreCollision = false;                // Defines if the Object ingores the collision
    protected bool isInsideTrigger = false;                // Defines if the Object is inside of the Trigger or not

    [SF] protected Vector3 velocity;                            // Current Velocity of the Object

    public Vector3 Velocity { get => velocity; set => velocity = value; }
    public MotionTypes MotionType { get => motionType; set => motionType = value; }
    public GeometryTypes GeometryType { get => geometryType; set => geometryType = value; }
    public float Bounciness { get => bounciness; set => bounciness = value; }
    public float Mass { get => mass; set => mass = value; }
    public bool IsTrigger { get => isTrigger; set => isTrigger = value; }
    public bool IgnoreCollision { get => ignoreCollision; set => ignoreCollision = value; }
    public bool IsInsideTrigger { get => isInsideTrigger; set => isInsideTrigger = value; }

    public event Action<PhysicalObject> TriggerEnter = delegate { };  //TriggerEnter is called when the Colliding Object enters the Trigger
    public event Action<PhysicalObject> TriggerStay = delegate { };  //TriggerStay is called when the Colliding Object stays inside the trigger
    public event Action<PhysicalObject> TriggerExit = delegate { };  //TriggerExit is called when the Colliding Object has stopped touching the trigger.                              

    public void Awake()
    {
        gameObject.tag = "PhysicalObject";
    }

    public void Move()
    {
        if(Velocity.magnitude > 0.1f)
            transform.Translate(Velocity * Time.fixedDeltaTime);
    }

    public void VelocityChangeCollision(Vector3 normal, float bouncinessFactor)
    {
        Velocity = Quaternion.AngleAxis(180.0f, normal) * Velocity;
        Vector3 v = Velocity;

        Velocity *= -bouncinessFactor;

        //limit bounce velocity 
        if (Velocity.magnitude > 5.0f)
            Velocity = v;
    }

    public bool CheckForCollision(PhysicalObject PO, out Vector3 normal)
    {
        return PO.GeometryType.Equals(GeometryTypes.Sphere) ? 
            CheckSphereCollision((PhysicalSphere)PO, out normal) : CheckCubeCollision((PhysicalCube)PO, out normal);
    }

    public static Vector3 Clamp(Vector3 value, Vector3 min, Vector3 max)
    {
        Vector3 c = Vector3.zero;
        c.x = Mathf.Clamp(value.x, min.x, max.x);
        c.y = Mathf.Clamp(value.y, min.y, max.y);
        c.z = Mathf.Clamp(value.z, min.z, max.z);
        return c;
    }

    public void Triggered(PhysicalObject PO)
    {
        if (!isInsideTrigger)
        {
            TriggerEnter(PO);
            isInsideTrigger = true;
        }
        else
            TriggerStay(PO);
    }

    public void ExitTrigger(PhysicalObject PO)
    {
        if (isInsideTrigger)
        {
            isInsideTrigger = false;
            TriggerExit(PO);
        }
    }

    abstract public bool CheckSphereCollision(PhysicalSphere sphere, out Vector3 normal);
    abstract public bool CheckCubeCollision(PhysicalCube cube, out Vector3 normal);
}