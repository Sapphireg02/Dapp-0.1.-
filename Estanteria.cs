using UnityEngine;

public class Estanteria : MonoBehaviour
{
    public GameObject panelJuegos; 

    void OnMouseDown() 
    {
        //Mostrar/ocultar panel
        if (panelJuegos != null)
        {
            panelJuegos.SetActive(true);
        }
    }
}
