using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager instance;

    Dictionary<string, Sound> m_sounds = new Dictionary<string, Sound>();

    AudioSource m_bgm;  // 브금 출력용
    AudioSource m_sfx;  // 이펙트 출력용

    private void Awake()
    {
        instance = this;

        m_bgm = gameObject.AddComponent<AudioSource>();
        m_sfx = gameObject.AddComponent<AudioSource>();

        m_bgm.playOnAwake = false;
        m_sfx.playOnAwake = false;

        AddSound("ButterFly"    , "Sounds/ButterFly"    , true);
        AddSound("KinnikuMan"   , "Sounds/KinnikuMan"   , true);
        AddSound("OnePiece"     , "Sounds/OnePiece"     , true);
        AddSound("SlamDunk"     , "Sounds/SlamDunk"     , true);

        Play("OnePiece");

        DontDestroyOnLoad(gameObject);
    }

    // 추가
    public void AddSound(string key, string path, bool bgm)
    {
        Sound sound = new Sound();

        sound.clip = Resources.Load<AudioClip>(path);
        sound.isBGM = bgm;

        m_sounds.Add(key, sound);
    }

    // 재생
    public void Play(string keyName, float volume = 1.0f)
    {
        if (m_sounds.ContainsKey(keyName))
        {
            Sound sound = m_sounds[keyName];

            if (sound.isBGM)
            {
                m_bgm.clip = sound.clip;
                m_bgm.volume = volume;
                m_bgm.Play();
            }
            else
            {
                m_sfx.clip = sound.clip;
                m_sfx.volume = volume;
                m_sfx.PlayOneShot(sound.clip);
            }
        }
    }

    // 정지
    public void Stop(string keyName)
    {
        if (m_sounds.ContainsKey(keyName))
        {
            Sound sound = m_sounds[keyName];

            if (sound.isBGM)
            {
                if (m_bgm.clip == sound.clip && m_bgm.isPlaying)
                {
                    m_bgm.Stop();
                }
            }
            else
            {
                if (m_sfx.clip == sound.clip && m_sfx.isPlaying)
                {
                    m_sfx.Stop();
                }
            }
        }
    }

    // 일시정지
    public void Pause(bool bgm)
    {
        if (bgm)
        {
            m_bgm.Pause();
        }
        else
        {
            m_sfx.Pause();
        }
    }

    public bool Playing(bool bgm)
    {
        if (bgm)
        {
            return m_bgm.isPlaying;
        }
        else
        {
            return m_sfx.isPlaying;
        }
    }

    // 볼륨 조정
    public void SetVolume(bool bgm, float volume)
    {
        if (bgm)
        {
            m_bgm.volume = volume;
        }
        else
        {
            m_sfx.volume = volume;
        }
    }

    public float GetVolume(bool bgm)
    {
        if (bgm)
        {
            return m_bgm.volume;
        }
        else
        {
            return m_sfx.volume;
        }
    }

    public float GetTime(bool bgm)
    {
        if (bgm)
        {
            return m_bgm.time;
        }
        else
        {
            return m_sfx.time;
        }
    }

    public void SetTime(bool bgm, float time)
    {
        if (bgm)
        {
            m_bgm.time = time;
        }
        else
        {
            m_sfx.time = time;
        }
    }
    public float GetLength(bool bgm)
    {
        if (bgm)
        {
            if (m_bgm.isPlaying)
            {
                return m_bgm.clip.length;
            }
        }

        return 0;
    }
}

public class Sound
{
    public bool isBGM { set; get; }
    public AudioClip clip { set; get; }
}