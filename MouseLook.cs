using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class MouseLook : MonoBehaviourPun, IPunObservable
{
    public Transform playerBody;
    public Transform headBone;
    public Transform cameraHolder;
    public float mouseSensitivity = 1f;
    public float rotationLerpSpeed = 10f;
    private float xRotation = 0f;
    private Vector2 mouseDelta;
    private float networkXRotation;
    //Gira el cuerpo
    private float networkYRotation; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked; 
        //Oculta y bloquea el cursor
    }

    void Update()
    {
        if (!photonView.IsMine)
        {
            //Actualiza la rotación de la cabeza
            Quaternion targetHeadRot = Quaternion.Euler(networkXRotation, 0f, 0f);
            headBone.localRotation = Quaternion.Slerp(headBone.localRotation, targetHeadRot, Time.deltaTime * rotationLerpSpeed);

            //Actualiza la rotación del cuerpo
            Vector3 bodyEuler = playerBody.eulerAngles;
            bodyEuler.y = Mathf.LerpAngle(bodyEuler.y, networkYRotation, 0.2f);
            playerBody.eulerAngles = bodyEuler;

            return;
        }

        if (Mouse.current != null)
            {
                mouseDelta = Mouse.current.delta.ReadValue() * mouseSensitivity * Time.deltaTime;

                // Rotacion de la cabeza
                xRotation -= mouseDelta.y;
                xRotation = Mathf.Clamp(xRotation, -90f, 90f);
                cameraHolder.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
                headBone.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

                // Rotacion del cuerpo
                playerBody.Rotate(Vector3.up * mouseDelta.x, Space.World);

            }
        

    }
    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            // Enviando datos al resto de jugadores
            stream.SendNext(xRotation);
            stream.SendNext(playerBody.eulerAngles.y);
        }
        else
        {
            // Recibiendo datos de otros jugadores
            networkXRotation = (float)stream.ReceiveNext();
            networkYRotation = (float)stream.ReceiveNext();
        }
    }
}
