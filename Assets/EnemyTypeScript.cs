using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeScript : MonoBehaviour
{
    [SerializeField] EnemyTypeOld enemyTypeOld;

    internal EnemyTypeOld EnemyTypeOld { get => enemyTypeOld;}
}
