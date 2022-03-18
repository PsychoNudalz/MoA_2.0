using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumToString
{
    public static string GetEnemyStringFromEnum(System.Enum enumForString)
    {
        switch (enumForString)
        {
            case EnemyTypeOld.StoneEnemy:
                return "Melee Enemy";
            case EnemyTypeOld.ShootingEnemy:
                return "Ranged Enemy";
            case EnemyTypeOld.TankEnemy:
                return "Tank Enemy";
            case EnemyTypeOld.BossEnemy:
                return "Boss Enemy";
            default:
                throw new System.Exception("Enemy type not found");
        }
    }
}
