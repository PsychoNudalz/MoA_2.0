using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialControllerScript : MonoBehaviour
{
    [SerializeField] int stepValue = -1;
    [SerializeField] Animator animator;
    [SerializeField] GameObject door;
    [SerializeField] GunGeneratorScript gunGenerator;
    [SerializeField] Transform guns;
    [SerializeField] GameObject currentGun;
    [SerializeField] ShootingRangeScript shootingRange;
    [SerializeField] GameObject gunAlter;
    [SerializeField] List<int> pickUpSteps;
    [SerializeField] List<int> killSteps;
    [SerializeField] List<int> timedKillSteps;


    // Start is called before the first frame update
    void Awake()
    {
        animator = GetComponent<Animator>();
        Application.targetFrameRate = 200;
        Time.timeScale = 1;

    }

    private void Start()
    {
        Application.targetFrameRate = 200;

    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //check for pick up on steps 1, 4, 6, 8
        if (pickUpSteps.Contains(stepValue) && currentGun != null)
        {
            if (!currentGun.transform.parent.Equals(guns))
            {
                NextStep();
                currentGun = null;

            }
        }


        //check if player killed targets on steps 2, 3, 5,7 ,9
        if (killSteps.Contains(stepValue))
        {
            if (shootingRange.IsWaveCleared())
            {
                NextStep();
                shootingRange.ResetRange();
                //print("WaveCleared");
            }
        }

        //check if player killed the targets in time on step 10
        if (timedKillSteps.Contains(stepValue))
        {
            if (shootingRange.IsWaveCleared())
            {
                NextStep();
                //Anson: cba to set a new if
                gunAlter.SetActive(true);
            }
            else if (!shootingRange.IsWaveCleared() && shootingRange.IsWaveTimedOut()&&shootingRange.IsRoutine)
            {
                ResetShootingRange();
                BackStep();
            }
        }
    }

    public void NextStep()
    {
        stepValue++;
        animator.SetTrigger("Next");
    }
    public void BackStep()
    {
        stepValue--;
        animator.SetTrigger("Back");
    }

    public void IncrementStep()
    {
        stepValue++;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") && stepValue < 0)
        {
            NextStep();
            door.SetActive(true);
        }
    }

    public void SpawnGun_PP()
    {
        SpawnGun(0);
    }
    public void SpawnShootingRange(string seed, float waveTime, int numberOfWaves)
    {
        shootingRange.SetLoopMode(1);

        shootingRange.StartShootCourse(seed, waveTime, numberOfWaves);

    }
    public void ResetShootingRange()
    {
        shootingRange.ResetRange();

    }

    public void SpawnShootingRange_Single()
    {
        shootingRange.SetLoopMode(1);
        shootingRange.StartShootCourse("01", float.PositiveInfinity, 1);
    }



    public void SpawnShootingRange_Full()
    {
        shootingRange.SetLoopMode(1);
        shootingRange.StartShootCourse("101101101101", float.PositiveInfinity, 1);
        
        //debug
        //shootingRange.StartShootCourse("1", float.PositiveInfinity, 1);

    }

    public void SpawnShootingRange_Cover()
    {
        shootingRange.SetLoopMode(0);
        int[] list = { 6, 7, 8, 9, 10, 11 };
        shootingRange.StartShootCourse("101101111101", 40f/9f, 1, new List<int>(list), 50);
    }
    public void SpawnShootingRange_Timed()
    {
        shootingRange.SetLoopMode(0);
        int[] list = { 0,2,3,5 };

        shootingRange.StartShootCourse("101101000000", 15f/4f, 1, new List<int>(list), 3000);

    }
    public void SpawnShootingRange_TimedStarter()
    {
        shootingRange.SetLoopMode(0);
        int[] list = { 1};

        shootingRange.StartShootCourse("01", float.PositiveInfinity, 1, new List<int>(list), 1);

    }


    public void SpawnGun(int i)
    {
        currentGun = null;
        currentGun = gunGenerator.GenerateGun(i);
        currentGun.transform.SetParent(guns);
    }


}
