using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour {

    [Header("Basic parameters")]
    [Tooltip("Speed of the projectile.")]
    public float speed;
    [Tooltip("Damage of the projectile (only used for player).")]
    public int damage = 1;
    [Tooltip("Velocity of the projectile in local space. Controls all straight movement. 1 is forward, -1 is backwards.")]
    public Vector3 velocity;
    [Tooltip("Timer controlling time between shots.")]
    public float timeBetweenShots;

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
    [Tooltip("How fast the value goes through the curve (X value Time).")]
    public float animationSpeedMultiplier;
    [Tooltip("The height of the curve (Y value Amount).")]
    public float curveMaxY;

    private float curveTimer;
    private float curveAmountX;
    private float curveAmountY;

    [Space(10)]
    [Header("Homing movement")]
    [Tooltip("Makes the projectile homing to the player.")]
    public bool useHoming;
    [Tooltip("Set a target for the projectile. If tag is set to ProjectileBoss, it sets the target to Player. If tag is set to Projectile, it sets the target to Boss.")]
    public Transform target;
    [Tooltip("Set the speed at which the projectile can turn around.")]
    public float angleChangingSpeed;

    [Space(10)]
    [Header("Targeted")]
    [Tooltip("Makes the projectile targeted at the target transform when shot.")]
    public bool useTarget;
    Vector3 targetPos;
    Vector3 velocityTarget;

    Rigidbody rb;

    void Start ()
    {
        rb = GetComponent<Rigidbody>();
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
    }

    void FixedUpdate ()
    {
        if (useCurve)
        {
            CurveMovement(); 
        }
        if (useHoming)
        {
            HomingMovement();
        }
        if (useTarget)
        {
            rb.velocity = velocityTarget * speed;
        }
        else {
            rb.velocity = transform.TransformDirection(velocity * speed);
        }
    }

    void OnTriggerEnter (Collider hit)
    {
        if (hit.gameObject.CompareTag("OutOfBounds"))
        {
            Destroy(gameObject);
        }
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
        }
        else
        {
            curveTimer = 0;
        }
    }

    void HomingMovement()
    {
        //needs fixing
        Vector3 direction = target.position - rb.position;
        direction.Normalize();
        Vector3 rotateAmount = Vector3.Cross(transform.up, direction);
        rb.angularVelocity = rotateAmount * angleChangingSpeed;
        rb.velocity = transform.up * speed;
    }
}