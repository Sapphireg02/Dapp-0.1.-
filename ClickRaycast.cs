using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;
using System.Runtime.CompilerServices;
using System.Collections;

public class ClickRaycast : MonoBehaviourPun
{
    public Camera playerCam;
    public GameObject panel;
    private Transform currentEstanteria;
    public float distanciaMax = 5f;
    // distancia maxima para mantener el panel abierto

    public Animator playerAnimator;
    private bool sentado = false;

    void Start()
    {
        if (!photonView.IsMine)
            return;

           //Buscar panel en el canvas
            Canvas canvas = Object.FindFirstObjectByType<Canvas>();

            if (canvas != null)
            {
                Transform panelTransform = canvas.transform.Find("PanelJuegos");
                if (panelTransform != null)
                    panel = panelTransform.gameObject;
            }


            if (panel == null)
            {
                Debug.LogError("ClickRaycast: No se encontró el panel de juegos!");
            }
            else
            {
                //Lo oculta al inicio
                panel.SetActive(false);
            }

            if (playerCam == null)
                playerCam = GetComponentInChildren<Camera>(); // busca la cámara en los hijos

            if (playerCam == null)
                Debug.LogError("ClickRaycast: No se encontró la cámara del jugador local!");

            playerAnimator = GetComponentInChildren<Animator>();

            if (playerAnimator == null)
                Debug.LogError("ClickRaycast: No se encontró el Animator del jugador local");

        //else
        //{
            //Desactiva el script para jugadores remotos
        //    if (playerCam != null)
          //      playerCam.gameObject.SetActive(false);

           // this.enabled = false;
     //   }
    }

    void Update()
    {
        //Solo el jugador local puede usar el raycast
        if (!photonView.IsMine) return;

        if (Mouse.current != null && Mouse.current.leftButton.wasPressedThisFrame)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();

            Ray ray = playerCam.ScreenPointToRay(mousePos);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                if (hit.collider.CompareTag("Estanteria"))
                {
                    //currentEstanteria = hit.collider.transform;

                    if (panel != null)
                    {
                        panel.SetActive(true);

                        //Desbloquear cursor
                        Cursor.lockState = CursorLockMode.None;
                        Cursor.visible = true;
                    }
                    else
                    {
                        Debug.LogWarning("Panel no asignado!");
                    }

                    currentEstanteria = hit.collider.transform;

                }
                if (hit.collider.CompareTag("Sillon"))
                {
                    if (playerAnimator != null && !sentado)
                    {
                        //playerAnimator.SetTrigger("Sit");
                        StartCoroutine(Sentarse());

                       
                    }
                }
            }
        }

        //Si hay una estanteria activa, comprobar la distancia
        if (currentEstanteria != null && panel != null && panel.activeSelf)
        {
            float dist = Vector3.Distance(playerCam.transform.position, currentEstanteria.position);

            if (dist > distanciaMax)
            {
                panel.SetActive(false);
                currentEstanteria = null; //resetea la estanteria activa

                //Bloquear cursor
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
            }
        }
    }

    IEnumerator Sentarse()
    {
        sentado = true;
        yield return null; // Espera un frame para que se procese el trigger
        playerAnimator.SetTrigger("Sit");
    }
    public void CerrarPanel()
    {
        if (panel != null)
        {
            panel.SetActive(false);
            currentEstanteria = null;
            Debug.Log("Panel cerrado desde boton");

            //Bloquear cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
    }
}