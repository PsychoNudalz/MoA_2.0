using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeakPointScript: MonoBehaviour
{
    [SerializeField] LifeSystemScript ls;

    public LifeSystemScript Ls { get => ls;}


    private void Awake()
    {
        if (ls == null)
        {
            ls = GetComponentInParent<TargetLifeSystem>();
        }
    }
}
