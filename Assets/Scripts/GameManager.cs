using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // 노트 프리팹
    public GameObject notePrefab;
    public GameObject barPrefab;

    public bool isFinishLoad = false;

    public float speed = 10.0f;
    float beatPerBar = 32.0f;
    int timeRateBySpeed = 2;

    GameObject note;
    NoteObj m_note;

    Bms bms;

    // Start is called before the first frame update
    void Start()
    {
        bms = GetComponent<Bms>();

        // BMS 파일 파싱
        BmsParsing();

        bms.debug();

        // 시작선
        GameObject planeTop = GameObject.Find("Plane_Top");
        float startPositionY = planeTop.transform.position.y;

        // 판정선
        GameObject lineJudgeMent = GameObject.Find("LineJudgeMent");
        float judgeMentPositionY = lineJudgeMent.transform.position.y;

        // 노트 프리팹 생성, 리스트 저장
        float destroyDelayPositionY = 30.0f;

        // 노트 간격 비율
        float noteWidthRate = 1.8f;

        // 노트 프리팹 생성 및 리스트 저장
        List<NoteObj> noteLine1 = new List<NoteObj>();
        List<NoteObj> noteLine2 = new List<NoteObj>();
        List<NoteObj> noteLine3 = new List<NoteObj>();
        List<NoteObj> noteLine4 = new List<NoteObj>();
        List<NoteObj> noteLine5 = new List<NoteObj>();
        List<NoteObj> barLine = new List<NoteObj>();

        bool isLongNoteStart_1 = true;
        bool isLongNoteStart_2 = true;
        bool isLongNoteStart_3 = true;
        bool isLongNoteStart_4 = true;
        bool isLongNoteStart_5 = true;

        float preNoteTime_Ln1 = 0f;
        float preNoteTime_Ln2 = 0f;
        float preNoteTime_Ln3 = 0f;
        float preNoteTime_Ln4 = 0f;
        float preNoteTime_Ln5 = 0f;

        // 노트 소멸 딜레이 타임
        float destroyDelayTime = bms.TotalPlayTime + 1;

        // 바 생성
        float secondPerBar = 60.0f / bms.Bpm * 4.0f;
        int barCount = 0;

        // Bar 생성
        for (int i = 0; i < bms.TotalBarCount; i++)
        {
            // 바 라인 생성
            float barTime = barCount * secondPerBar;    // 바 시작시간
            note = (GameObject)Instantiate(barPrefab, new Vector3(0, startPositionY, 0), Quaternion.identity);

            m_note = note.GetComponent<NoteObj>();
            m_note.speed = speed;
            m_note.destroyPositionY = judgeMentPositionY - destroyDelayPositionY;
            m_note.destroyDelayTime = destroyDelayTime;
            m_note.noteTime = barTime;
            barLine.Add(m_note);
            barCount++;
        }

        // 노트 생성
        foreach (BarData barData in bms.BarDataList)
        {
            float linePositionX = 0;
            bool isLongChannel = false;

            int channel = barData.Channel;

            if (channel == 11 || channel == 51)
            {
                linePositionX -= 2;
            }
            else if (channel == 12 || channel == 52)
            {
                linePositionX -= 1;
            }
            else if (channel == 13 || channel == 53)
            {
                linePositionX = linePositionX;
            }
            else if (channel == 14 || channel == 54)
            {
                linePositionX += 1;
            }
            else if (channel == 15 || channel == 55)
            {
                linePositionX += 2;
            }

            if (channel == 51 || channel == 52 || channel == 53 || channel == 54 || channel == 55)
            {
                isLongChannel = true;
            }

            foreach (Dictionary<int, float> noteData in barData.NoteDataList)
            {
                foreach (int key in noteData.Keys)
                {
                    // 숏노트
                    if (!isLongChannel && key != 0 && channel != 16)
                    {
                        float noteTime = noteData[key];

                        note = (GameObject)Instantiate(notePrefab, new Vector3(linePositionX * noteWidthRate, startPositionY, 0), Quaternion.identity);
                        m_note = note.GetComponent<NoteObj>();
                        m_note.speed = speed;
                        m_note.destroyPositionY = judgeMentPositionY - destroyDelayPositionY;
                        m_note.destroyDelayTime = destroyDelayTime;
                        m_note.noteTime = noteTime;
                        m_note.channel = channel;

                        if(channel==11)
                        {
                            noteLine1.Add(m_note);
                        }
                        else if (channel == 12)
                        {
                            noteLine2.Add(m_note);
                        }
                        else if (channel == 13)
                        {
                            noteLine3.Add(m_note);
                        }
                        else if (channel == 14)
                        {
                            noteLine4.Add(m_note);
                        }
                        else if (channel == 15)
                        {
                            noteLine5.Add(m_note);
                        }
                    }

                    // 롱노트
                    if (isLongChannel && key != 0)
                    {
                        float secondPerBeat = 60.0f / bms.Bpm * 4.0f / beatPerBar;

                    }
                }
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    // BMS 로드
    void BmsParsing()
    {
        BarData barData;

        // BMS 파일을 찾기
        TextAsset path = Resources.Load("Sounds/Lovely_Summer") as TextAsset;

        // 엔터를 기준으로 자르기
        string[] lineData = path.text.Split('\n');

        for (int i = 0; i < lineData.Length; i++)
        {
            // 필요한 데이터만
            if (lineData[i].StartsWith("#"))
            {
                // 공백으로 자르기
                string[] splitText = lineData[i].Split(' ');

                // 데이터 섹션이 아니고 데이터가 없는 경우
                if (splitText.First().IndexOf(":") == -1 && splitText.Length == 1)
                {
                    continue;
                }

                // 제목
                if (splitText.First().Equals("#TITLE"))
                {
                    bms.Title = splitText[1];
                }
                // 가수
                else if (splitText.First().Equals("#ARTIST"))
                {
                    bms.Artist = splitText[1];
                }
                // 분당 비트수
                else if (splitText.First().Equals("#BPM"))
                {
                    bms.Bpm =float.Parse(splitText[1]);
                }
                // 노트 데이터 섹션
                else if (splitText.First().IndexOf(":") != -1)
                {
                    int bar = 0;

                    int.TryParse(splitText.First().Trim().Substring(1, 3), out bar);

                    int channel = 0;
                    int.TryParse(splitText.First().Trim().Substring(4, 2), out channel);

                    string noteString = splitText.First().Trim().Substring(7);

                    //barData = gameObject.AddComponent<BarData>();
                    barData = GetComponent<BarData>();
                    barData.Bar = bar;
                    barData.Channel = channel;
                    barData.NoteDataList = getNoteData(noteString, bar, bms.Bpm);
                }
            }
        }

        if (bms.BarDataList.Count > 0)
        {
            isFinishLoad = true;
        }
    }

    List<Dictionary<int, float>> getNoteData(string str, int bar, float bpm)
    {
        List<Dictionary<int, float>> data = new List<Dictionary<int, float>>();

        int totalBeat = 0;

        if (str.Trim().Length != 0)
        {
            // 현재 막대의 총 노트수 계산 >> 
            totalBeat = str.Trim().Length / 2;
        }

        float secondBar = 60.0f / bpm * 4.0f;
        float preSecond = bar * secondBar;

        float beatCount = 0;

        while (true)
        {
            int key = 0;
            int.TryParse(str.Substring(0, 2), out key);

            float time = 0;

            if (key != 0)
            {
                time = preSecond + (secondBar / totalBeat * beatCount);
            }

            Dictionary<int, float> dataPairs = new Dictionary<int, float>();

            dataPairs.Add(key, time);

            data.Add(dataPairs);

            bms.TotalPlayTime = time;
            bms.TotalBarCount = bar;

            if (str.Length > 2)
            {
                str = str.Substring(2);
            }
            else
            {
                break;
            }

            beatCount++;

            // 노트 수 증가
            for (int i = 0; i < data.Count; i++)
            {
                foreach (int dicKey in data[i].Values)
                {
                    if (key != 0)
                    {
                        bms.TotalNoteCount++;
                    }
                }
            }
        }

        return data;
    }

    // 현재 음악 정보파일
    public Bms CurrentBms { set; get; }
}