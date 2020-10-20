using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    UISlider m_musicSlider;

    public bool m_isClick = false;

    // Start is called before the first frame update
    void Start()
    {
        m_musicSlider = GetComponent<UISlider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!m_isClick)
        {
            AutioMusicLink();
        }
    }

    // 음악 자동 연동
    void AutioMusicLink()
    {
        float curMusicTime = MusicManager.instance.GetTime(true);
        float maxMusicTime = MusicManager.instance.GetLength(true);

        m_musicSlider.value = curMusicTime / maxMusicTime;
    }

    // 음악 수동 연동
    public void ManualLinkStart()
    {
        m_isClick = true;
    }

    public void ManualLingEnd()
    {
        float maxMusicTime = MusicManager.instance.GetLength(true);

        float setMusicTime = maxMusicTime * m_musicSlider.value;

        MusicManager.instance.SetTime(true, setMusicTime);

        m_isClick = false;
    }
}