using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCam : MonoBehaviour
{
  public float mouseSensitivity = 100f;
    public Transform playerBody;

    float xRotation = 0f;

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplica rotação vertical à câmera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Aplica rotação horizontal ao corpo do jogador
        playerBody.Rotate(Vector3.up * mouseX);

        Debug.Log($"MouseX: {mouseX}, MouseY: {mouseY}, Player Rotation: {playerBody.rotation.eulerAngles}");
    }

}
