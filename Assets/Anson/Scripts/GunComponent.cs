using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class GunComponent : MonoBehaviour
{
    [SerializeField] private GunComponents componentType = GunComponents.ATTACHMENT;
    [SerializeField] private List<GunTypes> gunTypes;
    //[SerializeField] protected List<GunComponent> connectedComponents;
    [SerializeField] protected List<GunConnectionPoint> essentialConnectionPoints = new List<GunConnectionPoint>();
    [SerializeField] protected List<GunConnectionPoint> extraConnectionPoints = new List<GunConnectionPoint>();
    [SerializeField] protected ComponentGunStatsScript componentGunStatsScript;
    [SerializeField] private int componentCost = 1;
    public GunComponents ComponentType { get => componentType; }
    public List<GunTypes> GTypes { get => gunTypes; }
    public List<GunConnectionPoint> EssentialConnectionPoints { get => essentialConnectionPoints; set => essentialConnectionPoints = value; }
    public List<GunConnectionPoint> ExtraConnectionPoints { get => extraConnectionPoints; set => extraConnectionPoints = value; }
    public int ComponentCost { get => componentCost; set => componentCost = value; }

    private void Awake()
    {
        if (componentGunStatsScript == null)
        {
            componentGunStatsScript = GetComponent<ComponentGunStatsScript>();

        }
    }


    public GunComponents GetGunComponentType()
    {
        return ComponentType;
    }

    public List<GunTypes> GetGunTypes()
    {
        return new List<GunTypes>(GTypes);
    }

    public virtual List<List<string>> GetStats()
    {
        List<string> statsStrings = new List<string>(componentGunStatsScript.GetStatsStrings());
        List<string> elementalStrings = new List<string>(componentGunStatsScript.GetElementalStrings());
        List<string> multiplierStrings = new List<string>(componentGunStatsScript.GetMultiplierStrings());
        /*
        foreach (string s1 in componentGunStatsScript.GetStatsStrings())
        {
            statsStrings.Add(s1);

        }
        foreach (string s1 in componentGunStatsScript.GetElementalStrings())
        {
            elementalStrings.Add(s1);
        }
        foreach (string s1 in componentGunStatsScript.GetMultiplierStrings())
        {
            if (!(s1.Equals("") || s1.Equals("1") || s1.Equals("1,1")))
            {
                multiplierStrings.Add(s1);
            }
        }
        */

        List<List<string>> returnList = new List<List<string>>();
        returnList.Add(statsStrings);
        returnList.Add(elementalStrings);
        returnList.Add(multiplierStrings);
        return returnList;
    }


}
