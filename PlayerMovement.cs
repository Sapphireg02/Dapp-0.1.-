using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using Photon.Pun;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviourPun
{
    public float moveSpeed = 5f;
    private CharacterController controller;

    public Transform CameraHolder;
    public Animator animator;

    public float jumpHeight = 0.5f;
    public float gravity = -60f;
    private float verticalVelocity = -2f; //velocidad vertical

    private bool sitState = false;

    // INPUT BUFFERING
    public float jumpBufferTime = 0.20f; // Tiempo maximo de guardado
    private float lastJumpPressedTime = -999f;

    // COYOTE TIME
    public float coyoteTime = 0.15f; // Tiempo maximo despues de salir del suelo
    private float lastGroundedTime = -999f;

    // Multi Salto
    public float jumpCooldown = 0.1f; // Tiempo entre saltos
    private float lastJumpTime = -999f;

    //Ref a MouseLook
    public Transform playerBody;

    //Menu de pausa
    public GameObject pauseMenu;
    private bool isPaused = false;

    //Modo foto
    public PhotoModeCamera photoModeScript;

    void Start()
    {
        sitState = false;
        animator.SetBool("Sit", sitState);
        controller = GetComponent<CharacterController>();
        if (controller == null)
        {
            Debug.LogWarning("Añade un controlador");
        }

        if (photonView.IsMine)
        {
            //Buscar panel de pausa
            pauseMenu = GameObject.Find("PauseMenu");

            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false);
            }
            else
            {
                Debug.LogWarning("No se encontró el menú de pausa");
            }
        }

        // Solo activa la cámara si es el jugador local
        if (!photonView.IsMine)
        {
            CameraHolder.gameObject.SetActive(false);

            //Busca la cámara en cameraholder o sus hijos
            Camera cam = CameraHolder.GetComponentInChildren<Camera>();
            if (cam != null)
                cam.enabled = false;

            //Busca el AudioListener en cameraholder o sus hijos
            AudioListener audio = CameraHolder.GetComponentInChildren<AudioListener>();
            if (audio != null)
                audio.enabled = false;
        }
    }

    void Update()
    {
        if (!photonView.IsMine) return; // Solo el jugador local controla su movimiento

        //Detecta ESC
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            TogglePause();
        }

        //Si está pausado no procesa movimiento
        if(isPaused) return;

        Vector3 move = Vector3.zero;

        if (Keyboard.current != null)
        {
            //Movimiento con la orientación de la cámara
            Vector3 forward = CameraHolder.forward;
            Vector3 right = CameraHolder.right;

            //Evita que afecte la inclinación vertical de la cámara
            forward.y = 0;
            right.y = 0;

            forward.Normalize();
            right.Normalize();

            //Movimiento relativo a la orientación del personaje
            if (Keyboard.current.wKey.isPressed) move += forward;
            if (Keyboard.current.sKey.isPressed) move -= forward;
            if (Keyboard.current.aKey.isPressed) move -= right;
            if (Keyboard.current.dKey.isPressed) move += right;

            //Saltar
            if (Keyboard.current.spaceKey.isPressed)
            {
                lastJumpPressedTime = Time.time; // Guarda el tiempo del salto

                //Si está sentado
                if (sitState)
                {
                    sitState = false;
                    animator.SetBool("Sit", sitState);
                }
            }

            //Si está sentado que no se mueva
            if (sitState)
            {
                move = Vector3.zero;
            }

            Vector3 velocity = move.normalized * moveSpeed;
            velocity.y = verticalVelocity;

            controller.Move(velocity * Time.deltaTime);

            //Caminar e idle
            if (animator != null)
            {
                animator.SetFloat("Speed", move.magnitude);

                //sentarse
                if (Keyboard.current.cKey.wasPressedThisFrame)
                {
                    sitState = !sitState; //alterna sentado y de pie
                    animator.SetBool("Sit", sitState);
                }
            }

            if (controller.isGrounded && verticalVelocity < 0)
            {
                verticalVelocity = -2f; //Lo mantiene pegado al suelo
            }

            if (controller.isGrounded)
            {
                lastGroundedTime = Time.time; // Actualiza el tiempo en que estuvo en el suelo
            }

            if ((Time.time - lastJumpPressedTime <= jumpBufferTime) && (controller.isGrounded || (Time.time - lastGroundedTime <= coyoteTime)))
            {
                verticalVelocity = Mathf.Sqrt(Mathf.Max(jumpHeight, 0.01f) * -2f * gravity);

                //Limita la velocidad vertical
                verticalVelocity = Mathf.Min(verticalVelocity, 4f);
                lastJumpTime = Time.time; // Actualiza el tiempo del último salto
                lastJumpPressedTime = -999f; // Resetea el tiempo del salto
            }

            verticalVelocity += gravity * Time.deltaTime; //Aplica gravedad
        }
    }

    //Activar/Desactivar pausa
    void TogglePause()
    {
        isPaused = !isPaused;
        if (pauseMenu != null)
        {
            pauseMenu.SetActive(isPaused);
        }

        //Detener el tiempo de juego
        Time.timeScale = isPaused ? 0f : 1f;

        //Mostrar/ocultar cursor
        Cursor.visible = isPaused;
        Cursor.lockState = isPaused ? CursorLockMode.None : CursorLockMode.Locked;
    }

    public void TogglePhotoModeFromButton()
    {
        if (photoModeScript == null)
            photoModeScript = GetComponent<PhotoModeCamera>();

        if (photoModeScript != null)
            photoModeScript.TogglePhotoMode();
    }
}