using UnityEngine;

public class OutlineObject : MonoBehaviour
{
    [SerializeField] private Material hovermaterial;
    [SerializeField] private Material Nonhovermaterial;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void Start()
    {
        OnHoverExit();
    }
    public void OnHoverEnter()
    {
        Renderer  rendObject = GetComponent<Renderer>();
        Material[] materials  = rendObject.materials;
        materials[materials.Length - 1] = hovermaterial;
        rendObject.materials = materials;
        Debug.Log("hover");
        Debug.Log(rendObject.materials[materials.Length - 1].name);
    }

    public void OnHoverExit() 
    {
        Renderer rendObject = GetComponent<Renderer>();
        Material[] materials = rendObject.materials;
        materials[materials.Length - 1] = Nonhovermaterial;
        rendObject.materials = materials;
    }
}
