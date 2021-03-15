using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class FragmentWizard : ScriptableWizard
{
    public FragmentList list;
    public Material firstMat;
    public Material secondMat;

    public ObjectRotation rotation;


    [MenuItem("Fragment/Setup fragment in scene")]
    static void CreateWizard()
    {
        ScriptableWizard.DisplayWizard<FragmentWizard>("Setup Fragment", "Create", "Apply");
        //If you don't want to use the secondary button simply leave it out:
        //ScriptableWizard.DisplayWizard<WizardCreateLight>("Create Light", "Create");
    }

    void OnWizardCreate()
    {

    }

    void OnWizardUpdate()
    {

    }

    // When the user presses the "Apply" button OnWizardOtherButton is called.
    void OnWizardOtherButton()
    {
        Fragment[] fragments = FindObjectsOfType<Fragment>();

        foreach(Fragment frag in fragments)
        {
            frag.list = list;
            frag.BakeInitialPosAndRot();

            frag.GetComponent<MeshRenderer>().material = firstMat;

            ObjectRotation rot = frag.gameObject.GetComponent<ObjectRotation>();
            if (rot == null)
            {
                frag.FragmentRotation = frag.gameObject.AddComponent<ObjectRotation>();
            }
            else
            {
                rot.m_minXRotation = 600;
                rot.m_minYRotation = 600;
                rot.m_minZRotation = 600;

                rot.m_maxXRotation = 2500;
                rot.m_maxYRotation = 2500;
                rot.m_maxZRotation = 2500;

                rot.enabled = false;
                frag.FragmentRotation = rot;
            }
        }
    }
}