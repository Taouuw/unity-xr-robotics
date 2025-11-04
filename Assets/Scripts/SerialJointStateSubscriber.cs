
using System.Linq;
using UnityEngine;

public class SerialJointStateSubscriber : JointStateSubscriber
{
    public Transform tip;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected virtual void Awake()
    {
        if (jointNames.Length != axes.Length)
        {
            Debug.LogError("jointNames and axes are of different length: " + jointNames.Length + " != " + axes.Length);
        }

        Transform current_transform = tip;
        while (linkTransforms.Length < jointNames.Length)
        {
            linkTransforms = linkTransforms.Prepend(current_transform).ToArray();
            current_transform = current_transform.parent;
        }
        //public string topicName = "/unity/joint_states";

        //public string[] jointNames = { };
        //public Vector3[] axes = { };

        //public Transform[] linkTransforms;
        //float[] joint_states = { };

        //public Quaternion[] default_rotations = { };
    }

}
