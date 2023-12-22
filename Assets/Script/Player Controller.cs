using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private CharacterController characterController;
    [SerializeField] private float playerSpeed = 5f;
    [SerializeField] private Camera followCamera;

    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 playerVelocity;
    [SerializeField] private float gravityValue = -10f;

    public bool groundedPlayer;
    [SerializeField] private float jumpHeight = 2.5f;

    public Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        characterController = GetComponent<CharacterController>();
        animator = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
    }

    void Movement()
    {
        groundedPlayer = characterController.isGrounded;
        if (characterController.isGrounded && playerVelocity.y < -2f) 
        {
            playerVelocity.y = -1f;
        }

        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        Vector3 movementInput = Quaternion.Euler(0, followCamera.transform.eulerAngles.y, 0) * new Vector3(horizontalInput, 0, verticalInput);

        Vector3 movementDirection = movementInput.normalized;

        characterController.Move(movementDirection * playerSpeed * Time.deltaTime);

        if(movementDirection != Vector3.zero)  

        {
            Quaternion desireRotation = Quaternion.LookRotation(movementDirection,Vector3.up);
            transform.rotation = Quaternion.Slerp(transform.rotation, desireRotation, rotationSpeed * Time.deltaTime);
        }

        if(Input.GetButtonDown("Jump") && groundedPlayer) 
        {
            playerVelocity.y += Mathf.Sqrt(jumpHeight * -2f * gravityValue);
        }

        playerVelocity.y += gravityValue * Time.deltaTime;
        characterController.Move(playerVelocity * Time.deltaTime);

        animator.SetFloat("Speed",Mathf.Abs(movementDirection.x) + Mathf.Abs(movementDirection.z));
        animator.SetBool("Ground", characterController.isGrounded);

        if (Input.GetButtonDown("Jump") && groundedPlayer)
        {

            playerVelocity.y += Mathf.Sqrt(jumpHeight * -1.0f * gravityValue);
            animator.SetTrigger("Jumping");
        }

    }
}
