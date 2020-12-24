using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GunConnectionPoint : MonoBehaviour
{
    [SerializeField] GunComponent connectedComponent;
    [SerializeField] List<GunComponents> compatableComponents;
    [SerializeField] List<GunTypes> compatableTypes;


    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Quaternion GetQuaternion()
    {
        return transform.rotation;
    }

    public bool IsCompatable(GunComponent g)
    {
        if (compatableComponents.Contains(g.GetGunComponentType()))
        {
            foreach(GunTypes gt in g.GetGunTypes())
            {
                if (compatableTypes.Contains(gt))
                {
                    return true;
                }
            }
        }

        return false;
    }

    public bool SetComponent(GunComponent g)
    {
        if (IsCompatable(g))
        {
            connectedComponent = g;
            return true;
        }
        return false;

    }

    public List<GunComponents> GetGunComponents()
    {
        return new List<GunComponents>(compatableComponents);
    }

}
