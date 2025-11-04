using NUnit.Framework.Constraints;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Inputs.Haptics;

public class DroneHapticFeedback : MonoBehaviour
{
    public HapticImpulsePlayer hapticImpulsePlayer;
    public GameObject Drone;
    public GameObject DroneTarget;
    public float tolerance = 0.1f;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = (Drone.transform.position - DroneTarget.transform.position).magnitude;
        if (distance < tolerance)
        {
            hapticImpulsePlayer.SendHapticImpulse((tolerance - distance) / tolerance, Time.fixedDeltaTime);
        }
    }
}
