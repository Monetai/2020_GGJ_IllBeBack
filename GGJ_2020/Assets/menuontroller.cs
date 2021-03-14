using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.SceneManagement;
using UnityEngine.Timeline;
public class menuontroller : MonoBehaviour
{
    public static menuontroller Instance;
    public PlayableDirector playableDirector;
    public PlayableDirector endcinematic;

    public GameObject ghost;
    public GameObject virtualCamera;
    bool menu;
    public bool canplay = false;
    public FragmentList list;
    private void Awake()
    {
        Instance = this;
        canplay = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        FragmentStopper.Instance.StopFragments();
        AkSoundEngine.SetState("game_state", "start_menu");
    }

    // Update is called once per frame
    void Update()
    {
        if (!menu && Input.anyKeyDown)
        {
            AkSoundEngine.PostEvent("cine_intro", gameObject);
            menu = true;
            playableDirector.Play();
            Invoke("ActivateCamera",10f);
            ghost.SetActive(true);
            FragmentStopper.Instance.UnStopFragments();
        }
    }

    void ActivateCamera()
    {
        canplay = true;
        virtualCamera.SetActive(true);
    }

    public void EndCinematic()
    {
        canplay = false;
        AkSoundEngine.PostEvent("cine_end", gameObject);
        AkSoundEngine.SetState("game_state", "None");
        list.CurrentItems.Clear();
        endcinematic.enabled = true;
        endcinematic.Play();
        ghost.GetComponent<Animator>().SetTrigger("GoBack");
        Invoke("Reload", 5f);
    }

    void Reload()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
