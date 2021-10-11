using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandController : MonoBehaviour
{
    [SerializeField] HandPositionPointer[] handPositionPointers;
    [SerializeField] HandPositionPointer handRest;
    [SerializeField] HandPositionPointer handMag;

    public HandPositionPointer HandRest { get => handRest; set => handRest = value; }
    public HandPositionPointer HandMag { get => handMag; set => handMag = value; }

    public void AddPoint_Left(int i)
    {
        try
        {
            HandController.left.AddPointer(handPositionPointers[i]);
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.LogError($"{transform.parent.name} pointer: {i} out of range");
        }
    }
    public void RemovePoint_Left(int i)
    {
        try
        {
            HandController.left.RemovePointer(handPositionPointers[i]);
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.LogError($"{transform.parent.name} pointer: {i} out of range");
        }
    }


    public void RemoveAllPoints_Left()
    {
        foreach(HandPositionPointer h in handPositionPointers)
        {
            HandController.left.RemovePointer(h);
        }
    }

}
