using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandController : MonoBehaviour
{

    [Header("Hand")]
    [SerializeField] Transform handTransform;
    [SerializeField] Transform targetTransform;
    [SerializeField] Vector3 targetPosition;
    [SerializeField] Quaternion targetRotation;
    [SerializeField] List<HandPositionPointer> pointers;

    [Header("Settings")]
    [SerializeField] bool rightHand;
    [SerializeField] float moveSpeed = 2f;
    [SerializeField] float transitionSpeed = 1f;
    [SerializeField] float positionDeadZone = 0.05f;
    [SerializeField] float rotationDeadZone = 0.05f;


    [Header("Model")]
    [SerializeField] Transform originalFinger;
    [SerializeField] Transform targetFinger;
    [Range(0f,1f)]
    [SerializeField] float range = 0;


    [Header("Debug")]
    [SerializeField] bool showDebug;
    [SerializeField] Transform targetVisualiseTransform;
    [SerializeField] float debugSize = .25f;


    public static HandController left;
    public static HandController right;

    Transform original;
    //[SerializeField] Vector3 debugSize = new Vector3(0.5f, 0.5f, 0.5f);

    // Start is called before the first frame update
    void Awake()
    {
        if (!handTransform)
        {
            handTransform = transform;
        }
        if (rightHand)
        {
            right = this;

        }
        else
        {
        left = this;

        }

        original = transform;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTarget();
        if (Vector3.Distance(handTransform.position, targetPosition) > positionDeadZone)
        {if (handTransform != original)
            {
                handTransform = original;
            }
            handTransform.position = Vector3.Lerp(handTransform.position, targetPosition, moveSpeed * Time.deltaTime);
        }
        else if (Vector3.Distance(handTransform.position, targetPosition) > positionDeadZone * .1f)
        {
            handTransform.parent = targetTransform;
            handTransform.localPosition = new Vector3();

        }
        if (Quaternion.Angle(handTransform.rotation, targetRotation) > rotationDeadZone)
        {
            handTransform.rotation = Quaternion.Lerp(handTransform.rotation, targetRotation, moveSpeed * Time.deltaTime);
        }
        else if (Quaternion.Angle(handTransform.rotation, targetRotation) > rotationDeadZone * .1f)
        {
            handTransform.rotation = targetRotation;
        }

        if (showDebug)
        {
            targetVisualiseTransform.position = targetPosition;
            targetVisualiseTransform.rotation = targetRotation;
        }
    }

    private void OnDrawGizmos()
    {
        if (showDebug)
        {
            Gizmos.DrawSphere(targetVisualiseTransform.position, debugSize);
        }
    }

    public void AddPointer(HandPositionPointer pp)
    {
        if (pointers.Contains(pp))
        {
            return;
        }
        if (pointers.Count == 0)
        {
            pointers.Add(pp);
        }
        bool flag = false;
        for (int i = 0; i < pointers.Count && !flag; i++)
        {
            if (pp.Priority>= pointers[i].Priority)
            {
                pointers.Insert(i,pp);
                flag = true;
            }
        }
        if (!flag)
        {
            pointers.Add(pp);
        }
        UpdateTarget();

    }

    public void RemovePointer(HandPositionPointer pp)
    {
        pointers.Remove(pp);
    }

    public void UpdateTarget()
    {
        if (pointers.Count > 0)
        {
            targetPosition = pointers[0].transform.position;
            targetRotation = pointers[0].transform.rotation;
            targetTransform = pointers[0].transform;
        }
    }
}
