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
    [SerializeField] protected float range = 0;
    [SerializeField] protected float magazineSize = 0;
    [SerializeField] protected int projectilePerShot;
    [SerializeField] protected float timeBetweenProjectile = 0f;
    [SerializeField] GunTypes gunType = GunTypes.RIFLE;
    [SerializeField] protected ElementTypes elementType = ElementTypes.PHYSICAL;
    [SerializeField] protected float elementDamage = 0;
    [SerializeField] protected float elementPotency = 0; //effect duration or range
    [SerializeField] protected float elementChance = 0;
    [SerializeField] FireTypes fireType = FireTypes.HitScan;
    [SerializeField] GameObject projectileGO;

    [SerializeField] protected AnimationCurve rangeCurve;
    [SerializeField] bool isFullAuto = true;
    [SerializeField] bool isFullReload = true;
    [SerializeField] int amountPerReload = 1;

    [SerializeField] AnimationCurve recoilPattern_X;
    [SerializeField] AnimationCurve recoilPattern_Y;
    [SerializeField] float timeToRecenter = 3f;


    [SerializeField] ParticleSystem bulletParticle;
    [SerializeField] GameObject impactEffect;
    [SerializeField] VisualEffect muzzleEffect;

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
    [SerializeField] Coroutine currentReloadCoroutine;
    [SerializeField] Look lookScript;
    [SerializeField] float originalFireDirection_X;

    [Header("Debug")]
    public bool displayFireRaycast = true;
    public bool isAI = false;
    public AnsonTempUIScript ansonTempUIScript;


    private void Awake()
    {
        ansonTempUIScript = FindObjectOfType<AnsonTempUIScript>();
        lookScript = FindObjectOfType<Look>();
        if (mainGunStatsScript != null)
        {
            UpdateGunScript(mainGunStatsScript);
        }
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
            //AdjustRecoil();
            CorrectRecoil();
        }
        if (mainGunStatsScript != null && !isAI)
        {
            SetWeaponRecoil();
            SetWeaponLocation();
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
        gunType = g.GunType;
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
        g.gameObject.transform.parent = transform;
        //g.transform.right = firePoint.forward;
        if (sightLocation == null)
        {
            sightLocation = transform;
        }
        sightOffset = sightLocation.position - gunPosition.position;
        mainGunStatsScript.transform.localRotation = Quaternion.Euler(0f, 0f, 0f);

        //HANDLE ELEMENTS.  Reduce main damage, change element damage
        elementDamage = Mathf.RoundToInt(g.ElementDamage * damagePerProjectile);
        damagePerProjectile = damagePerProjectile*0.85f;
        elementPotency = g.ElementPotency;
        elementChance = g.ElementChance;


        UpdateAmmoCount();
        UpdateGunStatText();



    }



    public void Fire(bool b)
    {
        isFiring = b;
        if (isFiring)
        {
            originalFireDirection_X = lookScript.YRotation_adjusted();
        }
        else
        {
            currentRecoilTime = currentRecoilTime / 2;
            AdjustRecoil();
        }

    }

    public void SetFirePoint(Transform t)
    {
        firePoint = t;
    }

    bool canFire()
    {
        if (isAI)
        {
            return true;
        }

        if (currentMag < 1 || (isReloading && isFullReload))
        {
            isFiring = false;
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
            currentRecoil = Vector2.Lerp(currentRecoil, new Vector2(0, 0), Time.deltaTime * 2 / timeToRecenter);

            /*
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
            }
            else if (currentRecoil.y > 0.05f)
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
            */

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


    public void AdjustRecoil()
    {


        //print("Adjusting recoil ");
        float NewAimDir = (firePoint.transform.rotation.eulerAngles.x + 90) % 360;
        Vector3 localEular = transform.localRotation.eulerAngles;
        if (NewAimDir > originalFireDirection_X && isADS && isFullAuto)
        {
            currentRecoil.x = currentRecoil.x * 0.05f;
            //currentRecoil.y = currentRecoil.y * 0.005f;

            transform.rotation = Quaternion.AngleAxis(firePoint.localEulerAngles.x + currentRecoil.x, transform.right) * transform.rotation;

            firePoint.localRotation = Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0);

            UpdateSights();
            SetWeaponLocation(true);

            //print("Remove recoil: ");

        }

        else if (lookScript.YRotation_adjusted() > originalFireDirection_X && isADS)
        {
            float differenceInAim = lookScript.YRotation_adjusted() - originalFireDirection_X;
            transform.rotation = Quaternion.AngleAxis(-differenceInAim, transform.right) * transform.rotation;
            firePoint.rotation = Quaternion.AngleAxis(differenceInAim, firePoint.right) * firePoint.rotation;
            //firePoint.localRotation = Quaternion.identity;


            currentRecoil.x -= differenceInAim;
            //print("Reduce recoil: " + differenceInAim);

        }
        else
        {
            //print("None");
            //print("None");
        }
    }




    public void Reload()
    {
        if (currentMag < magazineSize && !isReloading)
        {
            isReloading = true;
            currentReloadCoroutine = StartCoroutine(DelayReload());

        }
    }

    public bool Shoot()
    {
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
            mainGunStatsScript.PlayAnimationTrigger("Shoot", 1 / timeUntilFire);
            HandleWeapon();

            if (currentProjectile > 0 && currentMag > 0 && timeBetweenProjectile > 0)
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
        bool hitTarget = false;
        randomFireDir = new Vector2(Random.Range(0, currentRecoil.x), Random.Range(-180f, 180f));
        fireDir = Quaternion.AngleAxis(randomFireDir.y, firePoint.transform.forward) * Quaternion.AngleAxis(-randomFireDir.x, firePoint.transform.right) * firePoint.transform.forward;
        Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.blue, 1f);

        //Vector3 fireDir = firePoint.transform.forward;
        hitTarget = RayCastDealDamage(fireDir, hitTarget);


        if (gunType == GunTypes.SHOTGUN && projectilePerShot > 1)
        {
            //Shotgun Raycast
            for (int i = 0; i < projectilePerShot - 1; i++)
            {
                randomFireDir = new Vector2(Random.Range(recoil.x * 0.35f, recoil.x), (360f / (projectilePerShot - 1)) * i);
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
                randomFireDir = new Vector2(Random.Range(0, recoil.y), (360f / (projectilePerShot - 1)) * i);
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
                    dealCriticalDamageToTarget(ls, damagePerProjectile * dropOff, 1, elementType, 2f);
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
            for (int i = 0; i < projectilePerShot; i++)
            {
                projectileScript = Instantiate(projectileGO, mainGunStatsScript.transform.position, Quaternion.identity).GetComponent<ProjectileScript>();
                Vector3 fireDir = firePoint.forward;
                if (Physics.Raycast(firePoint.transform.position, firePoint.forward, out hit, range * 1.5f, layerMask))
                {
                    fireDir = hit.point - mainGunStatsScript.transform.position;
                }

                projectileScript.Launch(1, elementType, fireDir.normalized);
            }
        }
        else
        {
            Debug.LogError("Failed to get Projectile " + projectileGO.name + " script");
        }

    }

    void HandleWeapon()
    {
        mainGunStatsScript.Play_Fire();
        RecoilWeapon();
        currentProjectile -= 1;
        currentMag -= 1;

        UpdateAmmoCount();
        if (!isFullAuto)
        {
            Fire(false);
        }
    }

    void RecoilWeapon()
    {

        currentRecoilTime += 0.1f;
        currentRecoil += new Vector2(recoilPattern_X.Evaluate(currentRecoilTime) * recoil.x, recoilPattern_Y.Evaluate(currentRecoilTime) * recoil.y);
        if (gunType.Equals(GunTypes.SHOTGUN))
        {
            currentRecoil += new Vector2(recoilPattern_X.Evaluate(currentRecoilTime) * recoil.x, recoilPattern_Y.Evaluate(currentRecoilTime) * recoil.y) * projectilePerShot / 5f;

        }
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

            //mainGunStatsScript.transform.localRotation = Quaternion.Lerp(mainGunStatsScript.transform.localRotation, Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0), 10f * Time.deltaTime);
            mainGunStatsScript.transform.localRotation = Quaternion.Euler(0, 0, 0);

            if (firePoint.transform.rotation.eulerAngles.x > 180f && firePoint.transform.rotation.eulerAngles.x - currentRecoil.x < 275f)
            {
                currentRecoil.x = firePoint.transform.rotation.eulerAngles.x - 275f;
                currentRecoil.y = 0f;
            }

            Quaternion targetPoint = Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0);

            firePoint.transform.localRotation = Quaternion.Lerp(firePoint.transform.localRotation, targetPoint, 10f * Time.deltaTime);
            UpdateSights();
        }
        else
        {
            if (firePoint.transform.rotation.eulerAngles.x > 180f && firePoint.transform.rotation.eulerAngles.x - currentRecoil.x < 275f)
            {
                currentRecoil.x = firePoint.transform.rotation.eulerAngles.x - 275f;
                currentRecoil.y = 0f;
            }

            Quaternion targetPoint = Quaternion.Euler(-currentRecoil.x * .6f, currentRecoil.y * .2f, 0);
            mainGunStatsScript.transform.localRotation = Quaternion.Lerp(mainGunStatsScript.transform.localRotation, Quaternion.Euler(-currentRecoil.x * .6f, currentRecoil.y * .2f, 0), Time.deltaTime);



            firePoint.transform.localRotation = Quaternion.Lerp(firePoint.transform.localRotation, targetPoint, 10f * Time.deltaTime);
            UpdateSights();

        }

    }

    void SetWeaponLocation(bool forced = false)
    {
        if (isADS)
        {
            Vector3 targetPos = firePoint.transform.position - firePoint.transform.rotation * sightOffset;
            if (isFiring || forced)
            {
                mainGunStatsScript.transform.position = targetPos;

            }
            else
            {
                mainGunStatsScript.transform.position = Vector3.Lerp(mainGunStatsScript.transform.position, targetPos, 20 * Time.deltaTime);
            }

        }
        else
        {
            mainGunStatsScript.transform.position = Vector3.Lerp(mainGunStatsScript.transform.position, gunPosition.transform.position, 20 * Time.deltaTime);

        }
    }

    public void ADS_On()
    {

        UpdateSights();
        currentRecoil = new Vector2(0, 0);

        isADS = true;
        lookScript.AimSight(isADS, mainGunStatsScript.Component_Sight.ZoomMultiplier);

    }

    public void ADS_Off()
    {
        isADS = false;
        //transform.rotation = Quaternion.Euler(currentRecoil.x, currentRecoil.y, 0f) * transform.rotation;
        transform.rotation = mainGunStatsScript.transform.rotation;
        firePoint.transform.position = transform.position;
        firePoint.transform.rotation = transform.rotation;


        mainGunStatsScript.transform.rotation = gunPosition.transform.rotation;

        camera.transform.position = transform.position;
        camera.transform.rotation = transform.rotation;


        lookScript.AimSight(isADS, mainGunStatsScript.Component_Sight.ZoomMultiplier);
    }



    void ApplyElementEffect(LifeSystemScript ls)
    {
        ElementDebuffScript newDebuff;
        switch (elementType)
        {
            case (ElementTypes.PHYSICAL):
                break;
            case (ElementTypes.FIRE):
                newDebuff = new FireEffectScript(elementDamage, elementPotency);
                ls.ApplyDebuff(newDebuff as FireEffectScript);
                break;
            case (ElementTypes.ICE):
                break;
            case (ElementTypes.SHOCK):
                newDebuff = new ShockEffectScript(elementDamage, elementPotency, tagList, layerMask);
                ls.ApplyDebuff(newDebuff as ShockEffectScript);
                break;
        }
    }


    void UpdateSights()
    {
        mainGunStatsScript.transform.forward = firePoint.forward;
        firePoint.transform.rotation = Quaternion.Euler(firePoint.transform.rotation.eulerAngles.x, firePoint.transform.rotation.eulerAngles.y, 0f);
        mainGunStatsScript.transform.rotation = Quaternion.Euler(mainGunStatsScript.transform.rotation.eulerAngles.x, mainGunStatsScript.transform.rotation.eulerAngles.y, 0f);



        camera.transform.position = firePoint.position;
        float rot_X = firePoint.transform.rotation.eulerAngles.x;
        float rot_Y = firePoint.transform.rotation.eulerAngles.y;
        camera.transform.rotation = Quaternion.Euler(rot_X, rot_Y, 0f);

        if (isReloading)
        {
            // ADS_Off();
        }
    }

    void UpdateAmmoCount()
    {
        try
        {
            ansonTempUIScript.SetAmmoText(currentMag.ToString());

        }
        catch (System.Exception e)
        {

        }
    }


    IEnumerator BurstFire()
    {
        print("Bursting");
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
        HandleWeapon();
        if (currentProjectile > 0 && currentMag > 0)
        {
            StartCoroutine(BurstFire());
        }
    }

    IEnumerator DelayReload(float offset = 0)
    {
        isFiring = false;
        mainGunStatsScript.PlayAnimationTrigger("Reload", 1 / reloadSpeed);
        mainGunStatsScript.Play_StartReload();
        currentRecoilTime = 0f;
        AdjustRecoil();
        yield return new WaitForSeconds(reloadSpeed - offset);
        if (isFullReload)
        {
            mainGunStatsScript.Play_EndReload();
            currentMag = magazineSize;
            isReloading = false;


        }
        else
        {
            currentMag += amountPerReload;
            mainGunStatsScript.Play_StartReload();
            if (currentMag < magazineSize)
            {
                currentReloadCoroutine = StartCoroutine(DelayReload(0.05f));
            }
            else
            {
                isReloading = false;
                mainGunStatsScript.Play_EndReload();
                currentReloadCoroutine = null;
            }
        }
        UpdateAmmoCount();
    }



    void UpdateGunStatText()
    {
        if(ansonTempUIScript != null && !isAI)
        {
            ansonTempUIScript.SetGunText(mainGunStatsScript.ToString());
        }
    }


}
