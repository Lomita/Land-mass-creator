using UnityEngine;

/*
 * Represents a Sphere
 */
public class PhysicalSphere : PhysicalObject
{
    private float radius;                                       //The radius of the sphere
    public float Radius { get => radius; set => radius = value; }

    private void Start()
    {
        Velocity = this.MotionType.Equals(MotionTypes.Dynamic) ? InitialVeleocity : Vector3.zero;
        radius = transform.localScale.x / 2.0f;
        GeometryType = GeometryTypes.Sphere;
    }

    //SphereCubeCollision
    public override bool CheckCubeCollision(PhysicalCube cube, out Vector3 normal)
    {
        //AABB with Center at origin
        Bounds AABB = cube.AABB;
/*
# if DEBUG
        cube.DebugDrawBoundingBox(cube.Debug_AABB);
        cube.DebugDrawBoundingBox(AABB);
# endif
*/
        Quaternion cubeRotation = cube.transform.rotation;
        Vector3 pos = transform.position - cube.transform.position;

        //rotate the sphere position to match the AABB
        pos = Quaternion.Inverse(cubeRotation) * pos;

        //clamp to find the shortest point on the AABB
        Vector3 S = Clamp(pos, AABB.min, AABB.max);

        //get the normal for the possible collision point
        normal = (pos - S);
        float distance = normal.magnitude;
        
        //rotate normal back 
        normal = cubeRotation * normal.normalized;

        return distance < Radius;
    }

    //SphereSphereCollision
    public override bool CheckSphereCollision(PhysicalSphere sphere, out Vector3 normal)
    {
        Vector3 dir = sphere.transform.position - transform.position;
        normal = dir.normalized;
        return dir.magnitude <= Radius + sphere.Radius;
    }
}