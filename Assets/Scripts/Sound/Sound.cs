using UnityEngine.Audio;
using UnityEngine;

[System.Serializable]
public class Sound : MonoBehaviour
{
    /*
    private void Awake()
    {
        if (soundName.Equals(""))
        {
            soundName = clip.name;
        }
        if (isUnique)
        {
            soundName = soundName + GetHashCode() + transform.parent.name + Time.time;
        }
    }

    public override bool Equals(object other)
    {
        if ((other == null) || !this.GetType().Equals(other.GetType()))
        {
            return false;
        }
        if (!soundName.Equals((other as Sound).soundName))
        {
            return false;
        }

        return true;
    }
    */

    public string soundName;
    [SerializeField] protected AudioClip clip;

    [Range(0f, 1f)]
    public float volume = .75f;
    [Range(0f, 1f)]
    public float volumeVariance = .1f;

    [Range(.1f, 3f)]
    public float pitch = 1f;
    [Range(0f, 1f)]
    public float pitchVariance = .1f;

    
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] SoundManager soundManager;

    float baseVolume;
    float basePitch;
    [Header("Convert From Old System")]
    [SerializeField] bool isOld;
    [SerializeField] bool isLoop;
    [SerializeField] bool isPlayOnAwake;

    public AudioSource source;




    public AudioMixer AudioMixer { get => audioMixer; set => audioMixer = value; }
    public SoundManager SoundManager { get => soundManager; set => soundManager = value; }

    private void Awake()
    {
        AwakeBehaviour();
    }

    protected virtual void InitialiseClip()
    {
        source = gameObject.AddComponent<AudioSource>();
        source.clip = clip;
        source.volume = volume;
        source.pitch = pitch;
    }

    protected virtual void AwakeBehaviour()
    {
        if (source == null && clip != null)
        {
            InitialiseClip();
        }
        else if (source == null && gameObject.TryGetComponent(out source))
        {
            print("Auto found audio:" + source.clip);
        }
        if (soundManager != null)
        {
            soundManager = FindObjectOfType<SoundManager>();
        }
        source.clip = clip;
        baseVolume = source.volume;
        basePitch = source.pitch;

        if (isOld)
        {
            source.loop = isLoop;
        }
            source.playOnAwake = isPlayOnAwake;
    }

    public bool IsPlaying()
    {
        return source.isPlaying;
    }

    public virtual void Pause()
    {
        source.Pause();
    }
    public virtual void Resume()
    {
        source.UnPause();
    }
    public virtual void Play()
    {
        if (!source.isPlaying)
        {

            PlayF();
        }
    }
    public virtual void PlayF()
    {

        source.volume = baseVolume * (1f + UnityEngine.Random.Range(-volumeVariance / 2f, volumeVariance / 2f));
        source.pitch = basePitch * (1f + UnityEngine.Random.Range(-pitchVariance / 2f, pitchVariance / 2f));

        source.Play();
    }
    public virtual void Stop()
    {
        source.Stop();
    }

    public void ModifySource()
    {

    }
}
