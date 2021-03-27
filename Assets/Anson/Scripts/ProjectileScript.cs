using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float launchForce;
    [SerializeField] float launchSpeed;
    [SerializeField] float launchSpin;
    [SerializeField] float baseDamage;
    [SerializeField] private int level;
    [SerializeField] private ElementTypes elementType;


    [Header("Projectile Behaviour")]
    [SerializeField] Rigidbody rb;
    [SerializeField] PhysicMaterial physicMateral;
    [SerializeField] Collider collider;
    [SerializeField] int numberOfBounces = 0;
    [SerializeField] float fuseTime = Mathf.Infinity;
    [SerializeField] protected List<string> tagList;
    [SerializeField] ParticleSystem explodeEffect;
    [SerializeField] bool orientateProjectileToDirection;

    [Header("Swirl Behaciour")]
    [SerializeField] float swirlAmount;
    [SerializeField] float swirlFrequency = 1;
    [SerializeField] Vector3 swirlDirection;
    [SerializeField] Vector3 originalDir;
     Vector3 velocityValue;
    Vector2 seedOffset;

    protected int Level { get => level; set => level = value; }
    protected ElementTypes ElementType { get => elementType; set => elementType = value; }
    protected float BaseDamage { get => baseDamage; set => baseDamage = value; }


    // Start is called before the first frame update
    void Awake()
    {
        if (rb == null)
        {
            rb = FindObjectOfType<Rigidbody>();
        }
        if (collider == null)
        {
            collider = FindObjectOfType<Collider>();
        }
        if (collider.material == null)
        {
            collider.material = physicMateral;
        }
        seedOffset = new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
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

        if (swirlAmount > 0)
        {
            SwirlProjectile();
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
            Destroy(explodeEffect.gameObject,1f);
        }
    }
    public void PointProjectileToDirection()
    {
        transform.forward = rb.velocity.normalized;
    }

    public virtual void SwirlProjectile()
    {
        swirlDirection = new Vector3(Mathf.Sin(Time.time * swirlFrequency+seedOffset.x), Mathf.Cos(Time.time * swirlFrequency+seedOffset.y), 1/swirlAmount).normalized;
        velocityValue = (Quaternion.LookRotation(originalDir)* swirlDirection * launchSpeed);
        //velocityValue = Quaternion.LookRotation(originalDir) * Quaternion.LookRotation(swirlDirection) * rb.velocity;
        rb.velocity = velocityValue;
    }

}
