using RosMessageTypes.Geometry;
using RosMessageTypes.Sensor;
using RosMessageTypes.Std;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;

public class EventPublisher : MonoBehaviour
{
    ROSConnection ros;

    public string hoverTopicName = "/unity/switch_to_hover";
    public string resendTopicName = "/unity/resend";
    public string proceedTopicName = "/unity/proceed";
    public string forceTargetTopicName = "/unity/force_target";
    public string graspedNavigationTopicName = "/unity/grasped_navigation";
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<BoolMsg>(hoverTopicName);
        ros.RegisterPublisher<BoolMsg>(resendTopicName);
        ros.RegisterPublisher<BoolMsg>(proceedTopicName);
        ros.RegisterPublisher<BoolMsg>(graspedNavigationTopicName);
        ros.RegisterPublisher<Float64Msg>(forceTargetTopicName);
    }


    public void hover()
    {
        BoolMsg msg = new BoolMsg();
        msg.data = true;
        ros.Publish(hoverTopicName, msg);
    }

    public void set_grasped_navigation(bool is_grasped)
    {
        BoolMsg msg = new BoolMsg();
        msg.data = is_grasped;
        ros.Publish(graspedNavigationTopicName, msg);
    }

    public void resend()
    {
        BoolMsg msg = new BoolMsg();
        msg.data = true;
        ros.Publish(resendTopicName, msg);
    }

    public void resendAfterDelay(float delay)
    {
        Invoke(nameof(resend), delay);
    }

    public void proceed()
    {
        BoolMsg msg = new BoolMsg();
        msg.data = true;
        ros.Publish(proceedTopicName, msg);
    }

    public void setForceTarget(double force_target)
    {
        Float64Msg msg = new();
        msg.data = force_target;
        ros.Publish(forceTargetTopicName, msg);
    }
}
