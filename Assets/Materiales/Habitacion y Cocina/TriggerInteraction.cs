using UnityEngine;
using UnityEngine.Events;

public class TriggerInteraction : MonoBehaviour
{
    [SerializeField] UnityEvent onTriggerEnter;
    [SerializeField] UnityEvent onTriggerExit;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            onTriggerEnter.Invoke();

        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {

            onTriggerExit.Invoke();

        }
    }

}
