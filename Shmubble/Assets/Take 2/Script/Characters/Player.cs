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
    float startGravity;
    [Tooltip ("Layer to check when the player is grounded.")]
    public LayerMask groundLayer;
    private bool crouch;
	private bool jumping;

    [Tooltip("Damage the player takes when touching the boss.")]
    public float bossTouchDamage;

    [Header("Shooting")]
    [Tooltip("The GameObject the bullet spawns from.")]
    public Transform spawnPoint;
    public Transform spawnPointEX;
    float timeBetweenShots;
    int projectileChoice;
    [Tooltip("The GameObject with the Projectile Object Pool on it.")]
    public ObjectPooler projectilePool;
    [Tooltip("The GameObject with the EX Projectile Object Pool on it.")]
    public ObjectPooler exProjectilePool;
    public string attackSound;
    public string exAttackSound;
    private bool shooting;
    private bool EXshoot;

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
	private bool dashing;

    [Header("Invulnerability")]
    [Tooltip("How long you stay invulnerable to damage.")]
    public float invulnerabilityPeriod;
    float invulnerabilityTimer = 0;
    [Tooltip("The mesh renderers of the player. The renderers has to be assigned in the inspector.")]
    public List<Renderer> playerRenderers = new List<Renderer>();
    [Tooltip("How fast the player blinks when hit.")]
    public float flashPeriod;
    private float flashTimer;

    [Header("Knock Back")]
    [Tooltip("The force which is applied when the player is hit.")]
    public float knockBackForce;
    [Tooltip("The amount of time the player will have the force applied and can't move.")]
    public float knockBackPeriod;
    private float knockBackTimer;

    public Animator animator;

    public float downTime;
    public float waitTillShoot;

    public GameObject muzzleEffect;
    GameObject muzzleEffectActive;

    public GameObject effect;
    GameObject effectActive;

    float waitShoot;
    float downPeriod;

    [HideInInspector]
    public bool takenOver;
    [HideInInspector]
    public bool lockMovement;
    [HideInInspector]
    public bool allowDisable;
    [HideInInspector]
    public bool disableShoot;
    [HideInInspector]
    public bool disableJump;
    [HideInInspector]
    public bool disableDash;

    bool done;
    bool antDone;
    bool flasher;

    private Vector3 moveVector;
    private Vector3 lastMotion;
    private CharacterController controller;

	void Start () {
        controller = GetComponent<CharacterController>();
        currentDashTime = maxDashTime;
        startGravity = gravity;
    }

	void Update() {
        if (disableJump)
        {
            animator.SetBool("Jumping", false);
            jumping = false;
        }
        else
        {
            jumping = Input.GetButton("Jump");
        }

        if (disableDash)
        {
            animator.SetBool("Dashing", false);
            dashing = false;
        }
        else if (Input.GetButtonDown("Dash"))
        {
		    dashing = true;
	    }

        animator.SetBool("Jumping", jumping);
        animator.SetBool("Dashing", dashing);

        if (Input.GetButton("EX Shoot") && LevelManager.Instance.damageDealt >= LevelManager.Instance.specialMaxCharge)
        {
            waitShoot = waitTillShoot;
            downPeriod = downTime;
            EXshoot = true;
        }

        if (EXshoot)
        {
            if (downPeriod <= 0)
            {
                antDone = false;
                allowDisable = false;
                disableShoot = false;
                gravity = startGravity;
                animator.SetBool("Special", false);
                EXshoot = false;
            }
            else
            {
                Invulnerable();
                if (!antDone)
                {
                    effectActive = Instantiate(effect, spawnPointEX.position, spawnPointEX.rotation);
                    Destroy(effectActive, 3f);
                    effectActive = null;
                    antDone = true;
                }                

                gravity = 0;
                verticalVelocity = 0;
                animator.SetBool("Special", true);
                downPeriod -= Time.deltaTime;
                allowDisable = true;
                disableShoot = true;
            }

            if (waitShoot <= 0)
            { 
                if (!done)
                {
                    HandleExShoot();
                    done = true;
                }
            }
            else
            {
                waitShoot -= Time.deltaTime;
            }
        }
        else
        {
            done = false;
        }

        if (disableShoot)
        {
            animator.SetBool("Attacking", false);
            shooting = false;
        }
        else if (Input.GetButton("Shoot") || Input.GetAxis("Shoot") > 0)
        {
            animator.SetBool("Attacking", true);
            shooting = true;
        }
        else
        {
            timeBetweenShots = 0;
            animator.SetBool("Attacking", false);
            shooting = false;
        }

        if (shooting)
        {
            HandleShoot();
        }

        if (allowDisable)
        {
            lockMovement = true;
        }
        else if (Input.GetButton("Lock Movement"))
        {
            lockMovement = true;
        }
        else
        {
            lockMovement = false;
        }

        if (lockMovement)
        {
            animator.SetBool("Jumping", false);
            animator.SetBool("Dashing", false);
            animator.SetFloat("InputX", 0f);
            animator.SetFloat("InputY", 0f);
            animator.SetBool("Falling", false);
        }
        else if (crouch)
        {
            animator.SetFloat("InputX", 0f);
        }
        else
        {
            animator.SetFloat("InputX", Input.GetAxis("Horizontal"));
            animator.SetFloat("InputY", Input.GetAxis("Vertical"));
        }
    }
	
	void FixedUpdate () {
        if (knockBackTimer <= 0)
        {
            animator.SetBool("Hit", false);
            inputDirection = Input.GetAxis("Horizontal") * speed;

            if (IsControllerGrounded() && lockMovement)
            {
                moveVector.x = 0;
                moveVector.y -= gravity * Time.deltaTime;
                crouch = false;
            }
            else if (IsControllerGrounded() && Input.GetAxis("Vertical") < -0.5)
            {
                moveVector.x = 0;
                moveVector.y -= gravity * Time.deltaTime;
                crouch = true;
            }
            else if (EXshoot)
            {
                moveVector.x = 0;
                moveVector.y = 0;
            }
            else
            {
                crouch = false;

                moveVector = Vector3.zero;

                HandleDash();

                if (IsControllerGrounded())
                {
                    animator.SetBool("Falling", false);
                    verticalVelocity = 0;

                    if (jumping)
                    {
                        verticalVelocity = jumpHeight;
                    }
                }
                else
                {
                    verticalVelocity -= gravity * Time.deltaTime;

                    if (verticalVelocity < -2)
                    {
                        animator.SetBool("Falling", true);
                    }
                }
                if (takenOver)
                {
                    moveVector.x = inputDirection * GetComponentInParent<BulletData>().velocity.x;
                }
                else
                {
                    moveVector.x = inputDirection;
                }
                
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
                if (allowDisable)
                {
                    gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
                }
                else
                {
                    gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 0, gameObject.transform.rotation.z);
                }
            }
            else
            {
                lastMotion.x = -1;
                if (allowDisable)
                {
                    gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, gameObject.transform.rotation.y, gameObject.transform.rotation.z);
                }
                else
                {
                    gameObject.transform.rotation = Quaternion.Euler(gameObject.transform.rotation.x, 180, gameObject.transform.rotation.z);
                }
            }
        }

        if (invulnerabilityTimer > 0)
        {
            invulnerabilityTimer -= Time.deltaTime;

            flashTimer -= Time.deltaTime;
            if (flashTimer <= 0 && flasher)
            {
                for (int i = 0; i < playerRenderers.Count; i++)
                {
                    playerRenderers[i].enabled = !playerRenderers[i].enabled;
                    flashTimer = flashPeriod;
                }
            }
            if (invulnerabilityTimer <= 0 && flasher)
            {
                for (int i = 0; i < playerRenderers.Count; i++)
                {
                    playerRenderers[i].enabled = !playerRenderers[i].enabled;
                }
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

        if (Physics.Raycast(leftRayStart, Vector3.down, (controller.height / 2) + 0.1f, groundLayer, QueryTriggerInteraction.Ignore))
        { 
            return true;
        }

        if (Physics.Raycast(rightRayStart, Vector3.down, (controller.height / 2) + 0.1f, groundLayer, QueryTriggerInteraction.Ignore))
        {
            return true;
        }

        return false;
    }

    private void HandleDash ()
    {
        if (dashing)
        {
            dir = dashSpeed * lastMotion.x;
            currentDashTime = 0.0f;
            verticalVelocity = 0;
		    dashing = false;
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

            muzzleEffectActive = Instantiate(muzzleEffect, spawnPoint.position, spawnPoint.rotation);
            muzzleEffectActive.transform.parent = spawnPoint.transform;
            Destroy(muzzleEffectActive, 0.3f);

            muzzleEffectActive = null;

            //add sounds
            newProjectile.SetActive(true);
            AudioManager.instance.PlayPlayerSound(attackSound);

            timeBetweenShots = newProjectile.GetComponent<BulletData>().timeBetweenShots;
        }
        else
        {
            timeBetweenShots -= Time.deltaTime;
        }
    }

    private void HandleExShoot()
    {
        AudioManager.instance.PlayPlayerSound(exAttackSound);
        HandleDirection();

        GameObject newProjectile = exProjectilePool.GetPooledObject();

        newProjectile.transform.position = spawnPointEX.position;
        newProjectile.transform.rotation = spawnPointEX.rotation;

        //add sounds
        newProjectile.SetActive(true);

        float damage = newProjectile.GetComponent<BulletData>().damage;

        LevelManager.Instance.SpecialDone(damage);
    }

    // Delete this function later when Animations are added as it will serve the same purpose
    void HandleDirection ()
    {
        int zRotation;
        float xInput = Input.GetAxisRaw("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        if (xInput > 0)
        {
            if (yInput > 0.5)
            {
                zRotation = 45;
            }
            else
            {
                zRotation = 0;
            }
        }
        else if (xInput < 0)
        {
            if (yInput > 0.5)
            {
                zRotation = 135;
            }
            else
            {
                zRotation = 180;
            }
        }
        else
        {
            if (yInput > 0.5)
            {
                zRotation = 90;
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

        animator.SetFloat("InputShootX", xInput);
        animator.SetFloat("InputShootY", yInput);

        spawnPoint.eulerAngles = new Vector3(0, 0, zRotation);
        spawnPointEX.eulerAngles = new Vector3(0, 0, zRotation);
    }

    void OnTriggerEnter (Collider hit)
    {
        if (hit.gameObject.CompareTag("OutOfBounds"))
        {
            LevelManager.Instance.OutOfBounds();
        }
        if (hit.gameObject.layer == 9)
        {
            GetDamaged(hit.transform);
        }
    }

    public void GetDamaged(Transform hit)
    {
        if (invulnerabilityTimer <= 0)
        {
            Vector2 hitDirection = hit.position - transform.position;
            hitDirection = -hitDirection.normalized;
            Knockback(hitDirection);

            BulletData bulletDamage = hit.gameObject.GetComponent<BulletData>();
            Laser laserDamage = hit.GetComponent<Laser>();
            SplashDamage splashDamage = hit.gameObject.GetComponent<SplashDamage>();
            float damaged;

            if (bulletDamage != null)
            {
                damaged = bulletDamage.damage;
            }
            else if (laserDamage != null)
            {
                damaged = laserDamage.damage;
            }
            else if (splashDamage != null)
            {
                damaged = splashDamage.damage;
            }
            else
            {
                damaged = bossTouchDamage;
            }

            LevelManager.Instance.GetDamaged(damaged);
            Invulnerable();
        }
    }

    public void Knockback(Vector2 knockBackDirection)
    {
        animator.SetBool("Hit", true);
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

        if (!EXshoot)
        {
            flasher = true;
            for (int i = 0; i < playerRenderers.Count; i++)
            {
                playerRenderers[i].enabled = false;
                flashTimer = flashPeriod;
            }
        }
        else
        {
            flasher = false;
        }
    }
}

// no bugs plz
