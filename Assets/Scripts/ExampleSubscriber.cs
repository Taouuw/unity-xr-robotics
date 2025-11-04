using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;

public class ExampleSubscriber : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/unity_chatter_subscriber";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        
        ros.Subscribe<StringMsg>(topicName, ReceiveMessage);
    }

    void ReceiveMessage(StringMsg msg)
    {
        Debug.Log("Receiver from ros:  " + msg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
