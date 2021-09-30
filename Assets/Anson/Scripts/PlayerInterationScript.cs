using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInterationScript : MonoBehaviour
{
    /// <summary>
    /// Anson:
    /// Allows the player to interact with Buttons
    /// </summary>
    [SerializeField] Camera cam1;
    [SerializeField] float pickUpRange = 1.5f;
    [SerializeField] InteractableScript currentFocus;
    //public bool useFlage = false;
    [SerializeField] LayerMask layerMask;
    [SerializeField] float updateRate = 0.2f;
    float lastUpdateTime;
    public AnsonTempUIScript ansonTempUIScript;


    public InteractableScript CurrentFocus { get => currentFocus; set => currentFocus = value; }

    // Start is called before the first frame update

    private void Awake()
    {
        ansonTempUIScript = FindObjectOfType<AnsonTempUIScript>();
        cam1 = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();

    }

    private void FixedUpdate()
    {
        if (Time.time - lastUpdateTime > updateRate)
        {
            CastDetectionRay();
            lastUpdateTime = Time.time;
        }
    }
    public void useInteractable()
    {
        if (currentFocus != null)
        {
            currentFocus.activate();
            //useFlage = true;
        }
    }

    public void ResetFlag()
    {
        //useFlage = false;
    }

    public void ClearInteractable()
    {
        setFocus(null);
    }


    public void setFocus(InteractableScript i)
    {
        if (currentFocus != null && currentFocus.TryGetComponent(out InteractableScript b))
        {
            if (i != null)
            {
                //Debug.Log("Player drop: " + i.name);
            }
            if (b is WeaponPickUpInteractableScript)
            {
                DisplayWeaponStats(false);
            }

        }
        currentFocus = i;
        if (currentFocus != null && currentFocus.TryGetComponent(out InteractableScript b2))
        {
            //Debug.Log("Player found: " + i.name);
            if (b2 is WeaponPickUpInteractableScript)
            {
                DisplayWeaponStats(true, (b2 as WeaponPickUpInteractableScript).ConnectedGun.ToString());
            }
        }
    }

    public void CastDetectionRay()
    {
        Debug.DrawRay(cam1.transform.position, cam1.transform.forward * pickUpRange, Color.cyan);
        RaycastHit hit;
        if (Physics.Raycast(cam1.transform.position, cam1.transform.forward, out hit, pickUpRange, layerMask))
        {
            //print("detected");
            InteractableScript i = hit.collider.GetComponentInParent<InteractableScript>();
            if (i != null)
            {
                setFocus(i);
            }
            else
            {
                //print("failed to get script on: " + hit.collider.name);
            }
        }
        else
        {
            if (currentFocus != null)
            {
                setFocus(null);
            }
        }
    }

    void DisplayWeaponStats(bool b, string s = "")
    {
        if (b)
        {
            ansonTempUIScript.DisplayNewGunText(b, s);
        }
        else
        {
            ansonTempUIScript.DisplayNewGunText(b);
        }
    }
}
