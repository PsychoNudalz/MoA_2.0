using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGunDamageScript : GunDamageScript
{
    [Header("Player Specific ")]
    [SerializeField] protected Camera camera;
    [SerializeField] Look lookScript;

    [SerializeField] float originalFireDirection_X;
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
        UpdateBehaviour();
    }
    protected override void UpdateBehaviour()
    {
        //base.UpdateBehaviour();
        tickTime();

        //CorrectRecoil();

        if (isFiring)
        {
            if (!Shoot())
            {
                if (!isFullAuto)
                {
                    CorrectRecoil();
                }
            }
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

        if (displayFireRaycast)
        {
            Vector3 fireDir = firePoint.transform.forward;
            Debug.DrawRay(firePoint.transform.position, fireDir * range, Color.green, 0f);
        }
    }

    public override MainGunStatsScript UpdateGunScript(MainGunStatsScript g, int slot = -1)
    {
        MainGunStatsScript newGun = base.UpdateGunScript(g, slot);
        if (newGun != null)
        {
            print("Updating UI");
            UpdateAmmoCount();
            UpdateGunStatText();
            ansonTempUIScript.SetGunName(mainGunStatsScript.GetName(), currentSlot);
            //ansonTempUIScript.SetGunName(mainGunStatsScript.GetName(), mainGunStatsScript.ElementType, mainGunStatsScript.GunType, currentSlot);
        }

        return newGun;

    }

    public void UpdateUI()
    {
        print("Updating UI");
        UpdateAmmoCount();
        UpdateGunStatText();
        ansonTempUIScript.SetGunName(mainGunStatsScript.GetName(), currentSlot);
    }

    public override void Fire(bool b)
    {
        base.Fire(b);
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

    protected override float HandleWeapon(float newRecoilTime = -1)
    {
        float temp = base.HandleWeapon(newRecoilTime);
        ansonTempUIScript.FireCrossair();
        UpdateAmmoCount();
        return temp;
    }

    protected override void SetWeaponRecoil()
    {
        base.SetWeaponRecoil();
        UpdateSights();

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

    public void ADS_On()
    {
        if (mainGunStatsScript == null) { return; }

        UpdateSights();
        currentRecoil = new Vector2(0, 0);

        isADS = true;
        lookScript.AimSight(isADS, mainGunStatsScript.Component_Sight.ZoomMultiplier);
        ansonTempUIScript.SetCrossair(true);
    }

    public void ADS_Off()
    {
        if (mainGunStatsScript == null) { return; }

        isADS = false;
        //transform.rotation = Quaternion.Euler(currentRecoil.x, currentRecoil.y, 0f) * transform.rotation;
        transform.rotation = mainGunStatsScript.transform.rotation;
        firePoint.transform.position = transform.position;
        firePoint.transform.rotation = transform.rotation;


        mainGunStatsScript.transform.rotation = gunPosition.transform.rotation;

        camera.transform.position = transform.position;
        camera.transform.rotation = transform.rotation;


        lookScript.AimSight(isADS, mainGunStatsScript.Component_Sight.ZoomMultiplier);
    
        ansonTempUIScript.SetCrossair(false);
    }

    protected override IEnumerator DelayReload(float offset = 0)
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
            EndReload();

        }
        else
        {
            currentMag += amountPerReload;
            mainGunStatsScript.Play_StartReload();
            if (currentMag < magazineSize)
            {
                currentReloadCoroutine = StartCoroutine(DelayReload(0.05f));
                UpdateAmmoCount();

            }
            else
            {
                isReloading = false;
                mainGunStatsScript.Play_EndReload();
                EndReload();

            }
        }
    }


    public override void EndReload()
    {
        base.EndReload();
        UpdateAmmoCount();

    }

    void UpdateAmmoCount()
    {
        try
        {
            ansonTempUIScript.SetAmmoText(string.Concat(currentMag.ToString(), "/", magazineSize.ToString()), currentSlot);

        }
        catch (System.Exception e)
        {

        }
    }
    void UpdateGunStatText()
    {
        if (ansonTempUIScript != null)
        {
            ansonTempUIScript.SetGunText(mainGunStatsScript.ToString());
        }
    }


}
