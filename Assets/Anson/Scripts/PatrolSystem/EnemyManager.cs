using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType
{
    Ground_Range_Mobile_T_IE,
    Ground_Floor_Mobile_T_IE,
    Ground_Range_Mobile_T_IN,
    Ground_Floor_Mobile_T_IN,
    Ground_Range_Mobile_T_IH,
    Ground_Floor_Mobile_T_IH
}

public class EnemyManager : MonoBehaviour
{
    [SerializeField]
    EnemyHandler Ground_Range_Mobile_T_IE;

    [SerializeField]
    EnemyHandler Ground_Floor_Mobile_T_IE;

    [SerializeField]
    EnemyHandler Ground_Range_Mobile_T_IN;

    [SerializeField]
    EnemyHandler Ground_Floor_Mobile_T_IN;

    [SerializeField]
    EnemyHandler Ground_Range_Mobile_T_IH;

    [SerializeField]
    EnemyHandler Ground_Floor_Mobile_T_IH;

    public static EnemyManager current;

    public EnemyHandler GetEnemy(EnemyType enemyType)
    {
        switch (enemyType)
        {
            case EnemyType.Ground_Range_Mobile_T_IE:
                return Ground_Range_Mobile_T_IE;
                break;
            case EnemyType.Ground_Floor_Mobile_T_IE:
                return Ground_Floor_Mobile_T_IE;
                break;
            case EnemyType.Ground_Range_Mobile_T_IN:
                return Ground_Range_Mobile_T_IN;
                break;
            case EnemyType.Ground_Floor_Mobile_T_IN:
                return Ground_Floor_Mobile_T_IN;
                break;
            
            case EnemyType.Ground_Range_Mobile_T_IH:
                return Ground_Range_Mobile_T_IH;
                break;
            case EnemyType.Ground_Floor_Mobile_T_IH:
                return Ground_Floor_Mobile_T_IH;
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