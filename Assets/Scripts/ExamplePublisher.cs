using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Std;


public class ExamplePublisher : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/unity_chatter_publisher";

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        ros.RegisterPublisher<StringMsg>(topicName);

        InvokeRepeating(nameof(PublishMessage), 2f, 2f);
    }

    void PublishMessage()
    {
        StringMsg msg = new StringMsg("Hello I am unity");
        ros.Publish(topicName, msg);
        Debug.Log("Published " + msg);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
