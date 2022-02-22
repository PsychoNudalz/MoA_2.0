using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perk_PointExplosion : Perk
{
    [Header("Point Explosion Stat")]
    [SerializeField]
    private float maxRange = 5f;

    [Header("Debug")]
    [SerializeField]
    private Vector3 averagePoint;
    
    [SerializeField]
    private List<Vector3> lastHits;

    [SerializeField]
    private PointExplosionPoint[] pointExplosionPoints;

    private int pointIndex=0;

    [SerializeField]
    private PointExplosionSphere pointExplosionSphere;

    private Coroutine moveCoroutine;

    public override void OnShot(ShotData shotData)
    {
        
    }

    public override void OnHit(ShotData shotData)
    {
        //OnActivatePerk();

        if (duration_Current > 0)
        {
            return;
        }

        AddNewPoint(shotData.HitPos);
        UpdatePoints();

        stack_Current = lastHits.Count;
        
        if (!isActive)
        {
            isActive = true;
            if (isPlayerPerk)
            {
                PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.ADD);
                perkEffectController?.PlayActivate();
            }
        }
        else
        {
            if (isPlayerPerk)
            {
                PlayerUIScript.current.SetPerkDisplay(this, PerkDisplayCall.UPDATE);
                perkEffectController?.PlayActivate();
            }
        }
    }

    public override void OnTargetHit(ShotData shotData)
    {
        
    }

    public override void OnCritical(ShotData shotData)
    {
        
    }

    public override void OnMiss(ShotData shotData)
    {
        
    }

    public override void OnKill(ShotData shotData)
    {
        
    }

    public override void OnElementTrigger(ShotData shotData)
    {
        
    }

    public override void OnReloadStart()
    {
        
    }

    public override void OnReloadEnd()
    {
        
    }

    public override void OnPerReload()
    {
        
    }

    public override void OnFixedUpdate()
    {
        
    }

    public override void OnDurationEnd()
    {
        
    }

    public override void OnActivatePerk(Object data = null)
    {
        base.OnActivatePerk(data);
        moveCoroutine = StartCoroutine(DelayMoveExplosiveSphere());
        pointExplosionSphere.SetPrime();
        ResetDuration();
    }

    public override void OnAwake()
    {
        base.OnAwake();
        pointExplosionSphere.transform.parent = null;
        foreach (PointExplosionPoint pointExplosionPoint in pointExplosionPoints)
        {
            pointExplosionPoint.transform.parent = null;
            pointExplosionPoint.gameObject.SetActive(false);
        }
        pointExplosionSphere.gameObject.SetActive(false);
    }

    protected override void OnUpdate()
    {
        if (!isActive)
        {
            base.OnUpdate();
        }
    }


    public override void OnDeactivatePerk()
    {
        //base.OnDeactivatePerk();
        isActive = false;
        ResetDuration();
        lastHits = new List<Vector3>();
        foreach (PointExplosionPoint pointExplosionPoint in pointExplosionPoints)
        {
            pointExplosionPoint.gameObject.SetActive(false);
        }
    }



    void AddNewPoint(Vector3 point)
    {
        if (!pointExplosionPoints[pointIndex].gameObject.activeSelf)
        {
            pointExplosionPoints[pointIndex].gameObject.SetActive(true);
        }
        if (lastHits.Count >= stack_Max)
        {
            lastHits.RemoveAt(0);
        }
        lastHits.Add(point);
        averagePoint = CalculateAveragePoint(lastHits.ToArray());
        pointExplosionPoints[pointIndex].transform.position = point;
        foreach (PointExplosionPoint pointExplosionPoint in pointExplosionPoints)
        {
            if (!pointExplosionPoint.Equals(pointExplosionPoints[pointIndex]))
            {
                pointExplosionPoint.SetPositionPoint(point);
            }
            else
            {
                pointExplosionPoint.ResetAllPoints();
            }
        }

        pointIndex = (pointIndex+1)%pointExplosionPoints.Length;
        // pointExplosionPoints[pointIndex].ResetOldestPoint();
    }

    void UpdatePoints()
    {
        if (lastHits.Count<stack_Max|| Vector3.Distance(averagePoint,lastHits[0])>maxRange)
        {
            pointExplosionSphere.gameObject.SetActive(false);
            if (moveCoroutine!=null)
            {
                StopCoroutine(moveCoroutine);
            }
        }
        else
        {
           OnActivatePerk();
        }


    }

    Vector3 CalculateAveragePoint(Vector3[] posList)
    {
        Vector3 temp = posList[0];
        for (int i = 1; i < posList.Length; i++)
        {
            temp += posList[i];
        }

        temp /= posList.Length;
        return temp;
    }

    IEnumerator DelayMoveExplosiveSphere()
    {
        yield return new WaitForEndOfFrame();
        pointExplosionSphere.gameObject.SetActive(true);
        pointExplosionSphere.transform.position = averagePoint;
    }
    
    
}
