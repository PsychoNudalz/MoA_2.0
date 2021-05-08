using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalUIHandler : MonoBehaviour
{
    public void ConfirmOnClick() {
        FindObjectOfType<SaveManagerScript>().SaveProcedure();
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if(loader != null)
        {
            loader.LoadWithLoadingScreen("PrototypeLevel2",true);
        }
        else
        {
            Debug.LogWarning("SceneLoader not found");
            SceneManager.LoadScene("PrototypeLevel2");
        }
    }


    public void ConfirmOnClick_Base()
    {
        FindObjectOfType<SaveManagerScript>().SaveProcedure();
        SceneLoader loader = FindObjectOfType<SceneLoader>();
        if (loader != null)
        {
            loader.LoadWithLoadingScreen("Base");
        }
        else
        {
            Debug.LogWarning("SceneLoader not found");
            SceneManager.LoadScene("Base");
        }
    }
}
