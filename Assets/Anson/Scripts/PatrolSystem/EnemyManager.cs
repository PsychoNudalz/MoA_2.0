using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Shooter_T1,
    Shooter_T2,
    Clower_T1,
    Clower_T2,
    Drone_T1,
    Drone_T2,
    Tank_T1,
    Tank_T2,
    Boss_T1
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    EnemyHandler Shooter_T1;

    [SerializeField]
    EnemyHandler Shooter_T2;

    [SerializeField]
    EnemyHandler Clower_T1;

    [SerializeField]
    EnemyHandler Clower_T2;

    [SerializeField]
    EnemyHandler Drone_T1;

    [SerializeField]
    EnemyHandler Drone_T2;

    [SerializeField]
    EnemyHandler Tank_T1;

    [SerializeField]
    EnemyHandler Tank_T2;

    [Space(10f)]
    [SerializeField]
    private EnemyHandler Boss_T1;


    public static EnemyManager current;

    public EnemyHandler GetEnemy(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Shooter_T1:
                return Shooter_T1;
                break;
            case EnemyType.Clower_T1:
                return Clower_T1;
                break;
            case EnemyType.Shooter_T2:
                return Shooter_T2;
                break;
            case EnemyType.Clower_T2:
                return Clower_T2;
                break;

            case EnemyType.Drone_T1:
                return Drone_T1;
                
                break;
            case EnemyType.Drone_T2:
                return Drone_T2;
                break;

            case EnemyType.Tank_T1:
                return Tank_T1;
                break;
            case EnemyType.Tank_T2:
                return Tank_T2;

                break;
            case EnemyType.Boss_T1:
                return Boss_T1;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(enemyType), enemyType, null);
        }
    }

    private void Awake()
    {
        if (!current)
        {
            current = this;
        }
    }

    public static EnemyHandler GetEnemyS(EnemyType enemyType)
    {
        if (!current)
        {
            current = FindObjectOfType<EnemyManager>();
        }

        return current.GetEnemy(enemyType);
    }
}