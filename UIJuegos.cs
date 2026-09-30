using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIJuegos : MonoBehaviour
{
    [Header("Paneles")]
    public GameObject panelJuegos;
    public GameObject panelTuttiFrutti;
    public GameObject panelCrearPartida;

    [Header("Crear Partida UI")]
    public TMP_Text nombreJuegoText;
    public Image juegoImage;
    public TMP_Text nombreCreadorText;
    public Transform espaciosAmigos;
    public Button iniciarPartidaButton;

    [Header("Hoja Tutti Frutti")]
    public GameObject panelHojaTuttiFrutti;

    private void DesbloquearCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void BloquearCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void SeleccionarJuego(string nombreJuego)
    {
        Debug.Log("Juego seleccionado: " + nombreJuego);

        //Cerrar paneles anteriores
        if(panelJuegos != null)
        {
            panelJuegos.SetActive(false);
        }

        DesbloquearCursor();

        if (nombreJuego == "Tutti Frutti")
        {
            //Mostrar el panel para crear la partida
            if(panelTuttiFrutti != null)
            {
                panelTuttiFrutti.SetActive(true);
            }
            else
            {
                if(panelTuttiFrutti != null)
                {
                    panelTuttiFrutti.SetActive(false);
                }
            }
        }
    }

    public void CerrarPanel(GameObject panel)
    {
        panel.SetActive(false);
        BloquearCursor();
    }

    public void AbrirCrearPartida()
    {
        DesbloquearCursor();
        if (panelCrearPartida != null)
        {
            panelCrearPartida.SetActive(true);

            //Mostrar info del juego
            if(nombreJuegoText != null)
            {
                nombreJuegoText.text = "Tutti Frutti";
            }

            if(juegoImage != null)
            {
                //Asignar la imagen del juego (a implementar)
            }

            if (nombreCreadorText != null)
            {
                nombreCreadorText.text = "Creador: Jugador1";
            }

            //Preparar espacios de amigos
            foreach(Transform t in espaciosAmigos)
            {
                t.gameObject.SetActive(false);
            }
        }

        //Cerrar panel anterior
        if(panelTuttiFrutti != null)
        {
            panelTuttiFrutti.SetActive(false);
        }
    }

    public void IniciarPartida()
    {
        //Cerrar panel de crear partida
        if(panelCrearPartida != null)
        {
            panelCrearPartida.SetActive(false);
        }

        //Mostrar la hoja de tutti frutti
        if(panelHojaTuttiFrutti != null)
        {
            panelHojaTuttiFrutti.SetActive(true);
            DesbloquearCursor();
        }
    }
}
