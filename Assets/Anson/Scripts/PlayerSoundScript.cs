using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundScript : MonoBehaviour
{
    [SerializeField] SoundManager soundManager;
    [SerializeField] Sound dashSound;
    [SerializeField] Sound jumpSound;
    [SerializeField] Sound takeDamageSound;
    [SerializeField] Sound healSound;
    // Start is called before the first frame update

    private void Awake()
    {
        soundManager = FindObjectOfType<SoundManager>();
    }

    public void Play_Dash()
    {
        dashSound.PlayF();
    }
    public void Play_Jump()
    {
        jumpSound.PlayF();
    }
    public void Play_TakeDamage()
    {
        takeDamageSound.PlayF();
    }
    public void Play_Heal()
    {
        healSound.PlayF();
    }
}
