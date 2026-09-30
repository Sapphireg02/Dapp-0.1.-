using UnityEngine;
using Photon.Pun;

public class PhotoModeCamera : MonoBehaviourPun
{
    [Header("References")]
    public Transform playerBody; //Jugador
    public Transform cameraTransform; //Camara

    [Header("Settings")]
    public Vector3 thirdPersonOffset = new Vector3(0, 2, -4);
    public float rotationSpeed = 70f;

    private bool isThirdPerson = false;

    private Vector3 originalLocalPos;
    private Quaternion originalLocalRot;
    private Transform originalParent;

    void Start()
    {
        if (!photonView.IsMine)
        {
            cameraTransform.gameObject.SetActive(false);
            enabled = false;
            return;
        }

        //Guarda la poosición original
        originalParent = cameraTransform.parent;
        originalLocalPos = cameraTransform.localPosition;
        originalLocalRot = cameraTransform.localRotation;
    }

    void Update()
    {
        if (!photonView.IsMine || !isThirdPerson)
            return;

        //Rotacion libre de la camara
        float mouseX = Input.GetAxis("Mouse X") * rotationSpeed * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * rotationSpeed * Time.deltaTime;

        cameraTransform.RotateAround(playerBody.position, Vector3.up, mouseX);
        cameraTransform.RotateAround(playerBody.position, cameraTransform.right, -mouseY);
    }

    public void TogglePhotoMode()
    {
        if (!photonView.IsMine)
            return;

        isThirdPerson = !isThirdPerson;
        if (isThirdPerson)
        {
            //Mueve la cámara detras del jugador
            cameraTransform.SetParent(null);
            cameraTransform.position = playerBody.position + playerBody.TransformDirection(thirdPersonOffset);
            cameraTransform.LookAt(playerBody);
        }
        else
        {
            //Vuelve a la posición original
            cameraTransform.SetParent(originalParent);
            cameraTransform.localPosition = originalLocalPos;
            cameraTransform.localRotation = originalLocalRot;
        }
    }
}
