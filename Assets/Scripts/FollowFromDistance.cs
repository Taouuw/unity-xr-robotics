using UnityEngine;
using UnityEngine.InputSystem;

public class FollowFromDistance : MonoBehaviour
{
    public Transform followedTransform;
    public InputActionAsset xriActions;
    private InputAction moveToFOV;
    public float range = 3;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveToFOV = xriActions.FindAction("Custom/Button1");
        moveToFOV.performed += OnMoveToFOV;
        moveToFOV.Enable();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 relative_position = followedTransform.position - transform.position;
        float distance = relative_position.magnitude - range;
        if (distance > 0)
        {
            transform.position += relative_position.normalized * distance;
        }
    }

    public void OnMoveToFOV(InputAction.CallbackContext ctx)
    {
        transform.position = followedTransform.position + range * 0.6f * Vector3.ProjectOnPlane(followedTransform.forward, Vector3.up).normalized + Vector3.down * 0.3f;
    }

}
