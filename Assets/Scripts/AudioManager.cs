using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string name;    
    public AudioClip clip;

    private int fileID;
    private int soundID;

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

    public AudioSource GetAudioSource()
    {
        return audioSource;
    }

    public void SetFileID(int _fileID)
    {
        fileID = _fileID;
    }

    public int GetFileID()
    {
        return fileID;
    }

    public void SetSoundID(int _soundID)
    {
        soundID = _soundID;
    }

    public int GetSoundID()
    {
        return soundID;
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
#if UNITY_ANDROID && !UNITY_EDITOR
        AndroidNativeAudio.makePool();
#endif

        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];

#if UNITY_ANDROID && !UNITY_EDITOR
            if (sound.name == "background")
            {
                sound.volume = 0.2f;

                GameObject soundObejct = new GameObject("사운드 파일 이름 : " + i + " = " + sounds[i].name);
                sounds[i].SetAudioSource(soundObejct.AddComponent<AudioSource>());
                soundObejct.transform.SetParent(this.transform);

            }
            else
            {
                int fID = AndroidNativeAudio.load("GameSounds/" + sound.name + ".wav");
                sound.SetFileID(fID);
            }
#else
            GameObject soundObejct = new GameObject(sounds[i].name);
            sounds[i].SetAudioSource(soundObejct.AddComponent<AudioSource>());
            soundObejct.transform.SetParent(this.transform);
#endif
        }
    }

    public void Play(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sound.name != name)
                continue;

#if UNITY_ANDROID && !UNITY_EDITOR
            if (sound.name == "background")
            {
                sound.Play();
            }
            else
            {
                sound.SetSoundID(AndroidNativeAudio.play(sound.GetFileID()));
                
                AndroidNativeAudio.setVolume(sound.GetSoundID(), 1f);
            }
#else
            sound.Play();
#endif

        }
    }

    private void OnApplicationQuit()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sound.name == "background")
                continue;
            // Clean up when done
            AndroidNativeAudio.unload(sound.GetFileID());
        }

        AndroidNativeAudio.releasePool();
#endif
    }

    public void Stop(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sound.name != name)
                continue;


#if UNITY_ANDROID && !UNITY_EDITOR
            if (sound.name == "background")
                sound.Stop();
            else
            {
                AndroidNativeAudio.unload(sound.GetFileID());
            }
#else
            sound.Stop();
#endif
        }
    }

    public bool IsPlaying(string name)
    {
        for (int i = 0; i < sounds.Length; i++)
        {
            Sound sound = sounds[i];
            if (sound.name != name)
                continue;

#if UNITY_ANDROID && !UNITY_EDITOR
            if (sound.name == "background")
            {
                if (sound.GetAudioSource().isPlaying)
                    return true;
            }
#else
            if (sound.GetAudioSource().isPlaying)
                return true;
#endif
        }

        return false;
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
