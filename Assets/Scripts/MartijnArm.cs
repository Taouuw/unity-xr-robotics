using RosMessageTypes.Geometry;
using RosMessageTypes.Nav;
using RosMessageTypes.Sensor;
using System;
using System.Linq;
using Unity.Mathematics;
using Unity.Robotics.ROSTCPConnector;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.XR.Interaction.Toolkit.AffordanceSystem.State;

public class MartijnArm : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/unity/joint_states";
    public string[] jointNames = { "shoulder_joint_", "elbow_joint_", "forearm_joint_" };
    public Vector3[] axes = { Vector3.right, Vector3.right, Vector3.right };
    public Transform[] linkTransforms;

    float[] joint_states = {0,0,0};

    //Quaternion[] default_rotations = { Quaternion.identity, Quaternion.identity, Quaternion.identity};
    public Quaternion[] default_rotations = { };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<JointStateMsg>(topicName, ReceiveMessage);

        if (default_rotations.Length == 0)
        {
            default_rotations = new Quaternion[linkTransforms.Length];
            for (int i = 0; i < linkTransforms.Length; i++)
            {
                default_rotations[i] = linkTransforms[i].localRotation;
                axes[i] = axes[i].normalized;

            }
        }


    }

    // Update is called once per frame
    void FixedUpdate()
    {


        for (int i = 0; i < linkTransforms.Length; i++)
        {
            //Vector3 angles = axes[i] * ((joint_states[i] * 180 / (float)Math.PI) + 10f);
            linkTransforms[i].localRotation = default_rotations[i];
            // linkTransforms[i].localEulerAngles += axes[i] * joint_states[i] * 180 / (float)Math.PI;
            linkTransforms[i].Rotate(axes[i], joint_states[i] * 180 / (float)Math.PI, Space.Self);
            // linkTransforms[i].Rotate(linkTransforms[i].TransformVector(axes[i]), joint_states[i] * 180 / (float)Math.PI, relativeTo : Space.World); //(angles.x, angles.y, angles.z);
            // linkTransforms[i].Rotate(linkTransforms[i].TransformVector(axes[i]), joint_states[i] * 180 / (float)Math.PI); //(angles.x, angles.y, angles.z);
        }
    }

    void ReceiveMessage(JointStateMsg msg)
    {
        for (int i_joint = 0; i_joint < jointNames.Length; i_joint++)
        {
            int msg_index = Array.IndexOf(msg.name, jointNames[i_joint]);
            
            joint_states[i_joint] = (float)msg.position[msg_index];
        }
    }
}
