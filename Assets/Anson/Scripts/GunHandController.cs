using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandController : MonoBehaviour
{
    [SerializeField] HandPositionPointer[] handPositionPointers;
    [SerializeField] HandPositionPointer handRest;
    [SerializeField] HandPositionPointer handMag;

    public void AddPointToLeft(int i)
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
    public void RemovePointToLeft(int i)
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

}
