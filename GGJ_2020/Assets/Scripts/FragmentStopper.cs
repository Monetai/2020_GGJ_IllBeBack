using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FragmentStopper : MonoBehaviour
{
    public static FragmentStopper Instance;
    public FragmentList list;

    private void Awake()
    {
        Instance = this;
    }

    public void StopFragments()
    {
        foreach(Fragment frag in list.CurrentItems)
        {
            frag.PauseFragmentBehaviour();
        }
    }

    public void UnStopFragments()
    {
        foreach (Fragment frag in list.CurrentItems)
        {
            frag.ResumeFragmentBehaviour();
        }
    }
}
