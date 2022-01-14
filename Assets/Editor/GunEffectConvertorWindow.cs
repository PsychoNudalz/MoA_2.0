#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GunEffectConvertorWindow : EditorWindow
{
    Vector2 scrollPos;
    //CardHandler cardHandler { get => FindObjectOfType<CardHandler>(); }

    private GunEffectsConvertor gunEffectsConvertor = new GunEffectsConvertor();

    [MenuItem("Window/Card Effect Convertor")]
    public static void ShowWindow()
    {
        GetWindow<GunEffectConvertorWindow>("Gun Effect Convertor Window");
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        GUILayout.Label("I have no idea what I am doing", EditorStyles.boldLabel);

        int bodiesFound = 0;
        if (GUILayout.Button("Check all bodies"))
        {
            bodiesFound = gunEffectsConvertor.GetAllBodyComponents();
            gunEffectsConvertor.TestPrint();
        }

        GUILayout.Label($"Found Bodies: {bodiesFound}", EditorStyles.boldLabel);


        if (GUILayout.Button("Run Conversion check"))
        {
            gunEffectsConvertor.RunConvertion();
        }

        GUILayout.EndScrollView();
    }


/*
    private void MarkDirty()
    {
        EditorUtility.SetDirty(cardHandler);
        EditorSceneManager.MarkSceneDirty(cardHandler.gameObject.scene);
        foreach (Card c in cardHandler.AllCards)
        {
            if (c)
            {
            EditorUtility.SetDirty(c.gameObject);
            }

        }
    }*/
}


public class GunEffectsConvertor
{
    public List<GunComponent_Body> GunComponentBodies = new List<GunComponent_Body>();

    public int GetAllBodyComponents()
    {
        GunComponentBodies = new List<GunComponent_Body>();
        GunComponentBodies = FileLoader.GetAllFilesFromResources<GunComponent_Body>("Guns");
        return GunComponentBodies.Count;
    }

    public void TestPrint()
    {
        foreach (GunComponent_Body gunComponentBody in GunComponentBodies)
        {
            Debug.Log(gunComponentBody.name);
        }
    }

    public void RunConvertion()
    {
        GunComponentBodies = new List<GunComponent_Body>();
        GunComponentBodies = FileLoader.GetAllFilesFromResources<GunComponent_Body>("Guns");
        GunEffectsController gunEffectsController;
        foreach (GunComponent_Body gunComponentBody in GunComponentBodies)
        {
            Debug.Log($"Updating Gun Component Body {gunComponentBody.name}");
            gunEffectsController = gunComponentBody.GetComponent<GunEffectsController>();
            gunEffectsController.GetBodyData(gunComponentBody);
            EditorUtility.SetDirty(gunEffectsController.gameObject);
        }
    }
}

public class GunEffectConvertorWindowInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUI.changed)
        {
        }
    }
}
#endif