using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "FragmentList", menuName = "Custom/FragmentList", order = 2)]

public class FragmentList : RuntimeSet<Fragment>
{
    public Fragment[] GetFragmentsOfState(FragmentStates state)
    {
        List<Fragment> fragments = new List<Fragment>();
        foreach (Fragment fragment in CurrentItems)
        {
            if (fragment.GetCurrentState() == state)
            {
                fragments.Add(fragment);
            }
        }
        return fragments.ToArray();
    }

    public Fragment GetFragmentToSpawn()
    {
        Fragment[] readyToSpawn = GetFragmentsOfState(FragmentStates.Idling);

        if (readyToSpawn.Length == 0)
            return null;
        else
            return readyToSpawn[Random.Range(0, readyToSpawn.Length)];
    }
}
