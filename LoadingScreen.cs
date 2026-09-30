using UnityEngine;
using System.Collections;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen;

    private void Start()
    {
        StartCoroutine(EsperarCarga());
    }

    private IEnumerator EsperarCarga()
    {
        // Espera hasta que la escena esté completamente cargada
        while (GameObject.FindWithTag("dappi") == null)
        {
            yield return null;
        }

        //Delay para suavizar transición
        yield return new WaitForSeconds(0.3f);

        //Desactiva pantalla de carga
        loadingScreen.SetActive(false);
    }
}
