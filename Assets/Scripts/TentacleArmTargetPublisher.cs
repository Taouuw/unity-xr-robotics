using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using System.Linq;
using Unity.Robotics.ROSTCPConnector;
using Unity.Robotics.ROSTCPConnector.ROSGeometry;
using UnityEngine;

public class TentacleArmTargetPublisher : MonoBehaviour
{
    ROSConnection ros;

    public string topicName = "/unity/tentacle_arm_targets";
    private float armSpread;
    private float armBearing;
    private float tentacleSpread;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<JointStateMsg>(topicName);
    }

    public void SetArmSpread(float value)
    {
        armSpread = value;
        JointStateMsg msg = new JointStateMsg();//new PoseMsg(new PointMsg(transform.position.x, transform.position.y, transform.position.z), new QuaternionMsg(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
        msg.position = new[] { (double)armBearing + armSpread, (double)armBearing - armSpread };
        msg.name = new[] { "q_al", "q_ar" };
        ros.Publish(topicName, msg);
    }

    public void SetArmBearing(float value)
    {
        armBearing = value;
        JointStateMsg msg = new JointStateMsg();//new PoseMsg(new PointMsg(transform.position.x, transform.position.y, transform.position.z), new QuaternionMsg(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
        msg.position = new[] { (double)armBearing + armSpread, (double)armBearing - armSpread };
        msg.name = new[] { "q_al", "q_ar" };
        ros.Publish(topicName, msg);
    }

    public void SetTentacleSpread(float value)
    {
        tentacleSpread = value;
        JointStateMsg msg = new JointStateMsg();//new PoseMsg(new PointMsg(transform.position.x, transform.position.y, transform.position.z), new QuaternionMsg(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w));
        msg.position = new[]{(double)value, -(double)value};
        msg.name = new[]{ "q_ll", "q_lr" };
        ros.Publish(topicName, msg);
    }
}
