using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class GunDamageScript : DamageScript
{
    [Header("Gun Stats")]
    [SerializeField]
    protected MainGunStatsScript mainGunStatsScript;

    [SerializeField]
    protected float damagePerProjectile = 0;

    [SerializeField]
    protected float RPM = 0;

    [SerializeField]
    protected float reloadSpeed = 0;

    [SerializeField]
    protected Vector2 recoil = new Vector2(0, 0);

    [SerializeField]
    protected Vector2 recoil_HipFire = new Vector2(0, 0);

    [SerializeField]
    protected float range = 0;

    [SerializeField]
    protected float magazineSize = 0;

    [SerializeField]
    protected int projectilePerShot;

    [SerializeField]
    protected float timeBetweenProjectile = 0f;

    [SerializeField]
    GunTypes gunType = GunTypes.RIFLE;

    [SerializeField]
    Rarity rarity;

    [SerializeField]
    protected ElementTypes elementType = ElementTypes.PHYSICAL;

    [SerializeField]
    protected float elementDamage = 0;

    [SerializeField]
    protected float elementPotency = 0; //effect duration or range

    [SerializeField]
    protected float elementChance = 0;

    [SerializeField]
    FireTypes fireType = FireTypes.HitScan;

    [SerializeField]
    GameObject projectileGO;

    [SerializeField]
    protected AnimationCurve rangeCurve;

    [SerializeField]
    protected bool isFullAuto = true;

    [SerializeField]
    protected bool isFullReload = true;

    [SerializeField]
    protected int amountPerReload = 1;

    [SerializeField]
    AnimationCurve recoilPattern_X;

    [SerializeField]
    AnimationCurve recoilPattern_Y;

    [SerializeField]
    float timeToRecenter = 3f;


    [SerializeField]
    ParticleSystem bulletParticle;

    [SerializeField]
    GameObject impactEffect;

    [SerializeField]
    VisualEffect muzzleEffect;

    [SerializeField]
    Transform sightLocation;


    [Header("Current Stats")]
    [SerializeField]
    protected float timeUntilFire = 1f;

    [SerializeField]
    protected float timeNow_TimeUnitlFire;

    [SerializeField]
    protected int currentProjectile = 1;

    [SerializeField]
    protected float timeNow_TimeBetweenProjectile;

    [SerializeField]
    protected float currentMag;

    [SerializeField]
    protected bool isFiring = false;

    [SerializeField]
    protected bool isReloading = false;

    [SerializeField]
    protected Vector2 currentRecoil;

    [SerializeField]
    protected float currentRecoilTime = 0f;

    [SerializeField]
    protected float recoilDegree;


    [Header("Other Fields")]
    [SerializeField]
    protected Transform firePoint;

    [SerializeField]
    protected Transform gunPosition;

    [SerializeField]
    protected bool isADS = false;

    [SerializeField]
    protected Vector3 fireDir;

    [SerializeField]
    protected Vector3 randomFireDir;

    [SerializeField]
    protected Vector3 sightOffset;

    protected Coroutine currentReloadCoroutine;

    protected Coroutine currentBurstCoroutine;


    [Header("Debug")]
    public bool displayFireRaycast = true;

    protected bool isAI = false;
    protected int currentSlot = 0;
    private RaycastHit raycastHit;

    [Header("Other Components")]
    [SerializeField]
    protected GunEffectsController gunEffectsController;

    [SerializeField]
    protected GunPerkController gunPerkController;


    public float CurrentMag => currentMag;

    public float MagazineSize => magazineSize;

    public float DamagePerProjectile
    {
        get => damagePerProjectile;
        set => damagePerProjectile = value;
    }


    public Vector2 Recoil => recoil;

    public Vector2 RecoilHipFire => recoil_HipFire;


    public GunTypes GunType => gunType;

    public float RecoilDegree => recoilDegree;

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

        UpdateRecoilDegree();
    }


    public virtual void UnequipOldGun()
    {
        EndReload();
        Fire(false);
        //currentRecoil = new Vector2(0, 0);
        currentRecoilTime = 0f;
        gunPerkController?.OnUnequip();
        mainGunStatsScript = null;
        return;
    }

    public void ResetToWorldLoot()
    {
        if (mainGunStatsScript)
        {
            mainGunStatsScript.CurrentMag = currentMag;
            mainGunStatsScript.ResetToWorldLoot();
            gunEffectsController.PlayAnimationTrigger("Reset");
            int[] temp = {LayerMask.NameToLayer("Debug")};

            AnsonUtility.ConvertLayerMask(mainGunStatsScript.gameObject, "Gun", new List<int>(temp));
            //mainGunStatsScript.SetRarityEffect(true);
        }
    }


    public virtual MainGunStatsScript UpdateGunScript(MainGunStatsScript g, int slot = -1)
    {
        currentSlot = slot;
        isFiring = false;
        MainGunStatsScript oldGunScript = mainGunStatsScript;
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
        RPM = g.GetRPM;
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
        projectileGO = g.ProjectileGo;

        currentMag = g.CurrentMag;

        bulletParticle = g.BulletParticle;
        impactEffect = g.ImpactEffect;
        muzzleEffect = g.MuzzleEffect;

        recoilPattern_X = g.RecoilPatternX;
        recoilPattern_Y = g.RecoilPatternY;
        timeToRecenter = g.TimeToRecenter;

        sightLocation = g.SightLocation;

        rangeCurve = g.RangeCurve;
        gunEffectsController = g.GunEffectsController;
        if (g.GunPerkController)
        {
            gunPerkController = g.GunPerkController;
            gunPerkController.InitialisePerks(this, g, g);
        }


        g.GetComponentInChildren<Rigidbody>().isKinematic = true;
        if (gunPosition == null)
        {
            gunPosition = transform;
        }

        g.gameObject.transform.position = gunPosition.position;
        g.gameObject.transform.SetParent(transform);
        g.gameObject.SetActive(true);

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
            damagePerProjectile = damagePerProjectile * UniversalValues.ElementDamageNerf;
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


    public virtual void Reload()
    {
        if (mainGunStatsScript == null)
        {
            return;
        }

        if (currentMag < magazineSize && !isReloading)
        {
            gunPerkController.OnReloadStart();
            isReloading = true;
            currentReloadCoroutine = StartCoroutine(DelayReload());
        }
    }

    public virtual bool Shoot(ShotData shotData = null)
    {
        if (shotData == null)
        {
            shotData = new ShotData(transform.position);
        }

        //print("Shooting");
        if (canFire())
        {
            currentProjectile = projectilePerShot;

            switch (fireType)
            {
                case (FireTypes.HitScan):
                    RaycastDamage(shotData);
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

    bool RaycastDamage(ShotData shotData)
    {
        if (isADS)
        {
            return Raycast_ADS(shotData);
        }
        else
        {
            return Raycast_HipFire(shotData);
        }
    }

    bool Raycast_HipFire(ShotData shotData)
    {
        bool hitTarget = false;
        float randomX = Random.Range(0, currentRecoil.x * UniversalValues.HipFireRecoilMultiplier) +
                        Random.Range(0, recoil_HipFire.x);
        randomX = Mathf.Clamp(randomX, 0, recoil_HipFire.y);

        randomFireDir = new Vector2(randomX, Random.Range(-180f, 180f));

        fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) *
                  Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) * firePoint.transform.forward;
        Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.blue, 1f);

        //Vector3 fireDir = firePoint.transform.forward;
        hitTarget = RayCastDealDamage(fireDir, hitTarget, shotData);


        if (gunType == GunTypes.SHOTGUN && projectilePerShot > 1)
        {
            //Shotgun Raycast
            for (int i = 0; i < projectilePerShot - 1; i++)
            {
                randomFireDir = new Vector2(Random.Range(recoil_HipFire.x * 0.35f, recoil_HipFire.x),
                    (360f / (projectilePerShot - 1)) * i);
                fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) *
                          Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) *
                          firePoint.transform.forward;
                hitTarget = RayCastDealDamage(fireDir, hitTarget, shotData);
                Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.blue, 1f);
            }
        }

        return hitTarget;
    }

    bool Raycast_ADS(ShotData shotData)
    {
        bool hitTarget = false;
        //Vector3 fireDir = Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0) * firePoint.transform.forward;
        fireDir = firePoint.transform.forward;
        Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.red, 1f);
        hitTarget = RayCastDealDamage(fireDir, hitTarget, shotData);


        if (gunType == GunTypes.SHOTGUN && projectilePerShot > 1)
        {
            //Shotgun Raycast
            for (int i = 0; i < projectilePerShot - 1; i++)
            {
                randomFireDir = new Vector2(Random.Range(0, recoil_HipFire.y), (360f / (projectilePerShot - 1)) * i);
                fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) *
                          Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) *
                          firePoint.transform.forward;
                hitTarget = RayCastDealDamage(fireDir, hitTarget, shotData);
                Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.blue, 1f);
            }
        }

        return hitTarget;
    }

    bool RayCastDealDamage(Vector3 dir, bool hitTarget, ShotData shotData)
    {
        if (Physics.Raycast(firePoint.transform.position, dir, out raycastHit, range * 1.5f, layerMask))
        {
            shotData.IsHit = true;
            shotData.HitPos = raycastHit.point;

            Instantiate(impactEffect, raycastHit.point, Quaternion.Euler(raycastHit.normal));
            LifeSystemScript ls = raycastHit.collider.GetComponentInParent<LifeSystemScript>();

            if (tagList.Contains(raycastHit.collider.tag) &&
                (ls ||
                 raycastHit.collider.TryGetComponent(out WeakPointScript weakPointScript)))
            {
                shotData.IsTargetHit = true;


                float dropOff =
                    rangeCurve.Evaluate((firePoint.transform.position - raycastHit.point).magnitude / range);
                if (raycastHit.collider.TryGetComponent(out WeakPointScript wps))
                {
                    ls = wps.Ls;
                    shotData.SetLifeSystem(ls);

                    try
                    {
                        shotData.IsCritical = true;
                        shotData.IsKill = dealCriticalDamageToTarget(ls, damagePerProjectile * dropOff, 1, elementType,
                            UniversalValues.GetDamageMultiplier(gunType));
                        shotData.ShotDamage +=
                            damagePerProjectile * dropOff * UniversalValues.GetDamageMultiplier(gunType);
                    }
                    catch (System.Exception e)
                    {
                        Debug.LogError("Failed to do damage");
                        Debug.LogWarning(e);
                    }
                }
                else
                {
                    shotData.SetLifeSystem(ls);

                    shotData.IsKill = dealDamageToTarget(ls, damagePerProjectile * dropOff, 1, elementType);
                    shotData.ShotDamage += damagePerProjectile * dropOff;
                }

                if (Random.Range(0, 1f) <= elementChance)
                {
                    shotData.IsElementTrigger = true;
                    shotData.IsKill = shotData.IsKill || ApplyElementEffect(ls);
                }

                hitTarget = true;
            }
        }

        UpdateBulletTrailStuff();

        return hitTarget;
    }

    private void UpdateBulletTrailStuff()
    {
        //Set fire hitpoints and direction
        try
        {
            if (raycastHit.point.magnitude > 0)
            {
                gunEffectsController?.AddBulletTrail(raycastHit);
            }
            else
            {
                gunEffectsController?.AddBulletTrail(fireDir);
            }
        }
        catch (NullReferenceException e)
        {
            Debug.LogWarning(e);
            //throw;
        }
    }


    void LaunchProjectile()
    {
        if (projectileGO.TryGetComponent(out ProjectileScript projectileScript))
        {
            if (timeBetweenProjectile == 0)
            {
                for (int i = 0; i < projectilePerShot; i++)
                {
                    SpawnLaunchProjectile();
                }

                currentProjectile++;
            }
            else
            {
                SpawnLaunchProjectile();
            }
        }
        else
        {
            Debug.LogError("Failed to get Projectile " + projectileGO.name + " script");
        }
    }

    private void SpawnLaunchProjectile()
    {
        ProjectileScript projectileScript = Instantiate(projectileGO, mainGunStatsScript.transform.position, Quaternion.identity)
            .GetComponent<ProjectileScript>();
        float randomX = Mathf.Clamp(Random.Range(0, currentRecoil.x * .5f) + Random.Range(0, recoil_HipFire.x),
            0,
            recoil_HipFire.y);

        randomFireDir = new Vector2(randomX, Random.Range(-180f, 180f));

        Vector3 fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) *
                          Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) *
                          firePoint.transform.forward;

        projectileScript.Launch(damagePerProjectile, 1, elementType, fireDir.normalized);
    }

    protected virtual float HandleWeapon(float newRecoilTime = -1f)
    {
        gunEffectsController.PlayAnimationTrigger("Shoot");
        gunEffectsController.PlaySound_Fire();
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
        addRecoil = new Vector2(recoilPattern_X.Evaluate(currentRecoilTime) * recoil.x,
            recoilPattern_Y.Evaluate(currentRecoilTime) * recoil.y);
        if (!isADS)
        {
            addRecoil *= UniversalValues.HipFireRecoilMultiplier;
        }

        currentRecoil += addRecoil;


        if (gunType.Equals(GunTypes.SHOTGUN))
        {
            currentRecoil += addRecoil * projectilePerShot / UniversalValues.ShotgunPelletRecoilMultiplier;
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
        if (firePoint.transform.rotation.eulerAngles.x > 180f &&
            firePoint.transform.rotation.eulerAngles.x - currentRecoil.x < 275f)
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
            mainGunStatsScript.transform.localRotation = Quaternion.Lerp(mainGunStatsScript.transform.localRotation,
                Quaternion.Euler(-currentRecoil.x * .6f, currentRecoil.y * .2f, 0), Time.deltaTime);
        }

        firePoint.transform.localRotation =
            Quaternion.Lerp(firePoint.transform.localRotation, targetPoint, 10f * Time.deltaTime);
        return targetPoint;
    }

    protected void SetWeaponLocation(bool forced = false)
    {
        if (isADS)
        {
            Vector3 targetPos = firePoint.transform.position - firePoint.transform.rotation * sightOffset;
            mainGunStatsScript.transform.position =
                Vector3.Lerp(mainGunStatsScript.transform.position, targetPos, 20 * Time.deltaTime);
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
            mainGunStatsScript.transform.position = Vector3.Lerp(mainGunStatsScript.transform.position,
                gunPosition.transform.position, 20 * Time.deltaTime);
        }
    }


    bool ApplyElementEffect(LifeSystemScript ls)
    {
        return base.ApplyElementEffect(ls, elementDamage, elementPotency, elementType);
    }


    IEnumerator BurstFire(float newRecoilTime)
    {
        //print("Bursting");
        yield return new WaitForSeconds(timeBetweenProjectile);
        ShotData shotData = new ShotData(transform.position);
        switch (fireType)
        {
            case (FireTypes.HitScan):
                RaycastDamage(shotData);
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
        Fire(false);
        gunEffectsController.PlayAnimationTrigger("Reload");
        gunEffectsController.PlaySound_StartReload();
        currentRecoilTime = 0f;
        yield return new WaitForSeconds(reloadSpeed - offset);
        gunPerkController.OnPerReload();

        if (Reload_Action())
        {
            currentReloadCoroutine = StartCoroutine(DelayReload(0.05f));
        }
    }

    private bool Reload_Action()
    {
        if (isFullReload)
        {
            gunEffectsController.PlaySound_EndReload();
            currentMag = magazineSize;
            isReloading = false;
            EndReload();
        }
        else
        {
            currentMag += amountPerReload;
            gunEffectsController.PlaySound_StartReload();
            if (currentMag < magazineSize)
            {
                return true;
            }
            else
            {
                isReloading = false;
                gunEffectsController.PlaySound_EndReload();
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

    public void AddPerkStats(PerkGunStatsScript g)
    {
        damagePerProjectile += g.DamagePerProjectile;
        RPM += g.GetRPM;
        reloadSpeed += g.ReloadSpeed;
        recoil += g.Recoil;
        recoil_HipFire += g.Recoil_HipFire;
        range += g.Range;
        magazineSize += g.MagazineSize;
        elementDamage += g.ElementDamage;
        elementPotency += g.ElementPotency;
        elementChance += g.ElementChance;

        damagePerProjectile = damagePerProjectile * g.damagePerProjectileM;
        RPM = RPM * g.RPMM;
        reloadSpeed = reloadSpeed * g.reloadSpeedM;
        //recoil = recoil * g.recoilM;
        recoil = new Vector2(recoil.x * g.recoilM.x, recoil.y * g.recoilM.y);
        recoil_HipFire = new Vector2(recoil_HipFire.x * g.hipfireM.x, recoil_HipFire.y * g.hipfireM.y);
        range = range * g.rangeM;
        magazineSize = magazineSize * g.magazineSizeM;

        timeUntilFire = 60f / RPM;
        gunEffectsController.updateAnimatorSpeeds(reloadSpeed, RPM);
    }

    public void AddPerkStatsAdditive(PerkGunStatsScript g)
    {
        damagePerProjectile += mainGunStatsScript.DamagePerProjectile * (g.damagePerProjectileM - 1);
        RPM += mainGunStatsScript.GetRPM * (g.RPMM - 1);
        reloadSpeed += mainGunStatsScript.ReloadSpeed * (g.reloadSpeedM - 1);
        //recoil = recoil * g.recoilM;
        recoil += new Vector2(mainGunStatsScript.Recoil.x * (g.recoilM.x - 1),
            mainGunStatsScript.Recoil.y * (g.recoilM.y - 1));
        recoil_HipFire += new Vector2(mainGunStatsScript.Recoil_HipFire.x * (g.hipfireM.x - 1),
            mainGunStatsScript.Recoil_HipFire.y * (g.hipfireM.y - 1));
        range += mainGunStatsScript.Range * (g.rangeM - 1);
        magazineSize += mainGunStatsScript.MagazineSize * (g.magazineSizeM - 1);

        timeUntilFire = 60f / RPM;
        gunEffectsController.updateAnimatorSpeeds(reloadSpeed, RPM);

        if (GunType != GunTypes.SHOTGUN)
        {
            recoil_HipFire.y = Mathf.Max(recoil_HipFire.y, recoil_HipFire.x);
        }
    }

    public void RemovePerkStats(PerkGunStatsScript g)
    {
        damagePerProjectile -= g.DamagePerProjectile;
        RPM -= g.GetRPM;
        reloadSpeed -= g.ReloadSpeed;
        recoil -= g.Recoil;
        recoil_HipFire -= g.Recoil_HipFire;
        range -= g.Range;
        magazineSize -= g.MagazineSize;
        elementDamage -= g.ElementDamage;
        elementPotency -= g.ElementPotency;
        elementChance -= g.ElementChance;

        damagePerProjectile /= g.damagePerProjectileM;
        RPM /= g.RPMM;
        reloadSpeed /= g.reloadSpeedM;
        //recoil = recoil * g.recoilM;
        recoil = new Vector2(recoil.x / g.recoilM.x, recoil.y / g.recoilM.y);
        recoil_HipFire = new Vector2(recoil_HipFire.x / g.hipfireM.x, recoil_HipFire.y / g.hipfireM.y);
        range /= g.rangeM;
        magazineSize /= g.magazineSizeM;

        timeUntilFire = 60f / RPM;
        gunEffectsController.updateAnimatorSpeeds(reloadSpeed, RPM);
    }

    public void RemovePerkStatsAdditive(PerkGunStatsScript g)
    {
        damagePerProjectile -= mainGunStatsScript.DamagePerProjectile * (g.damagePerProjectileM - 1);
        RPM -= mainGunStatsScript.GetRPM * (g.RPMM - 1);
        reloadSpeed -= mainGunStatsScript.ReloadSpeed * (g.reloadSpeedM - 1);
        //recoil = recoil * g.recoilM;
        recoil -= new Vector2(mainGunStatsScript.Recoil.x * (g.recoilM.x - 1),
            mainGunStatsScript.Recoil.y * (g.recoilM.y - 1));
        recoil_HipFire -= new Vector2(mainGunStatsScript.Recoil_HipFire.x * (g.hipfireM.x - 1),
            mainGunStatsScript.Recoil_HipFire.y * (g.hipfireM.y - 1));
        range -= mainGunStatsScript.Range * (g.rangeM - 1);
        magazineSize -= mainGunStatsScript.MagazineSize * (g.magazineSizeM - 1);
        magazineSize -= mainGunStatsScript.MagazineSize * (g.magazineSizeM - 1);

        timeUntilFire = 60f / RPM;
        gunEffectsController.updateAnimatorSpeeds(reloadSpeed, RPM);
    }

    public virtual void AddAmmoToCurrentMag(int ammo)
    {
        currentMag += ammo;
    }

    protected virtual void UpdateRecoilDegree()
    {
        recoilDegree = Mathf.Clamp(currentRecoil.x * UniversalValues.HipFireRecoilMultiplier + recoil_HipFire.x, 0,
            recoil_HipFire.y);
    }
}