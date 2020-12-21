using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterDelay : MonoBehaviour
{
    public float duration;

    private void OnEnable()
    {
        Destroy(gameObject, duration);
    }
}
