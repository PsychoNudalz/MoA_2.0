using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunHandController : MonoBehaviour
{
    [Tooltip("0: rest, 1: mag, 2: bolt, 2+: anything ")]
    [SerializeField]
    HandPositionPointer[] handPositionPointers;

    [SerializeField]
    HandPositionPointer handRest;

    [SerializeField]
    HandPositionPointer handMag;

    [SerializeField]
    private HandPositionPointer rightHand_Rest;

    public HandPositionPointer HandRest
    {
        get => handRest;
        set => handRest = value;
    }

    public HandPositionPointer HandMag
    {
        get => handMag;
        set => handMag = value;
    }

    public HandPositionPointer RightHandRest
    {
        get => rightHand_Rest;
        set => rightHand_Rest = value;
    }

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
        foreach (HandPositionPointer h in handPositionPointers)
        {
            HandController.left.RemovePointer(h);
        }
    }

    public void SetNewRestPoint_Left(HandPositionPointer hpp)
    {
        handRest = hpp;
        handPositionPointers[0] = hpp;
    }
    public void AddPoint_Right(int i)
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

    public void RemovePoint_Right(int i)
    {
        try
        {
            HandController.right.RemovePointer(handPositionPointers[i]);
        }
        catch (System.IndexOutOfRangeException)
        {
            Debug.LogError($"{transform.parent.name} pointer: {i} out of range");
        }
    }


    public void RemoveAllPoints_Right()
    {
        foreach (HandPositionPointer h in handPositionPointers)
        {
            HandController.right.RemovePointer(h);
        }
    }

    public void SetNewRestPoint_Right(HandPositionPointer hpp)
    {
        handRest = hpp;
        handPositionPointers[0] = hpp;
    }

    public void RemoveAllPoints()
    {
        RemoveAllPoints_Left();
        RemoveAllPoints_Right();
    }

}