using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;
using RosMessageTypes.Px4;

public class UAMCommandSubscriber : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/uam/total_cmd";

    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<UAMCommandMsg>(topicName, ReceiveMessage);
    }

    bool isNaN(PointMsg p)
    {
        return double.IsNaN(p.x) || double.IsNaN(p.y) || double.IsNaN(p.z);
    }

    void ReceiveMessage(UAMCommandMsg msg)
    {

        transform.position = msg.uav_pose.position.From<FLU>();
        transform.rotation = msg.uav_pose.orientation.From<FLU>();

        if (isNaN(msg.uav_pose.position))
        {
            transform.position = Vector3.up * 100f;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
