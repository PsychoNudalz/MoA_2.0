using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyTypeScript : MonoBehaviour
{
    [SerializeField] EnemyType enemyType;

    internal EnemyType EnemyType { get => enemyType;}
}
