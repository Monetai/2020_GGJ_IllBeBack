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
        AkSoundEngine.PostEvent("ambiance", gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown("t"))
        {
            print("test_launch");
            AkSoundEngine.SetState("StateGroup", "StateName");
        }
        if (Input.GetKeyDown("p"))
        {
            AkSoundEngine.PostEvent(nomEvent.ToString(), gameObject);
        }
        if (Input.GetKeyDown("s"))
        {
            AkSoundEngine.PostEvent("slowmo_start", gameObject);
            AkSoundEngine.SetRTPCValue("slow_mo", 1);
        }

        if (Input.GetKeyDown("d"))
        {
            AkSoundEngine.PostEvent("slowmo_stop", gameObject);
            AkSoundEngine.SetRTPCValue("slow_mo", 0);
        }
    }
}
