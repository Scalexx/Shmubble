using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

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
    private bool crouch;

    public float bossTouchDamage;

    [Header("Shooting")]
    [Tooltip("The GameObject the bullet spawns from.")]
    public Transform spawnPoint;
    float timeBetweenShots;
    int projectileChoice;
    [Tooltip("The GameObject with the Object Pool on it.")]
    public ObjectPooler projectilePool;

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
    [Tooltip("The mesh renderer of the player. The renderer has to be assigned in the inspector.")]
    public Renderer playerRenderer;
    [Tooltip("How fast the player blinks when hit.")]
    public float flashPeriod;
    private float flashTimer;

    [Header("Knock Back")]
    [Tooltip("The force which is applied when the player is hit.")]
    public float knockBackForce;
    [Tooltip("The amount of time the player will have the force applied and can't move.")]
    public float knockBackPeriod;
    private float knockBackTimer;

    private Vector3 moveVector;
    private Vector3 lastMotion;
    private CharacterController controller;

	void Start () {
        controller = GetComponent<CharacterController>();
        currentDashTime = maxDashTime;
    }
	
	void FixedUpdate () {
        if (knockBackTimer <= 0)
        {
            inputDirection = Input.GetAxis("Horizontal") * speed;

            if (IsControllerGrounded() && Input.GetButton("Lock Movement"))
            {
                moveVector.x = 0;
                moveVector.y -= gravity * Time.deltaTime;
                crouch = false;
            }
            else if (IsControllerGrounded() && Input.GetAxisRaw("Vertical") < 0)
            {
                moveVector.x = 0;
                moveVector.y -= gravity * Time.deltaTime;
                crouch = true;
            }
            else
            {
                crouch = false;

                moveVector = Vector3.zero;

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
        }
        else
        {
            knockBackTimer -= Time.deltaTime;
        }

        controller.Move(moveVector * Time.deltaTime);

        if (inputDirection != 0)
        {
            float tempInput = inputDirection / speed;
            if (tempInput > 0)
            {
                lastMotion.x = 1;
            }
            else
            {
                lastMotion.x = -1;
            }
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
        if (Input.GetButton("Shoot") || Input.GetAxis("Shoot") > 0)
        {
            HandleShoot();
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

    private void HandleShoot()
    {
        HandleDirection();


        if(timeBetweenShots <= 0)
        {
            GameObject newProjectile = projectilePool.GetPooledObject();

            newProjectile.transform.position = spawnPoint.position;
            newProjectile.transform.rotation = spawnPoint.rotation;
            newProjectile.SetActive(true);

            timeBetweenShots = newProjectile.GetComponent<BulletData>().timeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    // Delete this function later when Animations are added as it will serve the same purpose
    void HandleDirection ()
    {
        int zRotation;
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxisRaw("Vertical");
        if (xInput > 0)
        {
            if (yInput > 0)
            {
                zRotation = 45;
            }
            else if (yInput < 0)
            {
                if (Input.GetButton("Lock Movement") || !IsControllerGrounded())
                {
                    zRotation = -45;
                }
                else
                {
                    zRotation = 0;
                }
            }
            else
            {
                zRotation = 0;
            }
        }
        else if (xInput < 0)
        {
            if (yInput > 0)
            {
                zRotation = 135;
            }
            else if (yInput < 0)
            {
                if (Input.GetButton("Lock Movement") || !IsControllerGrounded())
                {
                    zRotation = -135;
                }
                else
                {
                    zRotation = 180;
                }
            }
            else
            {
                zRotation = 180;
            }
        }
        else
        {
            if (yInput > 0)
            {
                zRotation = 90;
            }
            else if (yInput < 0)
            {
                if (Input.GetButton("Lock Movement") || !IsControllerGrounded())
                {
                    zRotation = -90;
                }
                else
                {
                    if (lastMotion.x >= 0)
                    {
                        zRotation = 0;
                    }
                    else
                    {
                        zRotation = 180;
                    }
                }
            }
            else
            {
                if (lastMotion.x >= 0)
                {
                    zRotation = 0;
                }
                else
                {
                    zRotation = 180;
                }
            }
        }

        spawnPoint.eulerAngles = new Vector3(0, 0, zRotation);
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
                hitDirection = -hitDirection.normalized;
                Knockback(hitDirection);

                BulletData bulletDamage = hit.gameObject.GetComponent<BulletData>();
                float damaged;
                if (bulletDamage != null)
                {
                    damaged = bulletDamage.damage;
                }
                else
                {
                    damaged = bossTouchDamage;
                }
                LevelManager.Instance.GetDamaged(damaged);
                Invulnerable();
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

    public void Invulnerable ()
    {
        invulnerabilityTimer = invulnerabilityPeriod;

        playerRenderer.enabled = false;
        flashTimer = flashPeriod;
    }
}

// no bugs plz
