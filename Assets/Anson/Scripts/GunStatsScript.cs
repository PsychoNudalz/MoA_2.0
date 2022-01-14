using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunStatsScript : MonoBehaviour
{
    [Header("Gun Stats")]
    [SerializeField] protected float damagePerProjectile = 0;
    [SerializeField] protected float RPM = 0;
    [SerializeField] protected float reloadSpeed = 0;
    [SerializeField] protected Vector2 recoil = new Vector2(0, 0);
    [SerializeField] protected Vector2 recoil_HipFire = new Vector2(0, 0);
    [SerializeField] protected float range = 0;
    [SerializeField] protected float magazineSize = 0;

    [Header("Elemental Stats")]
    [Range(0f, 2f)]
    [SerializeField] protected float elementDamage = 0;
    [SerializeField] protected float elementPotency = 0; //effect duration or range
    [Range(0f, 1f)]
    [SerializeField] protected float elementChance = 0;

    public float DamagePerProjectile{ get => damagePerProjectile; }
    public float GetRPM { get => RPM; }
    public float ReloadSpeed { get => reloadSpeed; }
    public Vector2 Recoil { get => recoil; }
    public Vector2 Recoil_HipFire { get => recoil_HipFire; }
    public float Range { get => range; }
    public float MagazineSize { get => magazineSize; }
    public float ElementDamage { get => elementDamage; }
    public float ElementPotency { get => elementPotency; }
    public float ElementChance { get => elementChance; }


    public virtual void AddStats(GunStatsScript g)
    {
        damagePerProjectile += g.damagePerProjectile;
        RPM += g.RPM;
        reloadSpeed += g.reloadSpeed;
        recoil += g.recoil;
        recoil_HipFire += g.recoil_HipFire;
        range += g.range;
        magazineSize += g.magazineSize;
        elementDamage += g.elementDamage;
        elementPotency += g.ElementPotency;
        elementChance += g.ElementChance;


    }

    public virtual void AddStats(ComponentGunStatsScript g)
    {
        AddStats(g as GunStatsScript);
        damagePerProjectile = damagePerProjectile * g.damagePerProjectileM;
        RPM = RPM * g.RPMM;
        reloadSpeed = reloadSpeed * g.reloadSpeedM;
        //recoil = recoil * g.recoilM;
        recoil = new Vector2(recoil.x * g.recoilM.x, recoil.y * g.recoilM.y);
        recoil_HipFire = new Vector2(recoil_HipFire.x * g.recoilM.x, recoil_HipFire.y * g.recoilM.y);
        range = range * g.rangeM;
        magazineSize = magazineSize * g.magazineSizeM ;

    }

    public List<string> GetStatsStrings()
    {
        List<string> returnList = new List<string>();
        returnList.Add("DMG:"+ damagePerProjectile.ToString());
        returnList.Add("RPM:" + RPM.ToString());
        returnList.Add("Reload:" + reloadSpeed.ToString()+"Sec");
        returnList.Add("Stability:"+recoil.ToString());
        returnList.Add("Accuracy:"+recoil_HipFire.ToString());
        returnList.Add("Range:"+range.ToString()+"m");
        returnList.Add("Mag Size:"+magazineSize.ToString());

        return returnList;

    }

    public List<string> GetElementalStrings()
    {
        List<string> returnList = new List<string>();
        returnList.Add("E. DMG:"+elementDamage.ToString());
        returnList.Add("E. Potency:" + elementPotency.ToString());
        returnList.Add("E. Chance:" + elementChance.ToString());

        return returnList;

    }


}
