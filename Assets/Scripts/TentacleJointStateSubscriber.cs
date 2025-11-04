using System.Linq;
using System.Runtime.CompilerServices;
using Unity.IO.LowLevel.Unsafe;
using UnityEngine;

public class TentacleJointStateSubscriber : SerialJointStateSubscriber
{

    public bool is_left = false;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    protected override void Awake()
    {
        string arm_name;
        if (is_left) 
        {
            arm_name = "left"; 
        } else
        {
            arm_name = "right";
        }

        int n = 18;

        for (int i = 1; i < n; i++) 
        {
            jointNames = jointNames.Append("flex_joint_" + arm_name + "_from_" + (i) + "_to_" + (i+1)).ToArray();
        }

        axes = Enumerable.Repeat(Vector3.down, n-1).ToArray();

        base.Awake();
        
        //public string topicName = "/unity/joint_states";
        //public string[] jointNames = { };
        //public Vector3[] axes = { };
        //public Transform[] linkTransforms;
        //
        //float[] joint_states = { };
        //public Quaternion[] default_rotations = { };
    }

}
