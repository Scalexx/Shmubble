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
    [Tooltip ("Layer to check when the player is grounded.")]
    public LayerMask groundLayer;

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

    [Header("Invulnerability")]
    [Tooltip("How long you stay invulnerable to damage.")]
    public float invulnerabilityPeriod;
    float invulnerabilityTimer = 0;
    int correctLayer;
    public Renderer playerRenderer;
    public float flashPeriod;
    private float flashTimer;

    [Header("Knock Back")]
    public float knockBackForce;
    public float knockBackPeriod;
    private float knockBackTimer;

    private Vector3 moveVector;
    private Vector3 lastMotion;
    private CharacterController controller;

	void Start () {
        controller = GetComponent<CharacterController>();
        currentDashTime = maxDashTime;
        correctLayer = gameObject.layer;
    }
	
	void Update () {
        if (knockBackTimer <= 0)
        {
            moveVector = Vector3.zero;
            inputDirection = Input.GetAxisRaw("Horizontal") * speed;

            HandleDash();

            if (IsControllerGrounded())
            {
                verticalVelocity = 0;
                secondJump = true;

                if (Input.GetButtonDown("Jump"))
                {
                    verticalVelocity = jumpHeight;
                }
            }
            else
            {
                if (Input.GetButtonDown("Jump"))
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
        }
        else
        {
            knockBackTimer -= Time.deltaTime;
        }

        controller.Move(moveVector * Time.deltaTime);

        if (inputDirection != 0)
        {
            lastMotion.x = moveVector.x / speed;
        }

        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= Time.deltaTime;

            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0)
            {
                playerRenderer.enabled = !playerRenderer.enabled;
                flashTimer = flashPeriod;
            }
            if (invulnerabilityTimer <= 0)
            {
                playerRenderer.enabled = true;
            }
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

        if(Physics.Raycast(leftRayStart, Vector3.down, (controller.height / 2) + 0.1f, groundLayer))
        { 
            return true;
        }

        if (Physics.Raycast(rightRayStart, Vector3.down, (controller.height / 2) + 0.1f, groundLayer))
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
        if (hit.gameObject.CompareTag("OutOfBounds"))
        {
            LevelManager.Instance.OutOfBounds();
        }
        if (hit.gameObject.layer == 9)
        {
            if (invulnerabilityTimer <= 0)
            {
                Vector2 hitDirection = hit.transform.position - transform.position;
                hitDirection =- hitDirection.normalized;
                Knockback(hitDirection);

                LevelManager.Instance.GetDamaged();
                invulnerabilityTimer = invulnerabilityPeriod;

                playerRenderer.enabled = false;
                flashTimer = flashPeriod;
            }
        }
    }

    public void Knockback(Vector2 knockBackDirection)
    {
        knockBackTimer = knockBackPeriod;
        if (knockBackDirection.x > 0)
        {
            knockBackDirection.x = 1;
        }
        else if (knockBackDirection.x <= 0)
        {
            knockBackDirection.x = -1;
        }
        moveVector.x = knockBackDirection.x * knockBackForce;
        moveVector.y = knockBackForce;
    }
}
