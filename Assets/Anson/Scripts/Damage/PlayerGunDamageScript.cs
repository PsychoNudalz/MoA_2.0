using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunDamageScript : GunDamageScript
{
    [Header("Player Specific ")]
    [SerializeField]
    protected bool pressedFire;

    [SerializeField]
    protected bool pressedADS;

    [SerializeField]
    protected Camera camera;

    [SerializeField]
    Look lookScript;

    [SerializeField]
    float originalFireDirection_X;

    public PlayerUIScript playerUIScript;




    private void Awake()
    {
        playerUIScript = FindObjectOfType<PlayerUIScript>();
        lookScript = FindObjectOfType<Look>();
        if (mainGunStatsScript != null)
        {
            UpdateGunScript(mainGunStatsScript);
        }
    }

    private void Update()
    {
        UpdateBehaviour();
    }

    protected override void UpdateBehaviour()
    {
        //base.UpdateBehaviour();
        tickTime();

        //CorrectRecoil();


        if (mainGunStatsScript && mainGunStatsScript.IsEquiped)
        {
            if (isFiring)
            {
                ShotData shotData = new ShotData();
                if (!Shoot(shotData))
                {
                    if (!isFullAuto)
                    {
                        CorrectRecoil();
                    }
                }

                ShotDataManager.Add(shotData);

            }
            else
            {
                CorrectRecoil();
            }

            if (mainGunStatsScript != null && !isAI)
            {
                SetWeaponRecoil();
                SetWeaponLocation();
            }
        }


        if (displayFireRaycast)
        {
            Vector3 fireDir = firePoint.transform.forward;
            Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.green, 0f);
        }
    }



    public override void UnequipOldGun()
    {
        HandController.ResetHands();
        ShotDataManager.Reset();
        if (mainGunStatsScript && mainGunStatsScript.GunComponentBody.GunHandController)
        {
            mainGunStatsScript.GunComponentBody.GunHandController.RemoveAllPoints_Left();

        }

        if (mainGunStatsScript)
        {
            mainGunStatsScript.IsEquiped = false;
            mainGunStatsScript.CurrentMag = currentMag;

        }

        base.UnequipOldGun();
        return;
    }

    public override bool Shoot(ShotData shotData = null)
    {
        bool temp = base.Shoot(shotData);
        if (shotData != null)
        {
            gunPerkController.OnShoot(shotData);
        }

        return temp;
    }

    public override void Fire(bool b)
    {
        base.Fire(b);
        if (isFullAuto)
        {
            lookScript.SetIsRecenter(!b, isFullAuto);
        }

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


    public override MainGunStatsScript UpdateGunScript(MainGunStatsScript g, int slot = -1)
    {
        bool wasADS = isADS;

        MainGunStatsScript oldGun = base.UpdateGunScript(g, slot);
        int[] temp = {LayerMask.NameToLayer("Debug")};
        AnsonUtility.ConvertLayerMask(g.gameObject, "PlayerGun", new List<int>(temp));


        if (mainGunStatsScript)
        {
            //print("Updating UI");
            UpdateAmmoCount();
            UpdateGunStatText();
            playerUIScript.SetGunName(mainGunStatsScript.GetName(), currentSlot);
            mainGunStatsScript.IsEquiped = true;
            //ansonTempUIScript.SetGunName(mainGunStatsScript.GetName(), mainGunStatsScript.ElementType, mainGunStatsScript.GunType, currentSlot);
        }

        if (wasADS)
        {
            lookScript.AimSight(wasADS, mainGunStatsScript.ComponentSight.ZoomMultiplier);
        }

        if (mainGunStatsScript && mainGunStatsScript.GunComponentBody.GunHandController)
        {
            HandController.left.AddPointer(mainGunStatsScript.GunComponentBody.GunHandController.HandRest);
        }
        
        

        return oldGun;

    }

    public void UpdateUI()
    {
        //print("Updating UI");
        UpdateAmmoCount();
        UpdateGunStatText();
        playerUIScript.SetGunName(mainGunStatsScript.GetName(), currentSlot);
    }

    public void PressFire(bool b)
    {
        pressedFire = b;
        if (!isFullAuto)
        {
            if (timeNow_TimeUnitlFire <= 0)
            {
                Fire(b);
            }
        }
        else
        {
            Fire(b);
        }
    }


    public void AdjustRecoil()
    {
        /*
        //print("Adjusting recoil ");
        float NewAimDir = (firePoint.transform.rotation.eulerAngles.x + 90) % 360;
        Vector3 localEular = transform.localRotation.eulerAngles;
        if (NewAimDir > originalFireDirection_X && isADS && isFullAuto)
        {
            currentRecoil.x = currentRecoil.x * 0.05f;
            //currentRecoil.y = currentRecoil.y * 0.005f;

            //transform.rotation = Quaternion.AngleAxis(firePoint.localEulerAngles.x + currentRecoil.x, transform.right) * transform.rotation;

            //firePoint.localRotation = Quaternion.Euler(-currentRecoil.x, currentRecoil.y, 0);

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
        */
    }

    protected override float HandleWeapon(float newRecoilTime = -1)
    {
        float temp = base.HandleWeapon(newRecoilTime);

        playerUIScript.FireCrossair();
        if (PlayerMasterScript.INFINITEAMMO)
        {
            currentMag = magazineSize;
        }

        UpdateAmmoCount();
        return temp;
    }

    protected override float RecoilWeapon(out Vector2 addRecoil)
    {
        float temp = base.RecoilWeapon(out addRecoil);
        lookScript.AddRecoil(addRecoil);
        return temp;
    }

    protected override Quaternion SetWeaponRecoil()
    {
        Quaternion targetPoint;
        /*
        if (firePoint.transform.rotation.eulerAngles.x > 180f && firePoint.transform.rotation.eulerAngles.x - currentRecoil.x < 275f)
        {
            currentRecoil.x = firePoint.transform.rotation.eulerAngles.x - 275f;
            currentRecoil.y = 0f;
        }
        */
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

        UpdateSights();
        return targetPoint;
    }

    void UpdateSights()
    {
        //set gun to forward
        mainGunStatsScript.transform.forward = firePoint.forward;

        //reset firepoint and gun to not rotate left and right
        firePoint.transform.rotation = Quaternion.Euler(firePoint.transform.rotation.eulerAngles.x,
            firePoint.transform.rotation.eulerAngles.y, 0f);
        //disabling this line doesnt change anything
        //mainGunStatsScript.transform.rotation = Quaternion.Euler(mainGunStatsScript.transform.rotation.eulerAngles.x, mainGunStatsScript.transform.rotation.eulerAngles.y, 0f);


        //WHY IS IT DOING THIS?????
        //to make sure the camera and the firepoint is the same
        //camera.transform.position = firePoint.position;
        //float rot_X = firePoint.transform.rotation.eulerAngles.x;
        //float rot_Y = firePoint.transform.rotation.eulerAngles.y;
        //camera.transform.rotation = Quaternion.Euler(rot_X, rot_Y, 0f);


    }

    public void PressADS(bool b)
    {
        pressedADS = b;
        if (b && !isReloading)
        {
            ADS_On();
        }
        else
        {
            ADS_Off();
        }
    }

    public void ADS_On()
    {
        if (mainGunStatsScript == null)
        {
            return;
        }

        UpdateSights();
        currentRecoil = new Vector2(0, 0);

        isADS = true;
        lookScript.AimSight(isADS, mainGunStatsScript.ComponentSight.ZoomMultiplier);
        playerUIScript.SetCrossair(true);
    }

    public void ADS_Off()
    {
        if (mainGunStatsScript == null)
        {
            return;
        }

        isADS = false;
        //transform.rotation = Quaternion.Euler(currentRecoil.x, currentRecoil.y, 0f) * transform.rotation;
        transform.rotation = mainGunStatsScript.transform.rotation;
        firePoint.transform.position = transform.position;
        firePoint.transform.rotation = transform.rotation;


        mainGunStatsScript.transform.rotation = gunPosition.transform.rotation;

        //camera.transform.position = transform.position;
        //camera.transform.rotation = transform.rotation;


        lookScript.AimSight(isADS, mainGunStatsScript.ComponentSight.ZoomMultiplier);

        playerUIScript.SetCrossair(false);
    }

    protected override IEnumerator DelayReload(float offset = 0)
    {

        Fire(false);
        gunEffectsController.PlayAnimationTrigger("Reload");
        gunEffectsController.PlaySound_StartReload();
        currentRecoilTime = 0f;
        AdjustRecoil();
        yield return new WaitForSeconds(reloadSpeed - offset);
        gunPerkController.OnPerReload();

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
                currentReloadCoroutine = StartCoroutine(DelayReload(0.05f));
                UpdateAmmoCount();

            }
            else
            {
                isReloading = false;
                gunEffectsController.PlaySound_EndReload();
                EndReload();

            }
        }
    }


    public override void EndReload()
    {
        base.EndReload();
        UpdateAmmoCount();
        //Reload
        if (pressedADS)
        {
            ADS_On();
        }

        PressFire(pressedFire);
        gunPerkController?.OnReload();
    }

    void UpdateAmmoCount()
    {
        try
        {
            playerUIScript.SetAmmoText(string.Concat(currentMag.ToString(), "/", magazineSize.ToString()), currentSlot);

        }
        catch (System.Exception e)
        {

        }
    }

    void UpdateGunStatText()
    {
        if (playerUIScript != null)
        {
            playerUIScript.SetGunText(mainGunStatsScript.ToString());
        }
    }

    public override void Reload()
    {
        base.Reload();
        Fire(false);
        ADS_Off();
        ShotDataManager.Reset();
    }





}
