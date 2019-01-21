using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player2 : MonoBehaviour {

    private float inputDirection;               //x value of moveVector
    private float verticalVelocity;             //y value of moveVector

    [Header ("Basic movement")]
    [Tooltip ("Speed of the character.")]
    public float speed = 5.0f;
    [Tooltip ("How high the character can jump.")]
    public float jumpHeight;
    [Tooltip ("How fast the character falls back down.")]
    public float gravity = 25.0f;
    private bool secondJump = false;

    [Header("Dash")]
    [Tooltip ("Dash distance. Relates to the amount of time it stays in the dash.")]
    public float maxDashTime = 1.0f;
    [Tooltip ("How fast the dash is.")]
    public float dashSpeed = 1.0f;
    [Tooltip ("How fast the dash stops. The dashStoppingSpeed gets added to the currentDashTime to eventually reach the maxDashTime.")]
    public float dashStoppingSpeed = 0.1f;
    private float currentDashTime;
    private Vector3 moveDirection;
    private float dir;

    private Vector3 moveVector;
    private Vector3 lastMotion;
    private CharacterController controller;

	void Start () {
        controller = GetComponent<CharacterController>();
        currentDashTime = maxDashTime;
    }
	
	void Update () {
        moveVector = Vector3.zero;
        inputDirection = Input.GetAxisRaw("Horizontal") * speed;

        HandleDash();

        if (IsControllerGrounded())
        {
            verticalVelocity = 0;
            secondJump = true;

            if (Input.GetKeyDown(KeyCode.Space))
            {
                verticalVelocity = jumpHeight;
            }
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (secondJump)
                {
                    verticalVelocity = jumpHeight;
                    secondJump = false;
                }
            }

            verticalVelocity -= gravity * Time.deltaTime;
        }

        moveVector.x = inputDirection;
        moveVector.y = verticalVelocity;

        controller.Move(moveVector * Time.deltaTime);
        if (inputDirection != 0)
        {
            lastMotion.x = moveVector.x / speed;
        }
	}

    private bool IsControllerGrounded()
    {
        Vector3 leftRayStart;
        Vector3 rightRayStart;

        leftRayStart = controller.bounds.center;
        rightRayStart = controller.bounds.center;

        leftRayStart.x -= controller.bounds.extents.x;
        rightRayStart.x += controller.bounds.extents.x;

        if(Physics.Raycast(leftRayStart, Vector3.down, (controller.height / 2) + 0.1f))
        { 
            return true;
        }

        if (Physics.Raycast(rightRayStart, Vector3.down, (controller.height / 2) + 0.1f))
        {
            return true;
        }

        return false;
    }

    private void HandleDash ()
    {
        if (Input.GetButtonDown("Dash"))
        {
            dir = dashSpeed * lastMotion.x;
            currentDashTime = 0.0f;
            verticalVelocity = 0;
        }
        if (currentDashTime < maxDashTime)
        {
            moveDirection = new Vector3(dir, 0, 0);
            currentDashTime += dashStoppingSpeed;
        }
        else
        {
            moveDirection = Vector3.zero;
        }
        controller.Move(moveDirection * Time.deltaTime);
    }

    void OnTriggerEnter (Collider hit)
    {
        if (hit.gameObject.tag == "OutOfBounds")
        {
            LevelManager.Instance.OutOfBounds();
        }
        if (hit.gameObject.tag == "ProjectileBoss")
        {
            LevelManager.Instance.GetDamaged();
        }
    }
}
