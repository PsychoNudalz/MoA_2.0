using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public abstract class ProjectileScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField]
    float launchForce;

    [SerializeField]
    float launchSpeed;

    [SerializeField]
    float launchSpin;

    [SerializeField]
    float baseDamage;

    [SerializeField]
    private int level;

    [SerializeField]
    private ElementTypes elementType;

    [Header("Element")]
    [SerializeField]
    private bool triggerElement;

    [SerializeField]
    private float elementDamage;

    [SerializeField]
    private float elementPotency;

    [Header("Effects")]
    [SerializeField]
    [Tooltip("can take particle system, visual effects and sounds")]
    GameObject[] ExplodeEffects;

    [SerializeField]
    float delayTime;

    [Header("Projectile Behaviour")]
    [SerializeField]
    Rigidbody rb;

    [SerializeField]
    PhysicMaterial physicMateral;

    [SerializeField]
    Collider mainCollider;

    [SerializeField]
    int numberOfBounces = 0;

    [SerializeField]
    float fuseTime = Mathf.Infinity;

    [SerializeField]
    protected List<string> tagList;

    [SerializeField]
    bool orientateProjectileToDirection;

    [Header("Swirl Behaviour")]
    [SerializeField]
    float swirlAmount;

    [SerializeField]
    float swirlFrequency = 1;

    [SerializeField]
    Vector3 swirlDirection;

    [SerializeField]
    Vector3 originalDir;

    Vector3 velocityValue;
    Vector2 seedOffset;

    [Header("Homing Behaviour")]
    [SerializeField]
    bool isHoming;

    [SerializeField]
    bool homingLock;

    [SerializeField]
    bool omniHoming;

    [SerializeField]
    ProjectileTriggerDetectionScript triggerDetectionScript;

    [SerializeField]
    Transform targetTransform;

    [SerializeField]
    Vector3 homingDir;

    [SerializeField]
    float homingStrength = 50f;

    [Header("ShotData")]
    [SerializeField]
    private ShotData shotData;

    [SerializeField]
    private GunPerkController gunPerkController;


    private bool canExplode = true;
    protected int Level
    {
        get => level;
        set => level = value;
    }

    protected ElementTypes ElementType
    {
        get => elementType;
        set => elementType = value;
    }

    protected float BaseDamage
    {
        get => baseDamage;
        set => baseDamage = value;
    }

    public ProjectileTriggerDetectionScript TriggerDetectionScript
    {
        get => triggerDetectionScript;
        set => triggerDetectionScript = value;
    }

    public List<string> TagList
    {
        get => tagList;
        set => tagList = value;
    }

    public bool TriggerElement
    {
        get => triggerElement;
        set => triggerElement = value;
    }

    public float ElementDamage => elementDamage;

    public float ElementPotency => elementPotency;

    protected ShotData ShotData => shotData;

    protected GunPerkController GunPerkController => gunPerkController;


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
            if (canExplode)
            {
                Explode();
            }
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
                if (canExplode)
                {
                    Explode();
                }
            }
            else
            {
                if (numberOfBounces == 0)
                {
                    if (canExplode)
                    {
                        Explode();
                    }
                }

                numberOfBounces -= 1;
            }
        }
    }


    public virtual ShotData Launch(float damage, int level, ElementTypes elementType, Vector3 LaunchDir = new Vector3(),
        bool triggerElement = false, float elementDamage = 0f, float elementPotency = 0f,
        GunPerkController gunPerkController = null)
    {
        if (LaunchDir.magnitude == 0)
        {
            LaunchDir = transform.forward;
        }

        this.triggerElement = triggerElement;
        baseDamage = damage;
        this.level = level;
        this.elementType = elementType;
        this.elementDamage = elementDamage;
        this.elementPotency = elementPotency;
        transform.forward = LaunchDir;
        rb.velocity = LaunchDir * launchForce;
        if (launchSpin > 0)
        {
            rb.AddTorque(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)) * launchSpin);
        }

        rb.AddForce(LaunchDir * (launchForce * rb.mass));
        originalDir = LaunchDir;

        shotData = new ShotData();
        shotData.PlayerPos = transform.position;
        
        if (gunPerkController)
        {
            this.gunPerkController = gunPerkController;
            this.gunPerkController.OnProjectile_Shot(shotData);
        }

        return shotData;
    }

    public virtual void Explode()
    {
        canExplode = false;
        PlayExplodeEffect();
    }

    public virtual void PlayExplodeEffect()
    {
        VisualEffect v;
        ParticleSystem p;
        Sound s;
        foreach (GameObject g in ExplodeEffects)
        {
            if (g.TryGetComponent(out v))
            {
                v.Play();
            }
            else if(g.TryGetComponent(out p))
            {
                p.Play();
            }
            else if (g.TryGetComponent(out s))
            {
                s.Play();
            }

            g.transform.SetParent(null);
            Destroy(g, delayTime);
        }

    }

    public void PointProjectileToDirection()
    {
        transform.forward = rb.velocity.normalized;
    }

    public virtual void SwirlProjectile()
    {
        swirlDirection = new Vector3(Mathf.Sin(Time.time * swirlFrequency + seedOffset.x),
            Mathf.Cos(Time.time * swirlFrequency + seedOffset.y), 1 / swirlAmount).normalized;
        if (originalDir.magnitude.Equals(0))
        {
            velocityValue = swirlDirection * launchSpeed;
        }
        else
        {
            velocityValue = (Quaternion.LookRotation(originalDir) * swirlDirection * launchSpeed);
        }

        //velocityValue = Quaternion.LookRotation(originalDir) * Quaternion.LookRotation(swirlDirection) * rb.velocity;
        rb.velocity = velocityValue;
    }

    public virtual void SetHoming(Transform target)
    {
        if (target == null)
        {
            targetTransform = null;
            homingLock = false;
            return;
        }

        if (homingLock || !isHoming)
        {
            return;
        }

        homingDir = (target.position - transform.position).normalized;

        float dotResults = Vector3.Dot(homingDir, transform.forward);
        if (dotResults <= 0f && !omniHoming)
        {
            return;
        }

        homingLock = true;
        targetTransform = target;
    }

    public virtual void HomingBehaviour()
    {
        if (!targetTransform)
        {
            homingLock = false;
            return;
        }

        homingDir = (targetTransform.position - transform.position).normalized;
        float dotResults = Vector3.Dot(homingDir, transform.forward);
        if (dotResults <= 0f && !omniHoming)
        {
            ResetHomingTarget();
        }
        else
        {
            rb.velocity =
                ((homingDir * (Mathf.Abs(dotResults) * Time.deltaTime * homingStrength)) + rb.velocity.normalized)
                .normalized * launchSpeed;
        }
    }

    protected virtual void ResetHomingTarget()
    {
        originalDir = transform.forward;
        homingLock = false;
        return;
    }

}