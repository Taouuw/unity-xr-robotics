using Unity.Robotics.UrdfImporter.Control;
using UnityEngine;

public class DisableURDFPhysics : MonoBehaviour
{
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
        }

        Controller controller = GetComponent<Controller>();
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
