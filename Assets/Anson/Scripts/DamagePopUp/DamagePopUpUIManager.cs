using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class DamagePopUpUIManager : MonoBehaviour
{
    public static DamagePopUpUIManager current;

    [SerializeField]
    List<DamagePopUpUIScript> DPPool;

    [SerializeField]
    DamagePopUpUIScript baseDP;

    [SerializeField]
    int pointer = 0;

    [SerializeField]
    int initialSize = 10;

    [Header("Text colours")]
    [SerializeField]
    Color normalColour = Color.white;

    [SerializeField]
    Color critColour = Color.red;

    [SerializeField]
    Color fireColour = new Color(255, 235, 0);

    [SerializeField]
    Color iceColour = Color.cyan;

    [SerializeField]
    Color shockColour = Color.yellow;

    [Header("Screen Size")]
    [SerializeField]
    private Vector2 screenSize;

    [SerializeField]
    private float margin=.05f;


    float spawnRange = .2f;

    float timeNow = 0;

    private void Awake()
    {
        current = this;
        screenSize = GetComponentInParent<CanvasScaler>().referenceResolution;
        DPPool = new List<DamagePopUpUIScript>();

        for (int i = 0; i < initialSize; i++)
        {
            DPPool.Add(Instantiate(baseDP, transform));
            DPPool[i].Initialise(screenSize,margin);
        }
    }

    private void FixedUpdate()
    {
        if (Time.time - timeNow > 10f)
        {
            CleanUpPool();
            timeNow = Time.time;
        }
    }

    public DamagePopUpUIScript displayDamage(string dmg, Color colour, Vector3 worldPos)
    {
        DamagePopUpUIScript currentDPUI = GetNextDPUI();
        currentDPUI.SetText(dmg, colour, worldPos + GetRandomPosition());
        return currentDPUI;
    }

    public DamagePopUpUIScript displayDamage(string dmg, ElementTypes e, Vector3 worldPos)
    {
        return displayDamage(dmg, GetElementToColour(e), worldPos);
    }

    DamagePopUpUIScript GetNextDPUI()
    {
        int i = 0;
        pointer = (pointer + 1) % DPPool.Count;

        DamagePopUpUIScript currentDP = DPPool[pointer];
        while (i < DPPool.Count && currentDP.gameObject.activeSelf)
        {
            pointer = (pointer + 1) % DPPool.Count;
            currentDP = DPPool[pointer];
            i++;
        }

        if (currentDP.gameObject.activeSelf)
        {
            currentDP = Instantiate(baseDP, transform);
            DPPool.Add(currentDP);
        }

        return currentDP;
    }

    void CleanUpPool()
    {
        if (DPPool.Count > initialSize)
        {
            // print(this + " loading clean up");
            int i = 0;
            DamagePopUpUIScript currentDP;
            while (i < DPPool.Count && DPPool.Count > initialSize)
            {
                currentDP = DPPool[i];
                i++;
                if (!currentDP.gameObject.activeSelf)
                {
                    //print(this + " clearing " + i);
                    DPPool.Remove(currentDP);
                    Destroy(currentDP.gameObject);
                    i--;
                }
            }
        }
    }


    public virtual Color GetElementToColour(ElementTypes e)
    {
        switch (e)
        {
            case ElementTypes.PHYSICAL:
                return normalColour;
                break;
            case ElementTypes.FIRE:
                return fireColour;

                break;
            case ElementTypes.ICE:
                return iceColour;

                break;
            case ElementTypes.SHOCK:
                return shockColour;

                break;
            case ElementTypes.CRIT:
                return critColour;

                break;
            default:
                return normalColour;
                break;
        }
    }

    protected Vector3 GetRandomPosition()
    {
        return new Vector3(Random.Range(-spawnRange, spawnRange),
            Random.Range(-spawnRange, spawnRange) + (Time.time % spawnRange), Random.Range(-spawnRange, spawnRange));
    }
}