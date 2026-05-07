using System;
using UnityEngine;

public class StationTrigger : MonoBehaviour
{
    private float center;
    private float halfWidth;
    [SerializeField] GameObject knobController;


    private void OnTriggerEnter(Collider other)
    {
        BoxCollider col = GetComponent<BoxCollider>();
        center = transform.localPosition.z;
        halfWidth = 0.3f;

        Debug.Log(halfWidth);

        if (knobController.TryGetComponent<IChangeTrack>(out IChangeTrack changeTrack))
        {
            changeTrack.IndicatorTrack(center, halfWidth);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (knobController.TryGetComponent<IChangeTrack>(out IChangeTrack changeTrack))
        {
            changeTrack.IndicatorTrack(center, halfWidth);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (knobController.TryGetComponent<IChangeTrack>(out IChangeTrack changeTrack))
        {
            changeTrack.NoiseWithoutTrack();
        }
    }
}


