using System.Collections;
using System.Collections.Generic;
using Mono.CSharp;
using UnityEngine;

[System.Serializable]
public class GunConnectionPoint : MonoBehaviour
{
    [SerializeField] GunComponent connectedComponent;
    [SerializeField] List<GunComponents> compatableComponents;
    [SerializeField] List<GunTypes> compatableTypes;

    [SerializeField]
    private bool showComponent = true;

    public GunComponent ConnectedComponent { get => connectedComponent; set => connectedComponent = value; }

    public List<GunComponents> CompatableComponents
    {
        get => compatableComponents;
        set => compatableComponents = value;
    }

    public List<GunTypes> CompatableTypes
    {
        get => compatableTypes;
        set => compatableTypes = value;
    }

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
        if (g is GunComponent_Perk)
        {
            GunComponent_Perk gp = g as GunComponent_Perk;
            if (!gp.IsStackable)
            {
                Perk[] perks = GetComponentInParent<GunPerkController>().Perks;
                foreach (Perk perk in perks)
                {
                    // print($"{perk.GetType()} , {gp.Perk.GetType()}");
                    if (perk.GetType().IsInstanceOfType(gp.Perk))
                    {
                        return false;
                    }
                }
            }
        }
        
        if (compatableComponents.Contains(g.GetGunComponentType()))
        {
            //print(g.name + " Compatable component");
            foreach(GunTypes gt in g.GetGunTypes())
            {
                if (compatableTypes.Contains(gt))
                {
                    compatable = true;
                    //print(g.name + " Compatable type");
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
            if (!showComponent)
            {
                foreach (Renderer r in connectedComponent.GetComponentsInChildren<Renderer>())
                {
                    r.enabled = false;
                }
            }

            return true;
        }
        return false;

    }

    public List<GunComponents> GetGunComponents()
    {
        return new List<GunComponents>(compatableComponents);
    }

    public bool HasComponent()
    {
        return connectedComponent != null;
    }

}
