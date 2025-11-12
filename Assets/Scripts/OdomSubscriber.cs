using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class OdomSubscriber : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/odom";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<OdometryMsg>(topicName, ReceiveMessage);
    }

    bool isNaN(PointMsg p)
    {
        return double.IsNaN(p.x) || double.IsNaN(p.y) || double.IsNaN(p.z);
    } 

    void ReceiveMessage(OdometryMsg msg)
    {

        transform.position = msg.pose.pose.position.From<FLU>();
        transform.rotation = msg.pose.pose.orientation.From<FLU>();

        if (isNaN(msg.pose.pose.position))
        {
            transform.position = Vector3.up * 100f;
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
}
