using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseTest : MonoBehaviour
{
    public string StateGroup, StateName;
    public string nomEvent;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            print("test_launch");
        }
        if (Input.GetKeyDown("p"))
        {
        }
        if (Input.GetKeyDown("s"))
        {
        }

        if (Input.GetKeyDown("d"))
        {
        }
    }
}
