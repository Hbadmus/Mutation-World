using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    public float speed = 10f;
    public float jumpHeight = 10f;
    public float gravity = 9.81f;
    public float airControl = 10f;
    public float sensitivity = 0.5f;
    private float speedMultiplier = 1f;

    CharacterController controller;
    Vector3 input, moveDirection;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        if(gameObject.name.Contains("Fiona")) {
            speedMultiplier = 1.3f; // 30% speed increase
        }
    }

    void Update()
    {
        if(SceneManager.GetActiveScene().buildIndex != 0) {
            float moveHorizontal = Input.GetAxis("Horizontal") * sensitivity;
            float moveVertical = Input.GetAxis("Vertical") * sensitivity;
            input = (transform.right * moveHorizontal + transform.forward * moveVertical).normalized;
            input *= speed * speedMultiplier;

            if (controller.isGrounded)
            {
                moveDirection = input;
                if (Input.GetButton("Jump"))
                {
                    moveDirection.y = Mathf.Sqrt(2 * jumpHeight * gravity);
                }
                else
                {
                    moveDirection.y = 0.0f;
                }
            }
            else
            {
                input.y = moveDirection.y;
                moveDirection = Vector3.Lerp(moveDirection, input, Time.deltaTime * airControl);
            }
            moveDirection.y -= gravity * Time.deltaTime;

            controller.Move(moveDirection * Time.deltaTime);
        }
    }
}