using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoteObj : MonoBehaviour
{
    public float speed;
    public bool isStart = false;

    public int channel;
    public float noteTime;

    public float destroyPositionY;
    public float destroyDelayTime;

    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator Drop()
    {
        if (transform.position.y > destroyPositionY)
        {
            transform.Translate(Vector3.down * speed * Time.smoothDeltaTime);
        }
        else
        {
            Destroy(gameObject);
        }

        yield return null;
    }
}
