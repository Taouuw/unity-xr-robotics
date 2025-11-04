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

public class JointStateSubscriber : MonoBehaviour
{
    ROSConnection ros;
    public string topicName = "/unity/joint_states";
    public string[] jointNames = {};
    public Vector3[] axes = {};
    public Transform[] linkTransforms;
    public bool IsJointNamesFromFirstMessage = false;

    protected float[] joint_states = {};

    //Quaternion[] default_rotations = { Quaternion.identity, Quaternion.identity, Quaternion.identity};
    public Quaternion[] default_rotations = {};

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        joint_states = Enumerable.Repeat(0.0f, jointNames.Length).ToArray();

        ros = ROSConnection.GetOrCreateInstance();

        ros.Subscribe<JointStateMsg>(topicName, ReceiveMessage);

        InitializeDefaultRotations();
    }

    void InitializeDefaultRotations()
    {
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
            if (math.abs(joint_states[i]) > 0.01f)
            {
                Debug.Log(joint_states[i]);
            }

            // linkTransforms[i].Rotate(linkTransforms[i].TransformVector(axes[i]), joint_states[i] * 180 / (float)Math.PI, relativeTo : Space.World); //(angles.x, angles.y, angles.z);
            // linkTransforms[i].Rotate(linkTransforms[i].TransformVector(axes[i]), joint_states[i] * 180 / (float)Math.PI); //(angles.x, angles.y, angles.z);
        }
    }

    void ReceiveMessage(JointStateMsg msg)
    {
        if (jointNames.Length == 0 && IsJointNamesFromFirstMessage)
        {
            // initialize based on the first message
            jointNames = msg.name;
            joint_states = Enumerable.Repeat(0.0f, jointNames.Length).ToArray();
            default_rotations = Enumerable.Repeat(Quaternion.identity, jointNames.Length).ToArray();

            InitializeDefaultRotations();
        }

        for (int i_joint = 0; i_joint < jointNames.Length; i_joint++)
        {
            int msg_index = Array.IndexOf(msg.name, jointNames[i_joint]);
            joint_states[i_joint] = (float)msg.position[msg_index];
        }
    }
}

