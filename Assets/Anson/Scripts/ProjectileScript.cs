using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ProjectileScript : MonoBehaviour
{
    [Header("Stats")]
    [SerializeField] float launchForce;
    [SerializeField] float launchSpeed;
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


    protected int Level { get => level; set => level = value; }
    protected ElementTypes ElementType { get => elementType; set => elementType = value; }


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
    }

    private void Update()
    {
        if (fuseTime <= 0)
        {
            Explode();
        }

        fuseTime -= Time.deltaTime;
    }



    private void OnCollisionEnter(Collision collision)
    {
        if (tagList.Contains(collision.collider.tag))
        {
            print(collision.collider.name);
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

    public virtual void Launch(int level, ElementTypes elementType)
    {
        Launch(level, elementType, transform.forward);
    }

    public virtual void Launch(int level, ElementTypes elementType, Vector3 LaunchDir)
    {
        this.level = level;
        this.elementType = elementType;
        transform.forward = LaunchDir;
        rb.velocity = LaunchDir * launchForce;
        rb.AddForce(LaunchDir * launchForce * rb.mass);
    }

    public virtual void Explode()
    {
        print(name + " go boom");
        if (explodeEffect != null)
        {
            explodeEffect = Instantiate(explodeEffect, transform.position, transform.rotation);
            explodeEffect.Play();
            Destroy(explodeEffect.gameObject,1f);
        }
    }

}
