using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalUIHandler : MonoBehaviour
{
    public void ConfirmOnClick() {
        FindObjectOfType<SaveManagerScript>().SaveProcedure();
        SceneManager.LoadScene("PrototypeLevel2");
    }


    public void ConfirmOnClick_Base()
    {
        FindObjectOfType<SaveManagerScript>().SaveProcedure();
        SceneManager.LoadScene("Base");
    }
}
