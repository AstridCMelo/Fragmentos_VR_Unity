using UnityEngine;

public class CajonVR : MonoBehaviour
{
    public Transform objetoHijo; // El cuaderno (Notebook_Brown)
    public float distanciaZ = -0.4f;
    public float velocidad = 5f;

    private Vector3 cajonCerrado;
    private Vector3 cajonAbierto;

    private bool abierto = false;
    private Rigidbody rbCuaderno;
    private bool gravedadActivadaParaSiempre = false;
    private Vector3 offsetInicial;

    void Start()
    {
        cajonCerrado = transform.localPosition;
        cajonAbierto = cajonCerrado + new Vector3(0, 0, distanciaZ);

        if (objetoHijo != null)
        {
            rbCuaderno = objetoHijo.GetComponent<Rigidbody>();
            offsetInicial = objetoHijo.position - transform.position;

            // Aseguramos congelarlo en el frame 1 antes de que caiga
            if (rbCuaderno != null)
            {
                rbCuaderno.isKinematic = true;
                rbCuaderno.useGravity = false;
            }
        }
    }

    public void AlternarCajon()
    {
        abierto = !abierto;
        StopAllCoroutines();
        StartCoroutine(MoverTodoSuave());
    }

    System.Collections.IEnumerator MoverTodoSuave()
    {
        Vector3 destinoCajon = abierto ? cajonAbierto : cajonCerrado;
        float tiempo = 0;
        Vector3 posInicialCajon = transform.localPosition;

        while (tiempo < 1f)
        {
            tiempo += Time.deltaTime * velocidad;
            transform.localPosition = Vector3.Lerp(posInicialCajon, destinoCajon, tiempo);

            if (gravedadActivadaParaSiempre)
            {
                yield break;
            }

            if (objetoHijo != null && !gravedadActivadaParaSiempre)
            {
                objetoHijo.position = transform.position + offsetInicial;
            }

            yield return null;
        }

        transform.localPosition = destinoCajon;
    }

    // Se activa al agarrarlo desde el evento del XR Grab Interactable
    public void ActivarGravedadDesdeVR()
    {
        if (!gravedadActivadaParaSiempre)
        {
            gravedadActivadaParaSiempre = true;

            if (rbCuaderno != null)
            {
                rbCuaderno.isKinematic = false;
                rbCuaderno.useGravity = true;
            }

            Debug.Log("¡Cuaderno liberado! El script del cajón ya no lo tocará más.");

            // Desactivamos este script para que no vuelva a evaluar al cuaderno nunca más
            this.enabled = false;
        }
    }
}