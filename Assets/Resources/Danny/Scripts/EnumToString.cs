using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EnumToString
{
    public static string GetEnemyStringFromEnum(System.Enum enumForString)
    {
        switch (enumForString)
        {
            case EnemyType.StoneEnemy:
                return "Melee Enemy";
            case EnemyType.ShootingEnemy:
                return "Ranged Enemy";
            case EnemyType.TankEnemy:
                return "Tank Enemy";
            default:
                throw new System.Exception("Enemy type not found");
        }
    }
}
