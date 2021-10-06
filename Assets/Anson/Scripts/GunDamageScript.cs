using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class GunDamageScript : DamageScript
{
    [Header("Gun Stats")]
    [SerializeField] protected MainGunStatsScript mainGunStatsScript;
    [SerializeField] protected float damagePerProjectile = 0;
    [SerializeField] protected float RPM = 0;
    [SerializeField] protected float reloadSpeed = 0;
    [SerializeField] protected Vector2 recoil = new Vector2(0, 0);
    [SerializeField] protected Vector2 recoil_HipFire = new Vector2(0, 0);
    [SerializeField] protected float range = 0;
    [SerializeField] protected float magazineSize = 0;
    [SerializeField] protected int projectilePerShot;
    [SerializeField] protected float timeBetweenProjectile = 0f;
    [SerializeField] GunTypes gunType = GunTypes.RIFLE;
    [SerializeField] Rarity rarity;
    [SerializeField] protected ElementTypes elementType = ElementTypes.PHYSICAL;
    [SerializeField] protected float elementDamage = 0;
    [SerializeField] protected float elementPotency = 0; //effect duration or range
    [SerializeField] protected float elementChance = 0;
    [SerializeField] FireTypes fireType = FireTypes.HitScan;
    [SerializeField] GameObject projectileGO;

    [SerializeField] protected AnimationCurve rangeCurve;
    [SerializeField] protected bool isFullAuto = true;
    [SerializeField] protected bool isFullReload = true;
    [SerializeField] protected int amountPerReload = 1;

    [SerializeField] AnimationCurve recoilPattern_X;
    [SerializeField] AnimationCurve recoilPattern_Y;
    [SerializeField] float timeToRecenter = 3f;


    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] GameObject impactEffect;
    [SerializeField] VisualEffect muzzleEffect;

    [SerializeField] Transform sightLocation;




    [Header("Current Stats")]
    [SerializeField] protected float timeUntilFire = 1f;
    [SerializeField] protected float timeNow_TimeUnitlFire;
    [SerializeField] protected int currentProjectile = 1;
    [SerializeField] protected float timeNow_TimeBetweenProjectile;
    [SerializeField] protected float currentMag;
    [SerializeField] protected bool isFiring = false;
    [SerializeField] protected bool isReloading = false;
    [SerializeField] protected Vector2 currentRecoil;
    [SerializeField] protected float currentRecoilTime = 0f;


    [Header("Other")]
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected Transform gunPosition;
    [SerializeField] protected bool isADS = false;
    [SerializeField] protected Vector3 fireDir;
    [SerializeField] protected Vector3 randomFireDir;
    [SerializeField] protected Vector3 sightOffset;
    [SerializeField] protected Coroutine currentReloadCoroutine;
    [SerializeField] protected Coroutine currentBurstCoroutine;

    [Header("Debug")]
    public bool displayFireRaycast = true;
    protected bool isAI = false;
    protected int currentSlot = 0;


    private void Awake()
    {
        AwakeBehaviour();
    }
    public virtual void AwakeBehaviour()
    {
        //lookScript = FindObjectOfType<Look>();
        if (mainGunStatsScript != null)
        {
            UpdateGunScript(mainGunStatsScript);
        }
        Reload_Action();
    }

    private void Update()
    {
        UpdateBehaviour();
    }
    protected virtual void UpdateBehaviour()
    {
        tickTime();

        //CorrectRecoil();

        if (isFiring)
        {
            Shoot();
        }

        if (displayFireRaycast)
        {
            Vector3 fireDir = firePoint.transform.forward;
            Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.green, 0f);
        }

    }


    public MainGunStatsScript TidyOldGun()
    {
        if (mainGunStatsScript != null)
        {
            mainGunStatsScript.CurrentMag = currentMag;
            mainGunStatsScript.PlayAnimationTrigger("Reset");
            convertWeaponLayerMask(mainGunStatsScript.gameObject, "Gun");
            //mainGunStatsScript.SetRarityEffect(true);
        }
        EndReload();
        isFiring = false;
        //currentRecoil = new Vector2(0, 0);
        currentRecoilTime = 0f;
        return mainGunStatsScript;
    }
    void convertWeaponLayerMask(GameObject currentGun, string layerName)
    {
        currentGun.gameObject.layer = LayerMask.NameToLayer(layerName);
        foreach (Transform child in currentGun.transform)
        {
            child.gameObject.layer = LayerMask.NameToLayer(layerName);
            convertWeaponLayerMask(child.gameObject, layerName);

            /*
            if (!child.TryGetComponent(out GunComponent_Sight s))
            {
                convertWeaponLayerMask(child.gameObject, layerName);

            }
            */
        }
    }

    public virtual MainGunStatsScript UpdateGunScript(MainGunStatsScript g, int slot = -1)
    {
        currentSlot = slot;
        isFiring = false;
        MainGunStatsScript oldGunScript = TidyOldGun();
        //Debug.Log("Weapon swap from " + mainGunStatsScript.name + " to " + g.name);

        if (g == null)
        {
            mainGunStatsScript = null;
            return null;
        }

        mainGunStatsScript = g;
        gunType = g.GunType;
        rarity = g.Rarity;
        damagePerProjectile = g.DamagePerProjectile;
        RPM = g.RPM_Get;
        reloadSpeed = g.ReloadSpeed;
        recoil = g.Recoil;
        recoil_HipFire = g.Recoil_HipFire;
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
        isFullReload = g.IsFullReload;
        amountPerReload = g.AmountPerReload;

        fireType = g.FireType;
        projectileGO = g.ProjectileGO;

        currentMag = g.CurrentMag;

        bulletParticle = g.BulletParticle;
        impactEffect = g.ImpactEffect;
        muzzleEffect = g.MuzzleEffect;

        recoilPattern_X = g.RecoilPattern_X;
        recoilPattern_Y = g.RecoilPattern_Y;
        timeToRecenter = g.TimeToRecenter;

        sightLocation = g.SightLocation;

        rangeCurve = g.RangeCurve;


        g.GetComponentInChildren<Rigidbody>().isKinematic = true;
        if (gunPosition == null)
        {
            gunPosition = transform;
        }

        g.gameObject.transform.position = gunPosition.position;
        g.gameObject.transform.SetParent(transform);
        g.gameObject.SetActive(true);
        convertWeaponLayerMask(g.gameObject, "PlayerGun");

        //g.transform.right = firePoint.forward;
        if (sightLocation == null)
        {
            sightLocation = transform;
        }
        /*
        mainGunStatsScript.transform.rotation = Quaternion.identity;
        sightOffset = sightLocation.position - gunPosition.position;
        mainGunStatsScript.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);
        */
        sightOffset = mainGunStatsScript.SightOffset;


        //HANDLE ELEMENTS.  Reduce main damage, change element damage
        elementDamage = Mathf.RoundToInt(g.ElementDamage * damagePerProjectile);
        //damagePerProjectile = damagePerProjectile * 0.85f;
        elementPotency = g.ElementPotency;
        elementChance = g.ElementChance;

        if (elementType != ElementTypes.PHYSICAL)
        {
            damagePerProjectile = damagePerProjectile * DamageMultiplier.ElementDamageNerf;
        }

        //mainGunStatsScript.SetRarityEffect(false);

        return oldGunScript;

    }



    public virtual void Fire(bool b)
    {
        //print(name + " Set Fire: " + b);
        //if (!isFullAuto)
        //{
        //    if (timeNow_TimeUnitlFire <= 0)
        //    {
        //        isFiring = b;
        //    }
        //}
        //else
        //{
        //    isFiring = b;
        //}
        isFiring = b;


    }

    public void SetFirePoint(Transform t)
    {
        firePoint = t;
    }

    protected bool canFire()
    {

        if (currentMag < 1 || (isReloading && isFullReload) || mainGunStatsScript == null)
        {
            Fire(false);
            if (currentMag < 1 && !isReloading)
            {
                Reload();
            }
            return false;
        }

        if (timeNow_TimeUnitlFire <= 0)
        {
            timeNow_TimeUnitlFire = timeUntilFire;
            if (!isFullReload)
            {
                isReloading = false;
                if (currentReloadCoroutine != null)
                {
                    StopCoroutine(currentReloadCoroutine);
                    currentReloadCoroutine = null;

                }
            }
            return true;
        }
        return false;

    }

    protected void tickTime()
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

    protected void CorrectRecoil(bool b = true)
    {
        if (b)
        {
            currentRecoil = Vector2.Lerp(currentRecoil, new Vector2(0, 0), Time.deltaTime * 2 / timeToRecenter);

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

        if (float.IsInfinity(Mathf.Abs(currentRecoilTime)))
        {
            currentRecoilTime = 0;

        }
    }







    public void Reload()
    {
        if (mainGunStatsScript == null) { return; }

        if (currentMag < magazineSize && !isReloading)
        {
            isReloading = true;
            currentReloadCoroutine = StartCoroutine(DelayReload());

        }
    }

    public bool Shoot()
    {
        //print("Shooting");
        if (canFire())
        {
            currentProjectile = projectilePerShot;

            switch (fireType)
            {
                case (FireTypes.HitScan):
                    RaycastDamage();
                    break;
                case (FireTypes.Projectile):
                    LaunchProjectile();
                    break;
            }
            HandleWeapon();

            if (currentProjectile > 0 && currentMag > 0 && timeBetweenProjectile > 0)
            {
                currentBurstCoroutine = StartCoroutine(BurstFire(currentRecoilTime));
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
        bool hitTarget = false;
        float randomX = Mathf.Clamp(Random.Range(0, currentRecoil.x * .5f) + Random.Range(0, recoil_HipFire.x), 0, recoil_HipFire.y);

        randomFireDir = new Vector2(randomX, Random.Range(-180f, 180f));

        fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) * Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) * firePoint.transform.forward;
        Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.blue, 1f);

        //Vector3 fireDir = firePoint.transform.forward;
        hitTarget = RayCastDealDamage(fireDir, hitTarget);


        if (gunType == GunTypes.SHOTGUN && projectilePerShot > 1)
        {
            //Shotgun Raycast
            for (int i = 0; i < projectilePerShot - 1; i++)
            {
                randomFireDir = new Vector2(Random.Range(recoil_HipFire.x * 0.35f, recoil_HipFire.x), (360f / (projectilePerShot - 1)) * i);
                fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) * Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) * firePoint.transform.forward;
                hitTarget = RayCastDealDamage(fireDir, hitTarget);
                Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.blue, 1f);
            }
        }
        return hitTarget;
    }

    bool Raycast_ADS()
    {
        RaycastHit hit;
        bool hitTarget = false;
        //Vector3 fireDir = Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0) * firePoint.transform.forward;
        fireDir = firePoint.transform.forward;
        Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.red, 1f);
        hitTarget = RayCastDealDamage(fireDir, hitTarget);


        if (gunType == GunTypes.SHOTGUN && projectilePerShot > 1)
        {
            //Shotgun Raycast
            for (int i = 0; i < projectilePerShot - 1; i++)
            {
                randomFireDir = new Vector2(Random.Range(0, recoil_HipFire.y), (360f / (projectilePerShot - 1)) * i);
                fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) * Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) * firePoint.transform.forward;
                hitTarget = RayCastDealDamage(fireDir, hitTarget);
                Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.blue, 1f);
            }
        }
        return hitTarget;

    }

    bool RayCastDealDamage(Vector3 dir, bool hitTarget)
    {
        RaycastHit hit;
        if (Physics.Raycast(firePoint.transform.position, dir, out hit, range * 1.5f, layerMask))
        {
            Instantiate(impactEffect, hit.point, Quaternion.Euler(hit.normal));
            if (tagList.Contains(hit.collider.tag) && (hit.collider.TryGetComponent(out LifeSystemScript ls) || hit.collider.TryGetComponent(out WeakPointScript weakPointScript)))
            {
                float dropOff = rangeCurve.Evaluate((firePoint.transform.position - hit.point).magnitude / range);
                if (hit.collider.TryGetComponent(out WeakPointScript wps))
                {
                    ls = wps.Ls;
                    try
                    {
                        dealCriticalDamageToTarget(ls, damagePerProjectile * dropOff, 1, elementType, DamageMultiplier.Get(gunType));

                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Failed to do damage");
                        Debug.LogError(e);
                    }
                }
                else
                {
                    dealDamageToTarget(ls, damagePerProjectile * dropOff, 1, elementType);

                }
                if (Random.Range(0, 1f) <= elementChance)
                {
                    ApplyElementEffect(ls);

                }
                hitTarget = true;
            }
        }
        return hitTarget;
    }




    void LaunchProjectile()
    {
        if (projectileGO.TryGetComponent(out ProjectileScript projectileScript))
        {
            RaycastHit hit;
            //for (int i = 0; i < projectilePerShot; i++)
            //{
            projectileScript = Instantiate(projectileGO, mainGunStatsScript.transform.position, Quaternion.identity).GetComponent<ProjectileScript>();
            float randomX = Mathf.Clamp(Random.Range(0, currentRecoil.x * .5f) + Random.Range(0, recoil_HipFire.x), 0, recoil_HipFire.y);

            randomFireDir = new Vector2(randomX, Random.Range(-180f, 180f));

            Vector3 fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) * Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) * firePoint.transform.forward;

            /*s
            if (Physics.Raycast(firePoint.transform.position, firePoint.forward, out hit, range * 1.5f, layerMask))
            {
                fireDir = hit.point - mainGunStatsScript.transform.position;
            }
            */

            projectileScript.Launch(damagePerProjectile, 1, elementType, fireDir.normalized);
            //}
        }
        else
        {
            Debug.LogError("Failed to get Projectile " + projectileGO.name + " script");
        }

    }

    protected virtual float HandleWeapon(float newRecoilTime = -1f)
    {
        mainGunStatsScript.PlayAnimationTrigger("Shoot", 1 / timeUntilFire);
        mainGunStatsScript.Play_Fire();
        Vector2 addRecoil = new Vector2();
        if (newRecoilTime < 0)
        {
            newRecoilTime = RecoilWeapon(out addRecoil);

        }
        else
        {
            newRecoilTime = RecoilWeapon(newRecoilTime, out addRecoil);
        }
        currentProjectile -= 1;
        currentMag -= 1;

        if (!isFullAuto)
        {
            Fire(false);
        }

        return newRecoilTime;
    }

    protected virtual float RecoilWeapon(out Vector2 addRecoil)
    {

        currentRecoilTime += 0.1f;
        addRecoil = new Vector2(recoilPattern_X.Evaluate(currentRecoilTime) * recoil.x, recoilPattern_Y.Evaluate(currentRecoilTime) * recoil.y);
        currentRecoil += addRecoil;


        if (gunType.Equals(GunTypes.SHOTGUN))
        {
            currentRecoil += new Vector2(recoilPattern_X.Evaluate(currentRecoilTime) * recoil.x, recoilPattern_Y.Evaluate(currentRecoilTime) * recoil.y) * projectilePerShot / 5f;

        }
        //print(currentRecoilTime + ", " + new Vector2(recoilPattern_X.Evaluate(currentRecoilTime) * recoil.x, recoilPattern_Y.Evaluate(currentRecoilTime) * recoil.y));
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

        return currentRecoilTime;

    }

    protected float RecoilWeapon(float newTime, out Vector2 addRecoil)
    {
        currentRecoilTime = newTime;
        return RecoilWeapon(out addRecoil);
    }

    protected virtual Quaternion SetWeaponRecoil()
    {
        Quaternion targetPoint;
        if (firePoint.transform.rotation.eulerAngles.x > 180f && firePoint.transform.rotation.eulerAngles.x - currentRecoil.x < 275f)
        {
            currentRecoil.x = firePoint.transform.rotation.eulerAngles.x - 275f;
            currentRecoil.y = 0f;
        }
        if (isADS)
        {
            //mainGunStatsScript.transform.localRotation = Quaternion.Lerp(mainGunStatsScript.transform.localRotation, Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0), 10f * Time.deltaTime);
            mainGunStatsScript.transform.localRotation = Quaternion.Euler(0, 0, 0);
            targetPoint = Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0);

        }
        else
        {
            targetPoint = Quaternion.Euler(-currentRecoil.x * .6f, currentRecoil.y * .2f, 0);
            mainGunStatsScript.transform.localRotation = Quaternion.Lerp(mainGunStatsScript.transform.localRotation, Quaternion.Euler(-currentRecoil.x * .6f, currentRecoil.y * .2f, 0), Time.deltaTime);
        }
        firePoint.transform.localRotation = Quaternion.Lerp(firePoint.transform.localRotation, targetPoint, 10f * Time.deltaTime);
        return targetPoint;

    }

    protected void SetWeaponLocation(bool forced = false)
    {
        if (isADS)
        {
            Vector3 targetPos = firePoint.transform.position - firePoint.transform.rotation * sightOffset;
            mainGunStatsScript.transform.position = Vector3.Lerp(mainGunStatsScript.transform.position, targetPos, 20 * Time.deltaTime);
            //if (isFiring || forced)
            //{
            //    mainGunStatsScript.transform.position = targetPos;

            //}
            //else
            //{
            //}

        }
        else
        {
            mainGunStatsScript.transform.position = Vector3.Lerp(mainGunStatsScript.transform.position, gunPosition.transform.position, 20 * Time.deltaTime);

        }
    }





    void ApplyElementEffect(LifeSystemScript ls)
    {
        base.ApplyElementEffect(ls, elementDamage, elementPotency, elementType);
    }





    IEnumerator BurstFire(float newRecoilTime)
    {
        //print("Bursting");
        yield return new WaitForSeconds(timeBetweenProjectile);

        switch (fireType)
        {
            case (FireTypes.HitScan):
                RaycastDamage();
                break;
            case (FireTypes.Projectile):
                LaunchProjectile();
                break;
        }
        newRecoilTime = HandleWeapon(newRecoilTime);
        if (currentProjectile > 0 && currentMag > 0)
        {
            if (currentBurstCoroutine != null)
            {
                StopCoroutine(currentBurstCoroutine);
            }
            currentBurstCoroutine = StartCoroutine(BurstFire(newRecoilTime));
        }
    }

    protected virtual IEnumerator DelayReload(float offset = 0)
    {
        isFiring = false;
        mainGunStatsScript.PlayAnimationTrigger("Reload", 1 / reloadSpeed);
        mainGunStatsScript.Play_StartReload();
        currentRecoilTime = 0f;
        yield return new WaitForSeconds(reloadSpeed - offset);
        if (Reload_Action())
        {
            currentReloadCoroutine = StartCoroutine(DelayReload(0.05f));
        }
    }

    private bool Reload_Action()
    {
        if (isFullReload)
        {
            mainGunStatsScript.Play_EndReload();
            currentMag = magazineSize;
            isReloading = false;
            EndReload();

        }
        else
        {
            currentMag += amountPerReload;
            mainGunStatsScript.Play_StartReload();
            if (currentMag < magazineSize)
            {
                return true;
            }
            else
            {
                isReloading = false;
                mainGunStatsScript.Play_EndReload();
                EndReload();

            }
        }
        return false;
    }

    public virtual void EndReload()
    {
        if (currentReloadCoroutine != null)
        {
            StopCoroutine(currentReloadCoroutine);
            currentReloadCoroutine = null;
        }
        isReloading = false;

    }





}
