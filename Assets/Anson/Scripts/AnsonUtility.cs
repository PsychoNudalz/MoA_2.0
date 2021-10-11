using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AnsonUtility
{
    public static void ConvertLayerMask(GameObject currentGO, string layerName, List<int> ignoreLayers = null)
    {
        if (ignoreLayers != null)
        {
            if (ignoreLayers.Contains(currentGO.layer))
            {

            }
            else
            {
                currentGO.gameObject.layer = LayerMask.NameToLayer(layerName);

            }
        }
        else
        {
            currentGO.gameObject.layer = LayerMask.NameToLayer(layerName);

        }
        foreach (Transform child in currentGO.transform)
        {
            ConvertLayerMask(child.gameObject, layerName, ignoreLayers);


            /*
            if (!child.TryGetComponent(out GunComponent_Sight s))
            {
                convertWeaponLayerMask(child.gameObject, layerName);

            }
            */
        }
    }
}
