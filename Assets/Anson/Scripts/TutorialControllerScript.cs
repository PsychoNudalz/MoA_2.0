using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TutorialControllerScript : MonoBehaviour
{
    [SerializeField] List<TextMeshProUGUI> allText;
    [SerializeField] int textPointer = 0;
    [SerializeField] int stepValue = -1;
    [SerializeField] Animator animator;
    [SerializeField] GameObject door;
    [SerializeField] GunGeneratorScript gunGenerator;
    [SerializeField] Transform guns;
    [SerializeField] GameObject currentGun;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (stepValue == 1&& currentGun != null)
        {
            if (!currentGun.transform.parent.Equals(guns))
            {
                NextStep();
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
        currentGun = gunGenerator.GenerateGun(0);
        currentGun.transform.SetParent(guns);
    }
}
