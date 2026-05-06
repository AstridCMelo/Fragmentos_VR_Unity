using System;
using UnityEngine;

public class StationTrigger : MonoBehaviour
{
    private float center;
    [SerializeField] GameObject knobController;

    private void OnTriggerEnter(Collider other)
    {
        BoxCollider col = GetComponent<BoxCollider>();
        float center = col.center.z;
        float halfWidth = col.size.z / 2f;

        Debug.Log("Trigger ok");

        if (knobController.TryGetComponent<IChangeTrack>(out IChangeTrack changeTrack))
        {
            changeTrack.IndicatorTrack(other.transform.localPosition.z, center, halfWidth);
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


