using UnityEngine;

public class LineRendererWire : MonoBehaviour 
{
    public GameObject[] points;
    private LineRenderer line;

    void Start()
    {
        line = this.gameObject.GetComponent<LineRenderer>();
    }

    private void Update()
    {
        for(int i = 0; i < points.Length;i++)
        {
            line.SetPosition(i, points[i].transform.position);
        }
    }

}
