using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ShootingRangeScript : MonoBehaviour
{
    [SerializeField] List<SpawnPointScript> spawnPoints;
    [SerializeField] Animator animator;
    [SerializeField] int totalKills = 0;
    [SerializeField] int totalTargets = 0;
    [SerializeField] int waveKills = 0;
    [SerializeField] int waveTargets = 0;
    [SerializeField] string waveSeed;
    [SerializeField] TextMeshPro killText;
    [SerializeField] float timeNow;
    [SerializeField] float timeNow_Wave;
    [SerializeField] bool isRoutine = false;
    [SerializeField] List<string> shootingSequence;
    [Header("Wave behaviour")]
    [SerializeField] Vector2 numberOfTargets;
    [SerializeField] float numberOfWave = 5;
    [SerializeField] float numberOfWaveNow;

    [SerializeField] float timeBetweenWavePerTarget = 1.5f;

    private void Awake()
    {
        if (spawnPoints.Count == 0)
        {
            spawnPoints.AddRange(GetComponentsInChildren<SpawnPointScript>());
        }
        foreach (SpawnPointScript s in spawnPoints)
        {
            s.currentSR = this;
        }
        if (numberOfTargets.y > spawnPoints.Count)
        {
            numberOfTargets.y = spawnPoints.Count;
        }
    }

    private void Update()
    {
        if (isRoutine)
        {
            UpdateKillCounter();

            if (numberOfWaveNow <= numberOfWave && (timeNow_Wave >= timeBetweenWavePerTarget * waveTargets || waveKills == waveTargets))
            {
                StartWave();
            }
            else if (numberOfWaveNow > numberOfWave)
            {
                numberOfWaveNow--;
                isRoutine = false;
                foreach (SpawnPointScript s in spawnPoints)
                {
                    s.Despawn();
                }
                UpdateKillCounter();
            }
            timeNow += Time.deltaTime;
            timeNow_Wave += Time.deltaTime;
        }
    }

    public void ResetRange()
    {
        totalKills = 0;
        totalTargets = 0;
        numberOfWaveNow = 0;
        timeNow = 0;
        waveKills = 0;
        waveTargets = 0;
        timeNow_Wave = 0;
        foreach (SpawnPointScript s in spawnPoints)
        {
            s.Despawn();
        }
    }

    public void StartShootCourse(string inputSeed = "0", float waveTime = 0f, int numberOfWave = 1, List<int> healthOverrideTargets = null, int overrideValue = -1)
    {
        ResetRange();
        totalKills = 0;
        totalTargets = 0;
        numberOfWaveNow = 0;
        timeNow = 0;
        if (waveTime != 0f)
        {
            timeBetweenWavePerTarget = waveTime;
        }
        if (numberOfWave != 0)
        {
            this.numberOfWave = numberOfWave;
        }
        isRoutine = true;
        shootingSequence = new List<string>();
        if (inputSeed.Equals(""))
        {
            StartWave();
        }
        else
        {
            StartWave(inputSeed, healthOverrideTargets,overrideValue);
        }
        //DelayStartWave();
    }

    public void StartWave()
    {
        StartWave(System.Convert.ToString(Mathf.RoundToInt(Random.Range(numberOfTargets.x, Mathf.Pow(2, numberOfTargets.y))), 2));
    }

    public void StartWave(string inputSeed, List<int> healthOverrideTargets = null, int overrideValue = -1)
    {
        waveSeed = inputSeed;
        waveKills = 0;
        waveTargets = 0;
        timeNow_Wave = 0;
        
        numberOfWaveNow++;
        if (numberOfWaveNow > numberOfWave)
        {
            return;
        }
        shootingSequence.Add(waveSeed.ToString());
        UpdateKillCounter();
        for (int i = 0; i < spawnPoints.Count && i < waveSeed.Length; i++)
        {
            spawnPoints[i].Despawn();
            if (waveSeed[i].ToString().Equals("1"))
            {
                if (healthOverrideTargets!=null&& healthOverrideTargets.Contains(i))
                {
                    spawnPoints[i].Spawn(overrideValue);
                }
                else
                {
                spawnPoints[i].Spawn();

                }
                waveTargets++;
                totalTargets++;
            }
        }
    }


    public void increaseWaveKills()
    {
        waveKills += 1;
        totalKills += 1;
        UpdateKillCounter();
    }

    void UpdateKillCounter()
    {
        killText.text = string.Concat(
            "Total kills: ", totalKills, "/", totalTargets, "\n",
            "Wave Kills: ", waveKills, "/", waveTargets, "\n",
            "Round: ", numberOfWaveNow, "/", numberOfWave, "\n",
            "Time: ", timeNow
            );
    }

    IEnumerator DelayStartWave()
    {
        StartWave();
        yield return new WaitForSeconds(timeBetweenWavePerTarget);
        if (numberOfWaveNow > 0)
        {
            StartCoroutine(DelayStartWave());
        }
        else
        {
            isRoutine = false;
        }
    }

    public bool IsWaveCleared()
    {
        bool returnBool = totalKills >= totalTargets && totalTargets != 0;
        return returnBool;
    }
    public bool IsWaveTimedOut()
    {
        return timeNow_Wave >= timeBetweenWavePerTarget * waveTargets;
    }

    public void SetLoopMode(int i)
    {
        if (animator == null)
        {
            return;
        }
        animator.SetInteger("LoopMode", i);
    }
}
