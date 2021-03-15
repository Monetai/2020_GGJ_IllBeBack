using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WwiseManager : MonoBehaviour
{
    private void Awake()
    {
        uint bankID; // Not used
        AkSoundEngine.LoadBank("Soundbank", AkSoundEngine.AK_DEFAULT_POOL_ID, out bankID);
    }

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent("Play_music", gameObject);
        AkSoundEngine.PostEvent("ambiance", gameObject);
        //AkSoundEngine.SetState("game_state", "game1A");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
