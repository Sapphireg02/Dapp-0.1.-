using UnityEngine;

public class SeleccionColor : MonoBehaviour
{
    public PlayerLudo jugador;
    public GameObject menuColores;

    public Transform posRojo;
    public Transform posAzul;
    public Transform posVerde;
    public Transform posAmarillo;

    public Camera cameraJugador;
    public Transform camaraSup;

    public void ElegirRojo()
    {
        jugador.AsignarColor("Rojo");
        jugador.transform.position = posRojo.position;
        cameraJugador.transform.position = camaraSup.position;
        cameraJugador.transform.rotation = camaraSup.rotation;
        menuColores.SetActive(false);
    }
    public void ElegirAzul()
    {
        jugador.AsignarColor("Azul");
        jugador.transform.position = posAzul.position;
        cameraJugador.transform.position = camaraSup.position;
        cameraJugador.transform.rotation = camaraSup.rotation;
        menuColores.SetActive(false);
    }

    public void ElegirVerde()
    {
        jugador.AsignarColor("Verde");
        jugador.transform.position = posVerde.position;
        cameraJugador.transform.position = camaraSup.position;
        cameraJugador.transform.rotation = camaraSup.rotation;
        menuColores.SetActive(false);
    }

    public void ElegirAmarillo()
    {
        jugador.AsignarColor("Amarillo");
        jugador.transform.position = posAmarillo.position;
        cameraJugador.transform.position = camaraSup.position;
        cameraJugador.transform.rotation = camaraSup.rotation;
        menuColores.SetActive(false);
    }
}
