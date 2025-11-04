using Unity.XR.CoreUtils;
using UnityEditor;
using UnityEngine;

public class CaptureTargetModel : MonoBehaviour
{
    public Material virtual_material;
    public GameObject model;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        model = GameObjectUtility.DuplicateGameObject(transform.parent.gameObject.GetNamedChild("model"));
        model.transform.SetParent(transform, false);
        foreach (Renderer model_renderer in model.GetComponentsInChildren<Renderer>())
        {
            model_renderer.material = virtual_material;
        }

        model.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
