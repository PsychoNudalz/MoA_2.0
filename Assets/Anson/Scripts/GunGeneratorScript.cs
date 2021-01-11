using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunGeneratorScript : MonoBehaviour
{
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
        AddRandomComponents(newGun);

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


        }

        return returnList;
    }

    void AddRandomComponents(GunComponent gunComponent)
    {
        foreach (GunConnectionPoint currentConnection in gunComponent.GetGunConnectionPoints())
        {
            List<GunComponent> possibleComponents = new List<GunComponent>();
            foreach (GunComponents compatableComp in currentConnection.GetGunComponents())
            {
                possibleComponents.AddRange(GetComponentList(compatableComp));
            }
            Debug.Log("Found " + possibleComponents.Count + " possible components for " + gunComponent.name + " " + currentConnection.name);
            if (possibleComponents.Count > 0)
            {
                GunComponent currentRandomComponent = possibleComponents[Mathf.RoundToInt(Random.Range(0, possibleComponents.Count))];
                while (!currentConnection.SetComponent(currentRandomComponent) && possibleComponents.Count > 1)
                {
                    print(currentRandomComponent.name + "  incompatable");
                    possibleComponents.Remove(currentRandomComponent);
                    currentRandomComponent = possibleComponents[Mathf.RoundToInt(Random.Range(0, possibleComponents.Count))];
                }
                GunComponent newComponent = Instantiate(currentRandomComponent, currentConnection.GetPosition(), currentConnection.GetQuaternion(), currentConnection.transform);
                currentMainGunStatsScript.AddStats(newComponent.GetComponent<ComponentGunStatsScript>());
                if (newComponent.GetGunComponentType().Equals(GunComponents.SIGHT))
                {
                    SetSight(newComponent.GetComponent<GunComponent_Sight>());
                } else if (newComponent.GetGunComponentType().Equals(GunComponents.MUZZLE))
                {
                    SetMuzzle(newComponent.GetComponent<GunComponent_Muzzle>());
                }
                else if (newComponent.GetGunComponentType().Equals(GunComponents.MAGAZINE))
                {
                    SetProjectile(newComponent.GetComponent<GunComponent_Magazine>().Projectile);
                }
                AddRandomComponents(newComponent);
            }
        }
    }

    void SetSight(GunComponent_Sight s)
    {
        print("Detect sight");
        newGun.SetSight(s);
    }

    void SetMuzzle(GunComponent_Muzzle m)
    {
        print("Detect muzzle");
        if (m.MuzzleLocation != null)
        {
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
