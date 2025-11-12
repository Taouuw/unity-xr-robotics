using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;
using RosMessageTypes.Geometry;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;


public class PosePublisher : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/unity/vehicle_target";
    double dt = 0.1;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        RosTopicState topicState = ros.GetTopic(topicName);
        if (!topicState.IsPublisher)
        {
            ros.RegisterPublisher<PoseMsg>(topicName);
        }
    }

    void PublishMessage()
    {
        PoseMsg msg = new PoseMsg();//new PoseMsg(new PointMsg(transform.position.x, transform.position.y, transform.position.z), new QuaternionMsg(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
        msg.position = transform.position.To<FLU>();
        msg.orientation = transform.rotation.To<FLU>();
        ros.Publish(topicName, msg);
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void FixedUpdate()
    {
        if ((Time.timeAsDouble % dt) < ((Time.timeAsDouble - Time.fixedDeltaTime) % dt))
        {
            PublishMessage();
        }
    }
}
