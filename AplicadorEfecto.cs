using UnityEngine;
using UnityEngine.UI;

public class ApEfectoBotones : MonoBehaviour
{
    [Header("Parámetros del efecto")]
    // Header es una etiqueta visual para el inspector
    public float scaleMultiplier = 1.1f;
    public float transitionSpeed = 10f;

    void Start()
    {
        //Busca los botones hijos
        Button[] botones = GetComponentsInChildren<Button>();

        //foreach es como un for pero más limpio
        foreach (Button b in botones)
        {
            //Agrega el efecto a los que no lo tienen
            if (b.GetComponent<EfectoBotones>() == null)
            {
                EfectoBotones effect = b.gameObject.AddComponent<EfectoBotones>();
                effect.scaleMultiplier = scaleMultiplier;
                effect.transitionSpeed = transitionSpeed;
            }
        }
    }
}
