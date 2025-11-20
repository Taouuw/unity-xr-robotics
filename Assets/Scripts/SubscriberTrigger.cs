using RosMessageTypes.Px4;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

public class SubscriberTrigger : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/uam/total_cmd";

    public Material ReceivedMaterial;
    public Material NotReceivedMaterial;

    float time_stop;
    float duration = 0.1f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        time_stop = Time.time;
        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<UAMCommandMsg>(topicName, ReceiveMessage);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if ((Time.time - Time.fixedDeltaTime < time_stop) && (Time.time > time_stop))
        {
            SetChildMaterials(NotReceivedMaterial);
        }
    }

    void ReceiveMessage(UAMCommandMsg msg)
    {
        SetChildMaterials(ReceivedMaterial);
        time_stop = Time.time + duration;
    }

    void SetChildMaterials(Material material)
    {
        foreach (Renderer child_renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            child_renderer.material = material;
        }
    }
}
