using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float jumpHeight = 1f;
    private bool isGrounded;
    public float groundDistance = 0.4f;
    public float gravity = -9.8f; //-5f;
    public float speed = 0.5f;
    public CharacterController controller;
    public Transform groundCheck;
    public LayerMask groundMask;
    private Vector3 velocity;
    private PlayerBehavior disabled;

    void Start() {
        disabled = GetComponent<PlayerBehavior>();
    }
    // Update is called once per frame
    void Update()
    {
        if (!disabled.Killed) {
        //this creates a sphere of radius groundDistance. if it collides with anything in groundMask, it will return true.
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0) {
            velocity.y = -2f;
        }
        float horizontal = Input.GetAxis("Horizontal"); //1 is right, -1 is left
        float vertical = Input.GetAxis("Vertical"); //1 is forward, -1 is backwards 
        Vector3 move = transform.right * horizontal + transform.forward * vertical;
        controller.Move(move * speed * Time.deltaTime);
        if (isGrounded && Input.GetButtonDown("Jump")) {
            velocity.y = Mathf.Sqrt(jumpHeight * -1f * gravity);
        }

        velocity.y += (gravity * Time.deltaTime*2f);
        //Debug.Log("velocity of y: " + velocity.y.ToString("R"));
        controller.Move(velocity * Time.deltaTime);
        }
    }
}
