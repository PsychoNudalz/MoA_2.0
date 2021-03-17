using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunAlterUIHandler : MonoBehaviour
{
    [SerializeField] GunManager gunManager;
    [SerializeField] GunGeneratorScript gunGeneratorScript;
    [SerializeField] GameObject BaseUIButton;
    [SerializeField] RectTransform scrollArea;
    //[SerializeField] RectTransform scrollView;
    [SerializeField] float buttonSize = 120f;
    [SerializeField] float offset = 300;
    [SerializeField] Vector2 buttonIndex = new Vector2();

    private void Start()
    {
        //Test button creation
        /*
        GunComponentUIButtonScript newButton = Instantiate(BaseUIButton, scrollArea).GetComponent<GunComponentUIButtonScript>();
        newButton.SetGCS(gunManager.AllGCSelections[0]);
        */
        //CreateNewButton(0);

        CreateAllButtons();
    }

    public void GenerateGun()
    {
        gunManager.GenerateGun();
    }


    void CreateAllButtons()
    {
        for (int i = 0; i < gunManager.AllGCSelections.Count; i++)
        {
            CreateNewButton(i);
        }
    }
    void CreateNewButton(int i)
    {
        buttonIndex = new Vector2(i % 3, i / 3);
        print("Scroll area: " + scrollArea.rect.height);
        GunComponentUIButtonScript newButton = Instantiate(BaseUIButton).GetComponent<GunComponentUIButtonScript>();
        // , new Vector3((scrollArea.rect.width/3f)*buttonIndex.x,-buttonSize* buttonIndex.y),Quaternion.identity,scrollArea);
        newButton.transform.SetParent(scrollArea, false);
        RectTransform temp = newButton.GetComponent<RectTransform>();
        temp.anchoredPosition = new Vector3(buttonSize * buttonIndex.x, -buttonSize * (buttonIndex.y+1)+ scrollArea.rect.height/2);
        newButton.SetGCS(gunManager.AllGCSelections[i]);
    }
}
