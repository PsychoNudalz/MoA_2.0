using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGeneratorScript : MonoBehaviour
{
    [Header("Generator controls")]
    [SerializeField] bool randomRarity = false;

    [Header("Component Lists")]
    //public List<GunComponent> allGunComponents;
    [SerializeField] List<GunComponent_Body> components_Body;
    [SerializeField] List<GunComponent_Grip> components_Grip;
    [SerializeField] List<GunComponent_Stock> components_Stock;
    [SerializeField] List<GunComponent_Magazine> components_Magazine;
    [SerializeField] List<GunComponent_Barrel> components_Barrel;
    [SerializeField] List<GunComponent_Sight> components_Sight;
    [SerializeField] List<GunComponent_Muzzle> components_Muzzle;
    [SerializeField] List<GunComponent_Attachment> components_Attachment;
    [SerializeField] List<GunComponent_StatBoost> components_StatBoost;


    [Header("Other Components")]
    [SerializeField] GunComponent_Body newGun;
    [SerializeField] GameObject emptyGunGO;
    [SerializeField] MainGunStatsScript currentMainGunStatsScript;


    public GameObject GenerateGun()
    {
        GameObject newEmptyGun = Instantiate(emptyGunGO, transform.position, transform.rotation);
        currentMainGunStatsScript = newEmptyGun.GetComponent<MainGunStatsScript>();

        newGun = Instantiate(components_Body[Mathf.RoundToInt(Random.Range(0, components_Body.Count))], transform.position, Quaternion.Euler(0, -90, 0) * newEmptyGun.transform.rotation, newEmptyGun.transform);
        currentMainGunStatsScript.AddStats(newGun.GetComponent<ComponentGunStatsScript>());

        if (randomRarity)
        {
            newGun.Rarity = RandomiseRarity();
        }

        AddRandomEssentialComponents(newGun);
        AddRandomExtraComponents(newGun);

        currentMainGunStatsScript.SetBody(newGun);
        currentMainGunStatsScript.FinishAssemply();

        //newEmptyGun.name = newGun.name;

        Cursor.visible = false;


        return newEmptyGun;


    }

    List<GunComponent> GetComponentList(GunComponents gunComponents)
    {

        List<GunComponent> returnList = null;
        switch (gunComponents)
        {
            case (GunComponents.BODY):
                returnList = new List<GunComponent>(components_Body);
                break;
            case (GunComponents.GRIP):
                returnList = new List<GunComponent>(components_Grip);
                break;
            case (GunComponents.MAGAZINE):
                returnList = new List<GunComponent>(components_Magazine);
                break;
            case (GunComponents.BARREL):
                returnList = new List<GunComponent>(components_Barrel);
                break;
            case (GunComponents.STOCK):
                returnList = new List<GunComponent>(components_Stock);
                break;
            case (GunComponents.SIGHT):
                returnList = new List<GunComponent>(components_Sight);
                break;
            case (GunComponents.ATTACHMENT):
                returnList = new List<GunComponent>(components_Attachment);
                break;
            case (GunComponents.MUZZLE):
                returnList = new List<GunComponent>(components_Muzzle);
                break;
            case (GunComponents.STATBOOST):
                returnList = new List<GunComponent>(components_StatBoost);
                break;


        }

        return returnList;
    }

    void AddRandomComponents(List<GunConnectionPoint> connectionList, bool isExtra = false)
    {
        foreach (GunConnectionPoint currentConnection in connectionList)
        {
            //Getting all compatable components
            List<GunComponent> possibleComponents = new List<GunComponent>();
            foreach (GunComponents compatableComp in currentConnection.GetGunComponents())
            {
                possibleComponents.AddRange(GetComponentList(compatableComp));
            }
            Debug.Log("Found " + possibleComponents.Count + " possible components for " + currentConnection.name);

            //Start assigning if possible
            if (possibleComponents.Count > 0)
            {
                GunComponent currentRandomComponent = possibleComponents[Mathf.RoundToInt(Random.Range(0, possibleComponents.Count))];
                while (!currentConnection.IsCompatable(currentRandomComponent) && possibleComponents.Count > 1)
                {
                    print(currentRandomComponent.name + "  incompatable");
                    possibleComponents.Remove(currentRandomComponent);
                    currentRandomComponent = possibleComponents[Mathf.RoundToInt(Random.Range(0, possibleComponents.Count))];
                }
                GunComponent newComponent;
                newComponent = Instantiate(currentRandomComponent);
                currentConnection.SetComponent(newComponent);
                newComponent.transform.SetParent(currentConnection.transform);
                newComponent.transform.position = currentConnection.GetPosition();
                newComponent.transform.rotation = currentConnection.GetQuaternion();


                Debug.Log("Happened with: " + currentConnection + " and " + currentRandomComponent);
                currentMainGunStatsScript.AddStats(newComponent.GetComponent<ComponentGunStatsScript>());
                if (newComponent.GetGunComponentType().Equals(GunComponents.SIGHT))
                {
                    SetSight(newComponent.GetComponent<GunComponent_Sight>());
                }
                else if (newComponent.GetGunComponentType().Equals(GunComponents.BARREL))
                {
                    SetMuzzlePoint(newComponent.GetComponent<GunComponent_Barrel>());
                }
                else if (newComponent.GetGunComponentType().Equals(GunComponents.MUZZLE))
                {
                    SetMuzzlePoint(newComponent.GetComponent<GunComponent_Muzzle>());
                }
                else if (newComponent.GetGunComponentType().Equals(GunComponents.MAGAZINE))
                {
                    SetProjectile(newComponent.GetComponent<GunComponent_Magazine>().Projectile);
                }
                if (!isExtra)
                {

                    AddRandomComponents(newComponent.EssentialConnectionPoints);
                }
            }
        }
    }

    void AddRandomEssentialComponents(GunComponent gunComponent)
    {
        AddRandomComponents(newGun.EssentialConnectionPoints);

    }


    void AddRandomExtraComponents(GunComponent_Body gunComponent)
    {
        List<GunConnectionPoint> allConnections = GetExtraConnections(gunComponent);
        int numberOfExtras = RarityAmount(gunComponent.Rarity, allConnections.Count);
        List<GunConnectionPoint> connections = new List<GunConnectionPoint>();
        int pointer;
        for (int i = 0; i < numberOfExtras && allConnections.Count > 0; i++)
        {
            pointer = Random.Range(0, allConnections.Count);
            connections.Add(allConnections[pointer]);
            allConnections.Remove(allConnections[pointer]);
        }
        print("Found " + allConnections.Count + " extra connections on " + gunComponent.name);
        print("Pass " + connections.Count + " extra connections on " + gunComponent.name);
        AddRandomComponents(connections, true);
    }


    int RarityAmount(Rarity rarity, float amount)
    {
        float maxAmount = 0;
        switch (rarity)
        {
            case Rarity.UNCOMMON:
                maxAmount = 2;
                break;
            case Rarity.RARE:
                maxAmount = 3;
                break;
            case Rarity.LEGENDARY:
                maxAmount = 5;
                break;
            case Rarity.EXOTIC:
                maxAmount = amount;
                break;
        }
        return Mathf.RoundToInt(Mathf.Clamp(amount * ((int)rarity / 4f), 0, maxAmount));
    }

    Rarity RandomiseRarity()
    {
        return (Rarity)Random.Range(0, 4);
    }

    List<GunConnectionPoint> GetExtraConnections(GunComponent gunComponent)
    {
        List<GunConnectionPoint> returnList = new List<GunConnectionPoint>();
        try
        {
            if (gunComponent.EssentialConnectionPoints == null)
            {
                return returnList;
            }
        }
        catch (System.NullReferenceException)
        {
            return returnList;

        }
        foreach (GunConnectionPoint cp in gunComponent.EssentialConnectionPoints)
        {
            returnList.AddRange(GetExtraConnections(cp.ConnectedComponent));
        }
        returnList.AddRange(gunComponent.ExtraConnectionPoints);
        return returnList;
    }

    void SetSight(GunComponent_Sight s)
    {
        print("Detect sight");
        newGun.SetSight(s);
    }

    void SetMuzzlePoint(GunComponent_Muzzle m)
    {
        print("Detect muzzle");
        if (m.MuzzleLocation != null)
        {
            print("Found Muzzle Location");
            newGun.SetMuzzle(m.MuzzleLocation);
        }
        else
        {
            newGun.SetMuzzle(m.transform);
        }
    }

    void SetMuzzlePoint(GunComponent_Barrel m)
    {
        print("Detect muzzle");
        if (m.MuzzleLocation != null)
        {
            print("Found Muzzle Location");
            newGun.SetMuzzle(m.MuzzleLocation);
        }
        else
        {
            newGun.SetMuzzle(m.transform);
        }
    }

    void SetProjectile(GameObject g)
    {
        newGun.SetProjectile(g);
    }
}
