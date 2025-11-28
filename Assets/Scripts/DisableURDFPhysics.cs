using Unity.Robotics.UrdfImporter.Control;
using Unity.XR.CoreUtils;
using UnityEngine;

public class DisableURDFPhysics : MonoBehaviour
{
    // public int layerIndex = -1;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (ArticulationBody body in gameObject.GetComponentsInChildren<ArticulationBody>())
        {
            body.enabled = false;
        }

        foreach (Collider collider in gameObject.GetComponentsInChildren<Collider>())
        {
            collider.enabled = false;
            // if (layerIndex > 0)
            // {
            //     collider.gameObject.layer = layerIndex;
            // } else
            // {
            // 
            // }
        }

        Unity.Robotics.UrdfImporter.Control.Controller controller = GetComponent<Unity.Robotics.UrdfImporter.Control.Controller>();
        if (controller != null)
        {
            controller.enabled = false;
        }

        FKRobot fk = GetComponent<FKRobot>();
        if (fk != null)
        {
            fk.enabled = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
