using UnityEditor;
using UnityEngine;

public class MaterialInit : MonoBehaviour
{
    public Material material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        setChildrenMaterial(gameObject, material);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setChildrenMaterial(GameObject gameObject, Material material)
    {
        foreach (Renderer child_renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            child_renderer.material = material;
        }
    }
}
