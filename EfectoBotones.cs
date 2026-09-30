using UnityEngine;
using UnityEngine.EventSystems;

public class EfectoBotones : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Parámetros del efecto")]
    public float scaleMultiplier = 1.1f;
    public float transitionSpeed = 12f;

    private Vector3 originalScale;
    private Vector3 targetScale;

    void Start()
    {
        originalScale = transform.localScale;
        targetScale = originalScale;
    }

    void Update()
    {
        //Transición suave de escala
        transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * transitionSpeed);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetScale = originalScale * scaleMultiplier;
        Debug.Log("Mouse sobre el botón: " + gameObject.name);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetScale = originalScale;
    }
}
