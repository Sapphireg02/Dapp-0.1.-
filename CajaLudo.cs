using UnityEngine;
using UnityEngine.InputSystem;

public class CajaLudo : MonoBehaviour
{
    private Renderer rend;
    private Color colorOriginal;
    public Color colorResaltado = Color.yellow; //Cuando pasa el mouse
    public float intensidad = 2f;
    public float rangoRaycast = 100f; //Rango para que se ilumine

    public Camera cameraJugador;

    public GameObject menuColores; //Panel con los botones

    private void Start()
    {
        rend = GetComponent<Renderer>();
        colorOriginal = rend.material.GetColor("_EmissionColor");
        //Activa la emisión
        rend.material.EnableKeyword("_EMISSION");
    }

    private void Update()
    {
        //Raycast desde la cámara hacia el mouse
        Vector3 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cameraJugador.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rangoRaycast))
        {
            if (hit.collider == GetComponent<Collider>())
            {
                rend.material.SetColor("_EmissionColor", colorResaltado * intensidad);
            }
            else
            {
                rend.material.SetColor("_EmissionColor", colorOriginal);
            }
        }
        else
        {
            rend.material.SetColor("_EmissionColor", colorOriginal);
        }
    }

    private void OnMouseDown()
    {
        //Activar el menú de selección
        if(menuColores != null)
        {
            menuColores.SetActive(true);
        }
    }
}
