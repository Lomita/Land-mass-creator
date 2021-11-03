using UnityEngine;

/*
 * Represents a Cube
 */
public class PhysicalCube : PhysicalObject
{
    private Bounds aabb;                                        // Axis-aligned bounding box for collision detection
    private Bounds debug_AABB;                                  // Debug ABB at cube position

    public Bounds AABB { get => aabb; set => aabb = value; }
    public Bounds Debug_AABB { get => debug_AABB; set => debug_AABB = value; }

    private void Start()
    {
        Velocity = this.MotionType.Equals(MotionTypes.Dynamic) ? InitialVeleocity : Vector3.zero;
        GeometryType = GeometryTypes.Cube;
        UpdateAABB();
    }

    /* Only use if the Objects bounding box changes
       Create a boundingbox at the origin*/
    private void UpdateAABB()
    {
        aabb = new Bounds(Vector3.zero, transform.localScale);
        debug_AABB = new Bounds(transform.position, transform.localScale);
    }

    // CubeCube
    public override bool CheckCubeCollision(PhysicalCube cube, out Vector3 normal)
    {
        Debug.LogError("throw new System.NotImplementedException()");
        normal = Vector3.zero;
        return false;
    }

    //CubeSphere
    public override bool CheckSphereCollision(PhysicalSphere sphere, out Vector3 normal)
    {
        Debug.LogError("throw new System.NotImplementedException()");
        normal = Vector3.zero;
        return false;
    }

    public void DebugDrawBoundingBox(Bounds AABB, float duration = 0.1f)
    {
        if (AABB == null)
            return;

        Vector3 min = AABB.min;
        Vector3 max = AABB.max;
        Vector3 center = AABB.center;

        Vector3 xnn = new Vector3(max.x, min.y, min.z);
        Vector3 nnx = new Vector3(min.x, min.y, max.z);
        Vector3 xnx = new Vector3(max.x, min.y, max.z);

        Vector3 nxn = new Vector3(min.x, max.y, min.z);
        Vector3 nxx = new Vector3(min.x, max.y, max.z);
        Vector3 xxn = new Vector3(max.x, max.y, min.z);

        //down rect
        Debug.DrawLine(min, xnn, Color.yellow, duration);
        Debug.DrawLine(min, nnx, Color.yellow, duration);
        Debug.DrawLine(xnx, xnn, Color.yellow, duration);
        Debug.DrawLine(xnx, nnx, Color.yellow, duration);

        //top rect
        Debug.DrawLine(max, nxx, Color.yellow, duration);
        Debug.DrawLine(max, xxn, Color.yellow, duration);
        Debug.DrawLine(nxn, nxx, Color.yellow, duration);
        Debug.DrawLine(nxn, xxn, Color.yellow, duration);

        //connections
        Debug.DrawLine(min, nxn, Color.yellow, duration);
        Debug.DrawLine(nnx, nxx, Color.yellow, duration);
        Debug.DrawLine(xnx, max, Color.yellow, duration);
        Debug.DrawLine(xnn, xxn, Color.yellow, duration);

        //diagonals
        Debug.DrawLine(center, min, Color.red, duration);
        Debug.DrawLine(center, max, Color.blue, duration);
    }
}