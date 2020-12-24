using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunDamageScript : DamageScript
{
    [Header("Gun Stats")]
    [SerializeField] protected MainGunStatsScript mainGunStatsScript;
    [SerializeField] protected float damagePerProjectile = 0;
    [SerializeField] protected float RPM = 0;
    [SerializeField] protected float reloadSpeed = 0;
    [SerializeField] protected Vector2 recoil = new Vector2(0, 0);
    [SerializeField] protected float range = 0;
    [SerializeField] protected float magazineSize = 0;
    [SerializeField] protected int projectilePerShot;
    [SerializeField] protected float timeBetweenProjectile = 0f;
    [SerializeField] GunTypes gunType = GunTypes.MID;
    [SerializeField] protected ElementTypes elementType = ElementTypes.PHYSICAL;
    [SerializeField] bool isFullAuto = true;

    [SerializeField] AnimationCurve recoilPattern_X;
    [SerializeField] AnimationCurve recoilPattern_Y;
    [SerializeField] float timeToRecenter = 3f;


    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] GameObject impactEffect;
    [SerializeField] GameObject muzzleEffect;

    [SerializeField] Transform sightLocation;




    [Header("Current Stats")]
    [SerializeField] float timeUntilFire = 1f;
    [SerializeField] float timeNow_TimeUnitlFire;
    [SerializeField] int currentProjectile = 1;
    [SerializeField] float timeNow_TimeBetweenProjectile;
    [SerializeField] float currentMag;
    [SerializeField] bool isFiring = false;
    [SerializeField] bool isReloading = false;
    [SerializeField] Vector2 currentRecoil;
    [SerializeField] float currentRecoilTime = 0f;


    [Header("Other")]
    [SerializeField] Camera camera;
    [SerializeField] Transform firePoint;
    [SerializeField] Transform gunPosition;
    [SerializeField] bool isADS = false;
    [SerializeField] Vector3 fireDir;
    [SerializeField] Vector3 randomFireDir;
    [SerializeField] Vector3 sightOffset;

    [Header("Debug")]
    public bool displayFireRaycast = true;


    private void Awake()
    {
    }

    private void Update()
    {
        tickTime();

        //CorrectRecoil();

        if (isFiring)
        {
            Shoot();
        }
        else
        {
            CorrectRecoil();
        }
        if (mainGunStatsScript != null)
        {
            SetWeaponRecoil();

        }

        if (displayFireRaycast)
        {
            Vector3 fireDir = firePoint.transform.forward;
            Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.green, 0f);
        }

    }



    public void UpdateGunScript(MainGunStatsScript g)
    {
        isFiring = false;
        if (mainGunStatsScript != null)
        {
            mainGunStatsScript.CurrentMag = currentMag;
            mainGunStatsScript.transform.position += transform.right;
            mainGunStatsScript.GetComponentInChildren<Rigidbody>().isKinematic = false;
            mainGunStatsScript.GetComponentInChildren<Rigidbody>().AddForce(transform.up * 1000f);
            mainGunStatsScript.gameObject.transform.parent = null;
            Debug.Log("Weapon swap from " + mainGunStatsScript.name + " to " + g.name);
        }
        mainGunStatsScript = g;
        damagePerProjectile = g.DamagePerProjectile;
        RPM = g.RPM_Get;
        reloadSpeed = g.ReloadSpeed;
        recoil = g.Recoil;
        range = g.Range;
        magazineSize = g.MagazineSize;
        projectilePerShot = g.ProjectilePerShot;
        timeBetweenProjectile = g.TimeBetweenProjectile;
        timeUntilFire = 60f / RPM;
        timeNow_TimeUnitlFire = timeUntilFire;
        timeNow_TimeBetweenProjectile = timeBetweenProjectile;
        currentMag = g.CurrentMag;
        gunType = g.GunType;
        elementType = g.ElementType;
        isFullAuto = g.IsFullAuto;
        currentMag = g.CurrentMag;

        bulletParticle = g.BulletParticle;
        impactEffect = g.ImpactEffect;
        muzzleEffect = g.MuzzleEffect;

        recoilPattern_X = g.RecoilPattern_X;
        recoilPattern_Y = g.RecoilPattern_Y;
        timeToRecenter = g.TimeToRecenter;

        sightLocation = g.SightLocation;

        g.GetComponentInChildren<Rigidbody>().isKinematic = true;
        g.gameObject.transform.position = gunPosition.transform.position;
        g.gameObject.transform.parent = transform;
        //g.transform.right = firePoint.forward;

        sightOffset = sightLocation.position - gunPosition.position;
        mainGunStatsScript.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

    }



    public void Fire(bool b)
    {
        isFiring = b;
    }

    public void SetFirePoint(Transform t)
    {
        firePoint = t;
    }

    bool canFire()
    {
        if (currentMag < 1 || isReloading)
        {
            isFiring = false;
            return false;
        }

        if (timeNow_TimeUnitlFire <= 0)
        {
            timeNow_TimeUnitlFire = timeUntilFire;
            return true;
        }
        return false;

    }

    void tickTime()
    {
        if (timeNow_TimeBetweenProjectile > 0)
        {
            timeNow_TimeBetweenProjectile -= Time.deltaTime;
        }
        else
        {
            timeNow_TimeBetweenProjectile = 0;
        }
        if (timeNow_TimeUnitlFire > 0)
        {
            timeNow_TimeUnitlFire -= Time.deltaTime;
        }
        else
        {
            timeNow_TimeUnitlFire = 0;
        }



    }

    void CorrectRecoil()
    {
        if (!isFiring)
        {
            //x
            if (currentRecoil.x > recoil.x * .2f)
            {
                currentRecoil.x -= currentRecoil.x * Time.deltaTime / timeToRecenter;
            }

            if (currentRecoil.x > 0.005f)
            {
                currentRecoil.x -= recoil.x * Time.deltaTime / timeToRecenter;
            }
            else
            {
                currentRecoil.x = 0;
            }


            //y
            if (currentRecoil.y > recoil.y * .2f)
            {
                currentRecoil.y -= currentRecoil.y * Time.deltaTime * 2 / timeToRecenter;
            }
            else if (currentRecoil.y < recoil.y * -.2f)
            {
                currentRecoil.y += Mathf.Abs(currentRecoil.y) * Time.deltaTime * 2 / timeToRecenter;
            } else if (currentRecoil.y > 0.05f)
            {
                currentRecoil.y -= recoil.y * Time.deltaTime * 2 / timeToRecenter;
            }
            else if (currentRecoil.y < -0.05f)
            {
                currentRecoil.y += recoil.y * Time.deltaTime * 2 / timeToRecenter;
            }
            else
            {
                currentRecoil.y = 0;
            }

        }


        if (currentRecoilTime > 0)
        {
            if (isFiring)
            {

                currentRecoilTime -= Time.deltaTime / (timeToRecenter * 2);
            }
            else
            {
                if (currentRecoilTime > 1.2f)
                {
                    currentRecoilTime = 1.2f;
                }
                currentRecoilTime -= Time.deltaTime / (timeToRecenter);

            }
        }
        else
        {
            currentRecoilTime = 0;
        }
    }

    public void Reload()
    {
        if (currentMag < magazineSize && !isReloading)
        {
            isReloading = true;
            StartCoroutine(DelayReload());

        }
    }

    public bool Shoot()
    {
        if (canFire())
        {
            currentProjectile = projectilePerShot;

            RaycastDamage();
            mainGunStatsScript.PlayAnimationTrigger("Shoot");
            HandleWeapon();

            if (currentProjectile > 0 && currentMag > 1)
            {
                StartCoroutine(BurstFire());
            }
            return true;
        }
        return false;
    }

    bool RaycastDamage()
    {
        if (isADS)
        {
            return Raycast_ADS();
        }
        else
        {
            return Raycast_HipFire();
        }

    }

    bool Raycast_HipFire()
    {
        RaycastHit hit;
        randomFireDir = new Vector2(Random.Range(0, currentRecoil.x*.5f), Random.Range(-180f, 180f));
        fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) * Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) * firePoint.transform.forward;
        Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.blue, 1f);

        //Vector3 fireDir = firePoint.transform.forward;
        if (Physics.Raycast(firePoint.transform.position, fireDir, out hit, range * 1.5f, layerMask))
        {
            Instantiate(impactEffect, hit.point, Quaternion.Euler(hit.normal));
            if (tagList.Contains(hit.collider.tag) && hit.collider.TryGetComponent(out LifeSystemScript ls))
            {
                dealDamageToTarget(ls, damagePerProjectile, 1, elementType);
                return true;
            }
        }


        if (gunType == GunTypes.HIGH && projectilePerShot > 1)
        {
            //Shotgun Raycast
        }
        return false;
    }

    bool Raycast_ADS()
    {
        RaycastHit hit;
        //Vector3 fireDir = Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0) * firePoint.transform.forward;
        fireDir = firePoint.transform.forward;
        Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.red, 1f);

        if (Physics.Raycast(firePoint.transform.position, fireDir, out hit, range * 1.5f, layerMask))
        {
            Instantiate(impactEffect, hit.point, Quaternion.Euler(hit.normal));
            if (tagList.Contains(hit.collider.tag) && hit.collider.TryGetComponent(out LifeSystemScript ls))
            {
                dealDamageToTarget(ls, damagePerProjectile, 1, elementType);
                return true;
            }
        }



        if (gunType == GunTypes.HIGH && projectilePerShot > 1)
        {
            //Shotgun Raycast
        }
        return false;

    }

    void HandleWeapon()
    {
        mainGunStatsScript.Play_Fire();
        RecoilWeapon();
        currentProjectile -= 1;
        currentMag -= 1;
        bulletParticle.Play();
        if (!isFullAuto)
        {
            isFiring = false;
        }
    }

    void RecoilWeapon()
    {

        currentRecoilTime += 0.1f;
        currentRecoil += new Vector2(recoilPattern_X.Evaluate(currentRecoilTime) * recoil.x, recoilPattern_Y.Evaluate(currentRecoilTime) * recoil.y);

        /*
        if (isADS)
        {
            currentRecoil += new Vector2(recoilPattern_X.Evaluate(currentRecoilTime) * recoil.x, recoilPattern_Y.Evaluate(currentRecoilTime) * recoil.y);
            //mainGunStatsScript.transform.localRotation = Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0);

        }
        else
        {
            currentRecoil += new Vector2(recoilPattern_X.Evaluate(currentRecoilTime) * recoil.x, recoilPattern_Y.Evaluate(currentRecoilTime) * recoil.y);
            //mainGunStatsScript.transform.localRotation = Quaternion.Euler(-currentRecoil.x * .4f, currentRecoil.y * .2f, 0);

        }
        */

    }

    void SetWeaponRecoil()
    {
        if (isADS)
        {

            mainGunStatsScript.transform.localRotation = Quaternion.Lerp(mainGunStatsScript.transform.localRotation, Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0),10f*Time.deltaTime);
            ADS_On();
        }
        else
        {
            mainGunStatsScript.transform.localRotation = Quaternion.Lerp(mainGunStatsScript.transform.localRotation, Quaternion.Euler(-currentRecoil.x * .4f, currentRecoil.y * .2f, 0), Time.deltaTime);


        }

    }

    public void ADS_On()
    {
        isADS = true;
        mainGunStatsScript.transform.position = transform.position - transform.rotation * sightOffset;
        firePoint.transform.position = sightLocation.position;
        firePoint.transform.forward = sightLocation.forward;
        firePoint.transform.rotation = Quaternion.Euler(firePoint.transform.rotation.eulerAngles.x, firePoint.transform.rotation.eulerAngles.y, 0f);

        camera.transform.position = sightLocation.position;
        camera.transform.forward = sightLocation.forward;
        camera.transform.rotation = Quaternion.Euler(mainGunStatsScript.transform.rotation.eulerAngles.x, mainGunStatsScript.transform.rotation.eulerAngles.y, 0f);

    }

    public void ADS_Off()
    {
        isADS = false;
        firePoint.transform.position = transform.position;
        firePoint.transform.rotation = transform.rotation;
        mainGunStatsScript.transform.position = gunPosition.transform.position;
        mainGunStatsScript.transform.rotation = gunPosition.transform.rotation;

        camera.transform.position = transform.position;
        camera.transform.rotation = transform.rotation;
    }


    IEnumerator BurstFire()
    {
        print("Bursting");
        yield return new WaitForSeconds(timeBetweenProjectile);

        RaycastDamage();
        HandleWeapon();
        if (currentProjectile >= 1 && currentMag > 1)
        {
            StartCoroutine(BurstFire());
        }
    }

    IEnumerator DelayReload()
    {
        isFiring = false;
        mainGunStatsScript.PlayAnimationTrigger("Reload", 1 / reloadSpeed);
        mainGunStatsScript.Play_StartReload();
        currentRecoilTime = 0f;
        yield return new WaitForSeconds(reloadSpeed);
        mainGunStatsScript.Play_EndReload();
        currentMag = magazineSize;
        isReloading = false;
    }



}
