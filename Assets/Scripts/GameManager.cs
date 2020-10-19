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

    Bms bms;

    // Start is called before the first frame update
    void Start()
    {
        bms = GetComponent<Bms>();

        // BMS 파일 파싱
        BmsParsing();

        bms.debug();

        // 시작선

        // 판정선

        // 노트 프리팹 생성, 리스트 저장
        
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

                    barData = gameObject.AddComponent<BarData>();
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