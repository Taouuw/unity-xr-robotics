using UnityEngine;

public class VisualInit : MonoBehaviour
{
    public Material material;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (Renderer child_renderer in gameObject.GetComponentsInChildren<Renderer>())
        {
            if (child_renderer.material == null)
            {
                child_renderer.material = material;
            }
        }

        foreach (MeshFilter meshFilter in gameObject.gameObject.GetComponentsInChildren<MeshFilter>())
        {
            if (meshFilter.sharedMesh == null)
            {
                Mesh[] meshes = Resources.FindObjectsOfTypeAll<Mesh>();
                foreach (Mesh mesh in meshes)
                {
                    //Debug.Log(mesh.name);
                    if (mesh.name == meshFilter.gameObject.name)
                    {
                        meshFilter.sharedMesh = mesh;
                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
