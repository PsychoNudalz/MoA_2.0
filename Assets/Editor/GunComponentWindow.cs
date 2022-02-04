#if UNITY_EDITOR
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

public class GunComponentWindow : EditorWindow
{
    Vector2 scrollPos;

    //CardHandler cardHandler { get => FindObjectOfType<CardHandler>(); }
    private bool showConnectedEffectControllers;

    private GunComponentMasterController gunComponentMasterController = new GunComponentMasterController();

    [MenuItem("Window/Gun Component Window")]
    public static void ShowWindow()
    {
        GetWindow<GunComponentWindow>("Gun Component Window");
    }

    private void OnGUI()
    {
        GUILayout.BeginVertical();
        gunComponentMasterController = new GunComponentMasterController();
        gunComponentMasterController.Initialise();

        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);


        GUILayout.Label("I have no idea what I am doing", EditorStyles.boldLabel);

        GUILayout.Space(10f);
        GUILayout.Label("Sound Fix");
        if (GUILayout.Button("Fix gun sound naming"))
        {
            gunComponentMasterController.FixGunSoundNaming();
        }

        if (GUILayout.Button("Fix gun pool"))
        {
            gunComponentMasterController.FixGunSoundToPool();
        }

        GUILayout.Space(20f);
        showConnectedEffectControllers = GUILayout.Toggle(showConnectedEffectControllers,
            $"Show Connected Components: {gunComponentMasterController.GunEffectsControllers.Count}");
        if (showConnectedEffectControllers)
        {
            List<GunEffectsController> gunEffectsControllers = gunComponentMasterController.GunEffectsControllers;
            foreach (GunEffectsController gunEffectsController in gunEffectsControllers)
            {
                GUILayout.Label(gunEffectsController.name);
            }
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

public class GunComponentMasterController
{
    private List<GunEffectsController> gunEffectsControllers = new List<GunEffectsController>();
    private List<GunComponent_Body> gunComponentBodies = new List<GunComponent_Body>();

    public List<GunEffectsController> GunEffectsControllers => gunEffectsControllers;

    public List<GunComponent_Body> GunComponentBodies => gunComponentBodies;

    public void Initialise()
    {
        gunComponentBodies = new List<GunComponent_Body>();
        gunComponentBodies = FileLoader.GetAllFilesFromResources<GunComponent_Body>("Guns", "*.prefab", false);
        gunEffectsControllers = new List<GunEffectsController>();
        gunEffectsControllers = FileLoader.GetAllFilesFromResources<GunEffectsController>("Guns", "*.prefab", false);
    }

    public void MarkAllDirty()
    {
        foreach (GunComponent_Body body in gunComponentBodies)
        {
            EditorUtility.SetDirty(body);
        }

        foreach (GunEffectsController gunEffectsController in gunEffectsControllers)
        {
            EditorUtility.SetDirty(gunEffectsController);
        }
    }

    public void FixGunSoundNaming()
    {
        foreach (GunEffectsController gunEffectsController in GunEffectsControllers)
        {
            foreach (Sound sound in gunEffectsController.GetComponentsInChildren<Sound>())
            {
                if (sound.name.Equals("GunShound"))
                {
                    sound.name = "GunSound";
                }
            }
        }

        MarkAllDirty();
    }

    public void FixGunSoundToPool()
    {
        SoundPool temp;
        foreach (GunEffectsController gunEffectsController in GunEffectsControllers)
        {
            foreach (Sound sound in gunEffectsController.GetComponentsInChildren<Sound>())
            {
                if (sound.name.Equals("GunSound"))
                {
                    temp = sound.GetComponent<SoundPool>();
                    if (temp== null)
                    {
                        {
                            Debug.Log($"Found no sound pool for {sound.transform.parent.parent}");
                            temp = sound.gameObject.AddComponent<SoundPool>();
                            temp.TransferSound(sound);
                            GameObject.DestroyImmediate(sound, true);
                        }
                    }

                    if (temp)
                    {
                        gunEffectsController.SetFireSound(temp);
                    }
                }
            }
        }

        MarkAllDirty();
    }
}

public class GunComponentWindowInspector : Editor
{
    public override void OnInspectorGUI()
    {
        if (GUI.changed)
        {
        }
    }
}
#endif