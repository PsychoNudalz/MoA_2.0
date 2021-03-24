using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PortalUIHandler : MonoBehaviour
{
    public void ConfirmOnClick() {
        SceneManager.LoadScene("PrototypeLevel2");
    }

}
