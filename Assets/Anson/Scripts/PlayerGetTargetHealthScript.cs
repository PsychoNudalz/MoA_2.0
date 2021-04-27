using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetTargetHealthScript : MonoBehaviour
{
    [SerializeField] AnsonTempUIScript ansonTempUIScript;
    [SerializeField] Camera camera;
    [SerializeField] LifeSystemScript targetLS;
    [SerializeField] float updateRate = 0.5f;
    float lastUpdateTime;
    [SerializeField] float stayOnTargetTime = 1f;
    float lastStayOnTargetTime;
    [SerializeField] float detectionRange;
    [SerializeField] LayerMask layerMask;
    public AnsonTempUIScript AnsonTempUIScript { get => ansonTempUIScript; set => ansonTempUIScript = value; }

    private void FixedUpdate()
    {
        if (Time.time - lastUpdateTime > updateRate)
        {
            UpdateEnemyHealthBar();
            lastUpdateTime = Time.time;
        }
        if (targetLS!= null)
        {
            ansonTempUIScript.SetEnemyHealth(targetLS);
        }
    }

    void UpdateEnemyHealthBar()
    {
        Debug.DrawRay(camera.transform.position, camera.transform.forward * detectionRange, Color.cyan, updateRate);
        RaycastHit hit;
        if (Physics.Raycast(camera.transform.position, camera.transform.forward, out hit, detectionRange, layerMask))
        {
            targetLS = hit.collider.GetComponentInParent<TargetLifeSystem>();
            lastStayOnTargetTime = Time.time;
        }
        else
        {
            if (Time.time - lastStayOnTargetTime > stayOnTargetTime)
            {
                ansonTempUIScript.SetEnemyHealth(false);
                targetLS = null;
            }
        }
    }

}
