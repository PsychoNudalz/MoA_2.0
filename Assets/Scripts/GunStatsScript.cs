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
    [SerializeField] protected float range = 0;
    [SerializeField] protected float magazineSize = 0;

    public float DamagePerProjectile{ get => damagePerProjectile; }
    public float RPM_Get { get => RPM; }
    public float ReloadSpeed { get => reloadSpeed; }
    public Vector2 Recoil { get => recoil; }
    public float Range { get => range; }
    public float MagazineSize { get => magazineSize; }

    public virtual void AddStats(GunStatsScript g)
    {
        damagePerProjectile += g.damagePerProjectile;
        RPM += g.RPM;
        reloadSpeed += g.reloadSpeed;
        recoil += g.recoil;
        range += g.range;
        magazineSize += g.magazineSize;


    }

    public virtual void AddStats(ComponentGunStatsScript g)
    {
        AddStats(g as GunStatsScript);
        damagePerProjectile = damagePerProjectile * g.damagePerProjectileM;
        RPM = RPM * g.RPMM;
        reloadSpeed = reloadSpeed * g.reloadSpeedM;
        //recoil = recoil * g.recoilM;
        recoil = new Vector2(recoil.x * g.recoilM.x, recoil.y * g.recoilM.y);
        range = range * g.rangeM;
        magazineSize = magazineSize * g.magazineSizeM ;

    }


}
