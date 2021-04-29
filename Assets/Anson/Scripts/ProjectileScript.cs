using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class ProjectileScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float launchForce;
    [SerializeField] float launchSpeed;
    [SerializeField] float launchSpin;
    [SerializeField] float baseDamage;
    [SerializeField] private int level;
    [SerializeField] private ElementTypes elementType;

    [Header("Effects")]
    [SerializeField] ParticleSystem explodeEffect;
    [SerializeField] GameObject[] delayDistroyGOs;
    [SerializeField] float delayTime;

    [Header("Projectile Behaviour")]
    [SerializeField] Rigidbody rb;
    [SerializeField] PhysicMaterial physicMateral;
    [SerializeField] Collider mainCollider;
    [SerializeField] int numberOfBounces = 0;
    [SerializeField] float fuseTime = Mathf.Infinity;
    [SerializeField] protected List<string> tagList;
    [SerializeField] bool orientateProjectileToDirection;

    [Header("Swirl Behaviour")]
    [SerializeField] float swirlAmount;
    [SerializeField] float swirlFrequency = 1;
    [SerializeField] Vector3 swirlDirection;
    [SerializeField] Vector3 originalDir;
    Vector3 velocityValue;
    Vector2 seedOffset;

    [Header("Homing Behaviour")]
    [SerializeField] bool isHoming;
    [SerializeField] bool homingLock;
    [SerializeField] ProjectileTriggerDetectionScript triggerDetectionScript;
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 homingDir;
    [SerializeField] float homingStrength = 50f;

    protected int Level { get => level; set => level = value; }
    protected ElementTypes ElementType { get => elementType; set => elementType = value; }
    protected float BaseDamage { get => baseDamage; set => baseDamage = value; }
    public ProjectileTriggerDetectionScript TriggerDetectionScript { get => triggerDetectionScript; set => triggerDetectionScript = value; }
    public List<string> TagList { get => tagList; set => tagList = value; }


    // Start is called before the first frame update
    void Awake()
    {
        if (rb == null)
        {
            rb = FindObjectOfType<Rigidbody>();
        }
        if (mainCollider == null)
        {
            mainCollider = FindObjectOfType<Collider>();
        }
        if (mainCollider.material == null)
        {
            mainCollider.material = physicMateral;
        }
        seedOffset = new Vector2(Random.Range(-180f, 180f), Random.Range(-180f, 180f));
    }

    private void OnEnable()
    {
    }

    private void Update()
    {
        if (fuseTime <= 0)
        {
            Explode();
        }

        fuseTime -= Time.deltaTime;

        if (orientateProjectileToDirection)
        {
            PointProjectileToDirection();
        }

        if (swirlAmount > 0 && !homingLock)
        {
            SwirlProjectile();
        }
        if (homingLock && isHoming)
        {
            HomingBehaviour();
        }
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (tagList.Contains(collision.collider.tag))
        {
            //print(collision.collider.name);
            if (collision.collider.tag.Equals("Enemy") || collision.collider.tag.Equals("Player"))
            {
                Explode();
            }
            else
            {

                if (numberOfBounces == 0)
                {
                    Explode();
                }
                numberOfBounces -= 1;
            }
        }
    }

    public virtual void Launch(float damage, int level, ElementTypes elementType)
    {
        Launch(damage, level, elementType, transform.forward);

    }

    public virtual void Launch(float damage, int level, ElementTypes elementType, Vector3 LaunchDir)
    {
        baseDamage = damage;
        this.level = level;
        this.elementType = elementType;
        transform.forward = LaunchDir;
        rb.velocity = LaunchDir * launchForce;
        rb.AddTorque(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * launchSpin);
        rb.AddForce(LaunchDir * launchForce * rb.mass);
        originalDir = LaunchDir;
    }

    public virtual void Explode()
    {
        //print(name + " go boom");
        if (explodeEffect != null)
        {
            explodeEffect = Instantiate(explodeEffect, transform.position, transform.rotation);
            explodeEffect.Play();
            Destroy(explodeEffect.gameObject, 1f);
            
        }
        DetattachEffects();
    }
    public void PointProjectileToDirection()
    {
        transform.forward = rb.velocity.normalized;
    }

    public virtual void SwirlProjectile()
    {
        swirlDirection = new Vector3(Mathf.Sin(Time.time * swirlFrequency + seedOffset.x), Mathf.Cos(Time.time * swirlFrequency + seedOffset.y), 1 / swirlAmount).normalized;
        velocityValue = (Quaternion.LookRotation(originalDir) * swirlDirection * launchSpeed);
        //velocityValue = Quaternion.LookRotation(originalDir) * Quaternion.LookRotation(swirlDirection) * rb.velocity;
        rb.velocity = velocityValue;
    }

    public virtual void SetHoming(Transform target)
    {
        if (homingLock || !isHoming)
        {
            return;
        }
        homingDir = (target.position - transform.position).normalized;

        float dotResults = Vector3.Dot(homingDir, transform.forward);
        if (dotResults <= 0f)
        {
            return;
        }
        homingLock = true;
        targetTransform = target;
    }

    public virtual void HomingBehaviour()
    {
        if (targetTransform == null)
        {
            homingLock = false;
            return;
        }
        homingDir = (targetTransform.position - transform.position).normalized;
        float dotResults = Vector3.Dot(homingDir, transform.forward);
        if (dotResults <= 0f)
        {
            resetHomingTarget();
        }
        else
        {
            rb.velocity = ((homingDir * dotResults*Time.deltaTime*homingStrength) + rb.velocity.normalized).normalized * launchSpeed;
        }
    }

    protected virtual void resetHomingTarget()
    {
        originalDir = transform.forward;
        homingLock = false;
        return;
    }

    protected virtual void DetattachEffects()
    {
        VisualEffect v;
        foreach(GameObject g in delayDistroyGOs)
        {
            if (g.TryGetComponent(out v))
            {
                v.SendEvent("OnStop");
            }
            g.transform.SetParent(null);
            Destroy(g, delayTime);
        }
    }

}
