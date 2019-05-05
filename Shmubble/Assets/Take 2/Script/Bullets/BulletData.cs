using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletData : MonoBehaviour {

    [Header("Basic parameters")]
    [Tooltip("Speed of the projectile.")]
    public float startSpeed;
    float speed;
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
    public List<GameObject> trails = new List<GameObject>();
    List<GameObject> myTrails = new List<GameObject>();
    [Tooltip("Gameobject that spawns once the bullet is destroyed.")]
    public GameObject destroyEffect;
    GameObject destroyEffectSpawned;

    [Space(10)]
    [Tooltip("The name of the sound that plays throughout the bullet's lifetime.")]
    public string trailSound;
    [Tooltip("The name of the sound that plays on impact.")]
    public string impactSound;

    bool firstTime = true;
    bool enteredTarget;
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
        myTrails.Clear();

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
        stopHomingPeriod = stopHomingTimer;

        if (gameObject.CompareTag ("ProjectileBoss"))
        {
            if (!firstTime)
            {
                AudioManager.instance.PlayBossSound(trailSound);
            }

            target = GameObject.FindGameObjectWithTag("Player").transform;
            if (target != null)
            {
                gameObject.layer = 9;
            }
            else
            {
                return;
            }
        }
        else if (gameObject.CompareTag("Projectile"))
        {
            if (!firstTime)
            {
                AudioManager.instance.PlayPlayerSound(trailSound);
            }

            target = GameObject.FindGameObjectWithTag("Boss").transform;
            if (target != null)
            {
                gameObject.layer = 8;
            }
            else
            {
                return;
            }
        }
        if (target != null)
        {
            targetPos = target.position;
            velocityTarget = (targetPos - transform.position).normalized;
        }
        
        speed = startSpeed;

        if (trails.Count > 0 && myTrails.Count == 0)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                var trailObject = Instantiate(trails[i], transform.position + trails[i].transform.position, transform.rotation * trails[i].transform.rotation);
                myTrails.Add(trailObject);
                myTrails[i].transform.parent = gameObject.transform;
            }
        }

        enteredTarget = false;
        destroyEffectSpawned = null;

        if (firstTime)
        {
            firstTime = false;
        }
    }

    void FixedUpdate ()
    {
        if (useCurve)
        {
            CurveMovement(); 
        }
        if (useTarget)
        {
            if (!enteredTarget)
            {
                Vector3 direction = targetPos - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = rotation;

                enteredTarget = true;
            }

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
            DestroyMe();
        }
        else if (destroyOtherOnTouch)
        {
            if (hit.gameObject.CompareTag("ProjectileBoss") || hit.gameObject.CompareTag("Projectile"))
            {
                hit.gameObject.SetActive(false);
            }
            if (hit.gameObject.layer == target.gameObject.layer)
            {
                Impact();
                DestroyMe();
            }
        }
        else if (destroyOnTouch && target.gameObject.layer == hit.gameObject.layer)
        {
            if (gameObject.layer == 9)
            {
                AudioManager.instance.PlayBossSound(impactSound);
            }
            else
            {
                AudioManager.instance.PlayPlayerSound(impactSound);
            }
            
            Impact();
            DestroyMe();
        }
        else if (gameObject.CompareTag("Projectile") && hit.gameObject.CompareTag("Boss"))
        {
            AudioManager.instance.PlayPlayerSound(impactSound);

            Impact();
            DestroyMe();
        }
        else if (destroyOnCollisionGround && hit.gameObject.layer == 10)
        {
            AudioManager.instance.PlayBossSound(impactSound);

            Impact();
            DestroyMe();
        }
    }

    void Impact()
    {
        if (destroyEffect != null && destroyEffectSpawned == null)
        {
            var impactVFX = Instantiate(destroyEffect, transform.position, destroyEffect.transform.rotation) as GameObject;
            destroyEffectSpawned = impactVFX;

            Destroy(impactVFX, 5);
        }
    }

    void DestroyMe()
    {
        speed = 0;

        if (trails.Count > 0)
        {
            for (int i = 0; i < trails.Count; i++)
            {
                myTrails[i].transform.parent = null;
                var ps = myTrails[i].GetComponent<ParticleSystem>();
                if (ps != null)
                {
                    ps.Stop();
                    Destroy(ps.gameObject, ps.main.duration + ps.main.startLifetime.constantMax);
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
        if (stopHomingPeriod >= 0 && target != null)
        {
            direction = target.position - rb.position;
            direction.Normalize();
            Vector3 rotateAmount = Vector3.Cross(transform.right, direction);
            rb.angularVelocity = rotateAmount * angleChangingSpeed;
        }
        else
        {
            rb.angularVelocity = Vector3.zero;
            return;
        }
        
        velocity = transform.right.normalized;
        rb.velocity = transform.right * speed;
    }
}

// no bugs plz