using UnityEngine;

public class CorrectionPosition : MonoBehaviour
{
    private Vector3 correctPosition = Vector3.zero;
    private Vector3 currentPosition = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        correctPosition = transform.localPosition;
        currentPosition = correctPosition;
    }

    // Update is called once per frame
    void Update()
    {
        currentPosition = this.transform.localPosition;

        if(currentPosition != correctPosition)
        {
            this.transform.localPosition = correctPosition;
        }
    }
}
