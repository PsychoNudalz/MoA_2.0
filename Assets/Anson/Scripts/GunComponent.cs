using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GunComponent : MonoBehaviour
{
    [SerializeField] private GunComponents componentType = GunComponents.ATTACHMENT;
    [SerializeField] private List<GunTypes> gunTypes;
    //[SerializeField] protected List<GunComponent> connectedComponents;
    [SerializeField] protected List<GunConnectionPoint> essentialConnectionPoints = new List<GunConnectionPoint>();
    [SerializeField] protected List<GunConnectionPoint> extraConnectionPoints = new List<GunConnectionPoint>();
    [SerializeField] protected ComponentGunStatsScript componentGunStatsScript;

    public GunComponents ComponentType { get => componentType;}
    public List<GunTypes> GTypes { get => gunTypes;}
    public List<GunConnectionPoint> EssentialConnectionPoints { get => essentialConnectionPoints; set => essentialConnectionPoints = value; }
    public List<GunConnectionPoint> ExtraConnectionPoints { get => extraConnectionPoints; set => extraConnectionPoints = value; }

    private void Awake()
    {
        componentGunStatsScript = GetComponent<ComponentGunStatsScript>();
    }

    public GunComponents GetGunComponentType()
    {
        return ComponentType;
    }

    public List<GunTypes> GetGunTypes()
    {
        return new List<GunTypes>(GTypes);
    }


}
