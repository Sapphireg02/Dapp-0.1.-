using UnityEngine;

public class PlayerLudo : MonoBehaviour
{
    public string colorAsignado; //Color elegido por el jugador

    public void AsignarColor(string color)
    {
        colorAsignado = color;
        Debug.Log("Color asignado: " + colorAsignado);
    }
}
