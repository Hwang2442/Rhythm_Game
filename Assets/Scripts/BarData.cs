using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarData : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public int Bar { set; get; }

    public int Channel { set; get; }

    public List<Dictionary<int, float>> NoteDataList { set; get; }

    public void debug()
    {
        print("bar = " + Bar);
        print("channel = " + Channel);

        foreach (Dictionary<int, float> noteData in NoteDataList)
        {
            foreach (int key in noteData.Keys)
            {
                if (key != 0)
                {
                    print("note key = " + key + ", time = " + noteData[key]);
                }
            }
        }
    }
}
