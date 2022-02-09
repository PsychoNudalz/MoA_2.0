using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PerkDisplayCall
{
    ADD,
    UPDATE,
    REMOVE
}
public class PerkDisplay : MonoBehaviour
{
    [Header("Perk Display")]
    [SerializeField]
    private List<PerkDisplayIcon> perkDisplayIcons = new List<PerkDisplayIcon>();
    [SerializeField]
    private List<Perk> perks = new List<Perk>();

    [ContextMenu("Awake")]
    private void Awake()
    {
        perkDisplayIcons = new List<PerkDisplayIcon>(GetComponentsInChildren<PerkDisplayIcon>());
        UpdateUI();
    }

    public void AddPerk(Perk p)
    {
        if (!perks.Contains(p))
        {
            perks.Add(p);
        }
        UpdateUI();
    }

    public void UpdatePerk(Perk p)
    {

        UpdateUI();
    }

    public void RemovePerk(Perk p)
    {
        if (perks.Contains(p))
        {
            perks.Remove(p);
        }
        UpdateUI();
    }
    

    public void UpdateUI()
    {
        int i = 0;
        foreach (Perk perk in perks)
        {
            if (i < perkDisplayIcons.Count)
            {
                PerkDisplayIcon perkDisplayIcon = perkDisplayIcons[i];
                if (!perkDisplayIcon.gameObject.activeSelf)
                {
                    perkDisplayIcon.gameObject.SetActive(true);
                }
                perkDisplayIcon.SetPerk(perk);
            }

            i++;
        }

        for (int j = i; j < perkDisplayIcons.Count; j++)
        {
            perkDisplayIcons[j].gameObject.SetActive(false);
        }
    }
}
