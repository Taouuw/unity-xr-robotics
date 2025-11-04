using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit.Locomotion.Teleportation;

public class PositionInitXr : MonoBehaviour
{
    private TeleportationAnchor teleportationAnchor;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        teleportationAnchor = GetComponent<TeleportationAnchor>();
        Invoke(nameof(teleportationAnchor.RequestTeleport), 0.5f);
        Destroy(teleportationAnchor, 1.0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
