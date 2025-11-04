using UnityEngine;

public class trackLocalTransform : MonoBehaviour
{
    public Transform trackedTransform;
    public bool inverted = false;
    public bool x;
    public bool y;
    public bool z;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 newPosition;
        if (inverted)
        {
            newPosition = -trackedTransform.localPosition;

        } else
        {
            newPosition = trackedTransform.localPosition;
        }

        if (!x)
        {
            newPosition.x = 0.0f;
        }
        if (!y)
        {
            newPosition.y = 0.0f;
        }
        if (!z)
        {
            newPosition.z = 0.0f;
        }

        transform.localPosition = newPosition;
    }
}
