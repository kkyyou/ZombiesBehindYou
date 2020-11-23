using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;    
    public AudioClip clip;

    private AudioSource audioSource;

    public float volume;
    public bool loop;

    public void SetAudioSource(AudioSource source)
    {
        audioSource = source;
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = loop;
    }

    public void SetVolume()
    {
        audioSource.volume = volume;
    }

    public void Play()
    {
        audioSource.Play();
    }

    public void Stop()
    {
        audioSource.Stop();
    }

    public void SetLoop()
    {
        audioSource.loop = true;
    }

    public void SetLoopCancel()
    {
        audioSource.loop = false;
    }
}

public class AudioManager : MonoBehaviour
{
    static public AudioManager instance;

    [SerializeField]
    public Sound[] sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            GameObject soundObejct = new GameObject("사운드 파일 이름 : " + i + " = " + sounds[i].name);
            sounds[i].SetAudioSource(soundObejct.AddComponent<AudioSource>());
            soundObejct.transform.SetParent(this.transform);
        }

        if (GameManager.instance.GetListenBgm())
            Play("background");
    }

    public void Play(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sound.name != name)
                continue;

            sound.Play();
        }
    }

    public void Stop(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sound.name != name)
                continue;

            sound.Stop();
        }
    }

    public void SetLoop(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sound.name != name)
                continue;

            sound.SetLoop();
        }
    }

    public void SetLoopCancel(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sound.name != name)
                continue;

            sound.SetLoopCancel();
        }
    }

    public void SetVolume(string name, float volume)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sound.name != name)
                continue;

            sound.volume = volume;
            sound.SetVolume();
        }
    }

    public void PlayRandomDamageSound()
    {
        int soundNum = Random.Range(0, 2);

        if (soundNum == 0)
        {
            Play("damaged1");
        }
        else if (soundNum == 1)
        {
            Play("damaged2");
        }
        else if (soundNum == 2)
        {
            Play("damaged3");
        }
    }

    public void PlayRandomAttackSound()
    {
        int soundNum = Random.Range(0, 2);

        if (soundNum == 0)
        {
            Play("slash1");
        }
        else if (soundNum == 1)
        {
            Play("slash2");
        }
        else if (soundNum == 2)
        {
            Play("slash3");
        }
    }
}
