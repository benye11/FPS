using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //public Camera camera;
    public GameObject gun;
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    private float distance;
    private bool disabled = false;
    // Start is called before the first frame update
    void Start()
    {
        disabled = false;
        Cursor.lockState = CursorLockMode.Locked;
        //camera.transform.localRotation = Quaternion.Euler(xRotation, 0f , 0f);
        //gun.transform.localRotation = Quaternion.Euler(xRotation, 0f , 0f);
        //distance = Vector3.Distance(gun.transform.position, transform.position);
    }

    // Update is called once per frame
    //for example, 30 fps = 30 frames per second means 30 updates per second.
    void Update()
    {
        if (!disabled) {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        transform.localRotation = Quaternion.Euler(xRotation, 0f , 0f);
        //gun.transform.localRotation = Quaternion.Euler(xRotation, 0f , 0f);
        //gun.transform.localPosition = Quaternion.Euler(xRotation, 0f , 0f) * transform.localPosition * distance;
        //gun.transform.RotateAround(transform.localPosition, Vector3.up * xRotation, 1f);
        playerBody.Rotate(Vector3.up * mouseX);
        }
    }

    public void DisableMovement() {
        disabled = true;
    }
}
