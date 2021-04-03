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
    [SerializeField] List<int> pickUpSteps ;
    [SerializeField] List<int> killSteps;


    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {

        //check for pick up on steps 1, 4
        if (pickUpSteps.Contains(stepValue)&& currentGun != null)
        {
            if (!currentGun.transform.parent.Equals(guns))
            {
                NextStep();
                currentGun = null;

            }
        }


        //check if player killed targets on steps 2, 3, 5
        if (killSteps.Contains(stepValue))
        {
            if (shootingRange.IsWaveCleared())
            {
                NextStep();
                shootingRange.ResetRange();
                //print("WaveCleared");
            }
        }
    }

    public void NextStep()
    {
        stepValue++;
        animator.SetTrigger("Next");
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
        shootingRange.StartShootCourse(seed, waveTime, numberOfWaves);

    }

    public void SpawnShootingRange_Single()
    {
        shootingRange.StartShootCourse("01", float.PositiveInfinity,1);
    }


    public void SpawnShootingRange_Full()
    {
        shootingRange.StartShootCourse("111111111111", float.PositiveInfinity, 1);

    }


    public void SpawnGun(int i)
    {
        currentGun = null;
        currentGun = gunGenerator.GenerateGun(i);
        currentGun.transform.SetParent(guns);
    }
}
