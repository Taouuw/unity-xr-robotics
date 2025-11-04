using RosMessageTypes.Geometry;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;
using UnityEngine.UIElements;

public class PoseSubscriber : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/unity/vehicle_pose";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<PoseMsg>(topicName, ReceiveMessage);
    }

    void ReceiveMessage(PoseMsg msg)
    {

        transform.position = msg.position.From<FLU>();
        transform.rotation = msg.orientation.From<FLU>();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
