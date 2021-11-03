using UnityEngine;
using SF = UnityEngine.SerializeField;

public class PlayerController : MonoBehaviour
{
    [SF] float MoveSpeed;                                       //Player movement speed
    [SF] float Jump;                                       //Player movement speed
    [SF] float RotSpeed;                                       //Player rotation speed
    [SF] PhysicalObject PO;

    private void Awake()
    {
        PO = GetComponent<PhysicalObject>();
        if (!PO) Debug.LogError("Player PO is empty!!");
    }

    private void FixedUpdate()
    {
        if (PO)
        {
            if (Input.GetKey(KeyCode.W))
                PO.Velocity += transform.forward * MoveSpeed * Time.fixedDeltaTime;
            if (Input.GetKey(KeyCode.A))
                PO.Velocity += -transform.right * MoveSpeed * Time.fixedDeltaTime;
            if (Input.GetKey(KeyCode.D))
                PO.Velocity += transform.right * MoveSpeed * Time.fixedDeltaTime;
            if (Input.GetKey(KeyCode.S))
                PO.Velocity += -transform.forward * MoveSpeed * Time.fixedDeltaTime;
            if (Input.GetKey(KeyCode.Q))
                transform.Rotate(Vector3.up, -(RotSpeed * Time.fixedDeltaTime), Space.Self);
            if (Input.GetKey(KeyCode.E))
                transform.Rotate(Vector3.up, RotSpeed * Time.fixedDeltaTime, Space.Self);
            if (Input.GetKey(KeyCode.Space))
                PO.Velocity += transform.up * Jump * Time.fixedDeltaTime;
        }
    }
}