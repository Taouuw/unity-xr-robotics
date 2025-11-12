using UnityEngine;

public class StayLevel : MonoBehaviour
{
    // Update is called once per frame
    void FixedUpdate()
    {
        Quaternion rotation = transform.rotation;
        transform.LookAt(transform.position + Vector3.ProjectOnPlane(transform.forward * 10f, Vector3.up) * 10f, Vector3.up);
    }
}