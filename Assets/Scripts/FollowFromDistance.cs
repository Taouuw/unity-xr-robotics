using UnityEngine;
using UnityEngine.InputSystem;

public class FollowFromDistance : MonoBehaviour
{
    public Transform followedTransform;
    public InputActionAsset xriActions;
    private InputAction moveToFOV;
    public float range = 3f;
    public float height = 1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        moveToFOV = xriActions.FindAction("Custom/Button1");
        moveToFOV.performed += OnMoveToFOV;
        moveToFOV.Enable();
    }

    // Update is called once per frame
    void FixedUpdate()
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
        transform.position = followedTransform.position + range * 0.6f * followedTransform.forward.normalized + Vector3.down * (height * 0.5f);
    }

}
