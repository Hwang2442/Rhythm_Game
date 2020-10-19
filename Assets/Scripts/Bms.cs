using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bms : MonoBehaviour
{
    // BMS 파일을 담을 클래스

    private void Start()
    {
        BarDataList = new List<BarData>();
    }

    // 제목
    public string Title { set; get; }

    // 가수
    public string Artist { set; get; }

    // 노트 리스트
    public List<BarData> BarDataList { set; get; }

    // 분당 비트 수 (Beat Per Minute)
    public float Bpm { set; get; }

    // 전체 노트 개수
    public int TotalNoteCount { set; get; }

    // 총 막대 갯수
    public int TotalBarCount { set; get; }

    // 플레이 시간
    public float TotalPlayTime { set; get; }

    // 롱타입
    public int LongType { set; get; }

    public void debug()
    {
        print("title = " + Title);
        print("artist =" + Artist);
        print("bpm = " + Bpm);
        print("longNoteType = " + LongType);
        print("totalBarCount = " + BarDataList);
        print("totalNoteCount = " + TotalNoteCount);
        print("totalPlayTime = " + TotalPlayTime);
    }
}
