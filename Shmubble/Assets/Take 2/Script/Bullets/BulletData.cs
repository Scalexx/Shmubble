using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour {

    [Header("Basic parameters")]
    [Tooltip("Speed of the projectile.")]
    public float speed;
    [Tooltip("Damage of the projectile.")]
    public float damage = 1;
    [Tooltip("Velocity of the projectile in local space. Controls all straight movement. 1 is forward, -1 is backwards.")]
    public Vector3 velocity;
    [Tooltip("Timer controlling time between shots.")]
    public float timeBetweenShots;
    [Tooltip("Destroys the projectile when colliding with the target.")]
    public bool destroyOnTouch;
    [Tooltip("Destroys other projectile when colliding.")]
    public bool destroyOtherOnTouch;
    [Tooltip("Destroys the projectile when colliding with the ground.")]
    public bool destroyOnCollisionGround;
    [Tooltip("Estimated time the projectile needs to leave the screen (Boss Idle wait time).")]
    public float stopTimer;

    [Space(10)]
    [Header("Curve movement")]
    [Tooltip("Use the curve to decide the way the projectile moves.")]
    public bool useCurve;

    [Space(10)]
    [Tooltip("Use a curve for the X velocity.")]
    public bool useXCurve;
    [Tooltip("Curve in which the projective moves on the X axis.")]
    public AnimationCurve trajectoryCurveX;

    [Space(10)]
    [Tooltip("Use a curve for the Y velocity.")]
    public bool useYCurve;
    [Tooltip("Curve in which the projective moves on the Y axis.")]
    public AnimationCurve trajectoryCurveY;

    [Space(10)]
    [Tooltip("Use a curve for the Z velocity.")]
    public bool useZCurve;
    [Tooltip("Curve in which the projective moves on the Z axis.")]
    public AnimationCurve trajectoryCurveZ;

    [Space(10)]
    [Tooltip("How fast the value goes through the curve (X value Time).")]
    public float animationSpeedMultiplier;
    [Tooltip("The height of the curve (Y value Amount).")]
    public float curveMaxY;

    private float curveTimer;
    private float curveAmountX;
    private float curveAmountY;
    private float curveAmountZ;

    [Space(10)]
    [Header("Homing movement")]
    [Tooltip("Makes the projectile homing to the player.")]
    public bool useHoming;
    private bool isHoming;
    [Tooltip("Set a target for the projectile. If tag is set to ProjectileBoss, it sets the target to Player. If tag is set to Projectile, it sets the target to Boss.")]
    public Transform target;
    [Tooltip("Set the speed at which the projectile can turn around.")]
    public float angleChangingSpeed;
    [Tooltip("Stop homing after this long.")]
    public float stopHomingTimer;
    float stopHomingPeriod;

    [Space(10)]
    [Header("Targeted")]
    [Tooltip("Makes the projectile targeted at the target transform when shot.")]
    public bool useTarget;
    Vector3 targetPos;
    Vector3 velocityTarget;

    [Space(10)]
    [Header("Effects")]
    [Tooltip("All the trail particles which will need to stay once the gameobject is destroyed.")]
    public List<GameObject> trails;
    [Tooltip("Gameobject that spawns once the bullet is destroyed.")]
    public GameObject destroyEffect;

    Rigidbody rb;
    bool solidObject;
    string bounds;

    void Start ()
    {
        if (useHoming)
        {
            isHoming = true;
            stopTimer = stopHomingTimer + stopTimer;
        }
    }

    void OnEnable ()
    {
        if (isHoming)
        {
            useHoming = true;
        }

        rb = GetComponent<Rigidbody>();

        AreaDenial adCheck = GetComponent<AreaDenial>();
        if (adCheck != null)
        {
            bounds = "OutOfBoundsFar";
            if (adCheck.solidObject)
            {
                gameObject.layer = 10;
                GetComponent<Collider>().isTrigger = false;
                rb.freezeRotation = true;
                solidObject = true;
            }
        }
        else
        {
            bounds = "OutOfBounds";
        }

        curveTimer = 0;

        if (gameObject.CompareTag ("ProjectileBoss"))
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
            gameObject.layer = 9;
        }
        else if (gameObject.CompareTag("Projectile"))
        {
            target = GameObject.FindGameObjectWithTag("Boss").transform;
            gameObject.layer = 8;
        }
        targetPos = target.position;
        velocityTarget = (targetPos - transform.position).normalized;

        stopHomingPeriod = stopHomingTimer;
    }

    void FixedUpdate ()
    {
        if (useCurve)
        {
            CurveMovement(); 
        }
        if (useTarget)
        {
            rb.velocity = velocityTarget * speed;
        }
        else if (solidObject)
        {
            transform.Translate(velocity * speed * Time.deltaTime);
        }
        else if (useHoming)
        {
            stopHomingPeriod -= Time.deltaTime;
            HomingMovement();
        }
        else {
            rb.angularVelocity = new Vector3(0, 0, 0);
            rb.velocity = transform.TransformDirection(velocity * speed);
        }
    }

    void OnTriggerEnter (Collider hit)
    {
        if (hit.gameObject.CompareTag(bounds))
        {
            gameObject.SetActive(false);
        }
        else if (destroyOtherOnTouch)
        {
            if (hit.gameObject.CompareTag("ProjectileBoss") || hit.gameObject.CompareTag("Projectile"))
            {
                hit.gameObject.SetActive(false);
            }
        }
        else if (destroyOnTouch && target.gameObject.layer == hit.gameObject.layer)
        {
            DestroyMe();
        }
        if (gameObject.CompareTag("Projectile") && hit.gameObject.CompareTag("Boss"))
        {
            DestroyMe();
        }
        if (destroyOnCollisionGround && hit.gameObject.layer == 10)
        {
            DestroyMe();
        }
    }

    void DestroyMe()
    {
        if (destroyEffect != null)
        {
            var impactVFX = Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation) as GameObject;
            Destroy(impactVFX, 5);
        }

        if (trails.Count > 0)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                var ps = trails[i].GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop();
                }
            }
        }

        gameObject.SetActive(false);
    }

    void CurveMovement()
    {
        if (curveTimer < 1.0f)
        {
            curveTimer += Time.deltaTime * animationSpeedMultiplier;
            if (useXCurve)
            {
                curveAmountX = trajectoryCurveX.Evaluate(curveTimer);
                velocity.x = curveAmountX * curveMaxY;
            }
            if (useYCurve)
            {
                curveAmountY = trajectoryCurveY.Evaluate(curveTimer);
                velocity.y = curveAmountY * curveMaxY;
            }
            if (useZCurve)
            {
                curveAmountZ = trajectoryCurveZ.Evaluate(curveTimer);
                velocity.z = curveAmountZ * curveMaxY;
            }
        }
        else
        {
            curveTimer = 0;
        }
    }

    void HomingMovement()
    {
        Vector3 direction = Vector3.zero;
        if (stopHomingPeriod >= 0)
        {
            direction = target.position - rb.position;
            direction.Normalize();
        }
        Vector3 rotateAmount = Vector3.Cross(transform.right, direction);
        rb.angularVelocity = rotateAmount * angleChangingSpeed;
        velocity = transform.right.normalized;
        rb.velocity = transform.right * speed;
    }
}

// no bugs plz