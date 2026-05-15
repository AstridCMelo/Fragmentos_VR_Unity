using UnityEngine;

public class CajonVR : MonoBehaviour
{
    public Transform objetoHijo; // Aquí arrastrarás el cuaderno desde el Inspector
    public float distanciaZ = -0.4f;
    public float velocidad = 5f;

    private Vector3 cajonCerrado;
    private Vector3 cajonAbierto;
    private Vector3 cuadernoCerrado;
    private Vector3 cuadernoAbierto;

    private bool abierto = false;

    void Start()
    {
        // Posiciones del cajón
        cajonCerrado = transform.localPosition;
        cajonAbierto = cajonCerrado + new Vector3(0, 0, distanciaZ);

        // Posiciones del cuaderno (si existe)
        if (objetoHijo != null)
        {
            cuadernoCerrado = objetoHijo.localPosition;
            cuadernoAbierto = cuadernoCerrado + new Vector3(0, 0, distanciaZ);
        }
    }

    public void AlternarCajon()
    {
        abierto = !abierto;
        StopAllCoroutines();
        StartCoroutine(MoverTodo());
    }

    System.Collections.IEnumerator MoverTodo()
    {
        Vector3 destinoCajon = abierto ? cajonAbierto : cajonCerrado;
        Vector3 destinoCuaderno = abierto ? cuadernoAbierto : cuadernoCerrado;

        while (Vector3.Distance(transform.localPosition, destinoCajon) > 0.001f)
        {
            float step = Time.deltaTime * velocidad;
            transform.localPosition = Vector3.Lerp(transform.localPosition, destinoCajon, step);

            if (objetoHijo != null)
                objetoHijo.localPosition = Vector3.Lerp(objetoHijo.localPosition, destinoCuaderno, step);

            yield return null;
        }

        transform.localPosition = destinoCajon;
        if (objetoHijo != null) objetoHijo.localPosition = destinoCuaderno;
    }
}