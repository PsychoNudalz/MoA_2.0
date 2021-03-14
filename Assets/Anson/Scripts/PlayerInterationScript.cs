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
    public bool useFlage = false;
    [SerializeField] LayerMask layerMask;
 
    public InteractableScript CurrentFocus { get => currentFocus; set => currentFocus = value; }

    // Start is called before the first frame update

    private void FixedUpdate()
    {
        CastDetectionRay();
    }
    public void useInteractable()
    {
        if (currentFocus != null && !useFlage)
        {
            currentFocus.activate();
            useFlage = true;
        }
    }

    public void ResetFlag()
    {
        useFlage = false;
    }



    public void setFocus(InteractableScript i)
    {
        if (currentFocus != null && currentFocus.TryGetComponent(out InteractableScript b))
        {
            Debug.Log("Player drop: " + i.name);

        }
        currentFocus = i;
        if (currentFocus != null && currentFocus.TryGetComponent(out InteractableScript b2))
        {
            Debug.Log("Player found: " + i.name);
        }
    }

    public void CastDetectionRay()
    {
        Debug.DrawRay(cam1.transform.position, cam1.transform.forward * pickUpRange, Color.cyan);
        RaycastHit hit;
        if (Physics.Raycast(cam1.transform.position, cam1.transform.forward,out hit,pickUpRange, layerMask))
        {
            print("detected");
            InteractableScript i = GetComponentInParent<InteractableScript>();
            if (i != null)
            {
                setFocus(i);
            }
            else
            {
                print("failed to get script");
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
}
