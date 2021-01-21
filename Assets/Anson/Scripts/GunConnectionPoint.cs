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
        bool compatable = false;
        if (compatableComponents.Contains(g.GetGunComponentType()))
        {
            print(g.name + " Compatable component");
            foreach(GunTypes gt in g.GetGunTypes())
            {
                if (compatableTypes.Contains(gt))
                {
                    compatable = true;
                    print(g.name + " Compatable type");
                }
            }
        }

        return compatable;
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
