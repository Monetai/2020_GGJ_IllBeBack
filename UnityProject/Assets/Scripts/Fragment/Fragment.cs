using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FragmentHeight
{
    High,
    Middle,
    Low
}



public class Fragment : MonoBehaviour, IFSMController 
{
    public FragmentList list;
    public float moveSpeed = 2f;
    public FragmentHeight height;
    public FragmentTypeAudio typeAudio;
    public Vector3 SpawnPos { get; private set; }
    public Vector3 HeadingDir { get; private set; }
    public Vector3 InitialPosition { get; private set; }
    public Quaternion InitialRotation { get; private set; }

    public ObjectRotation FragmentRotation { get; set; }

    private FSM stateMachine;
    private bool isPaused;
    
    public float baseSinValue;

    public FragmentStates GetCurrentState()
    {
        return (FragmentStates)stateMachine.GetCurrentStateIndex();
    }

    void Awake()
    {
        FragmentRotation = GetComponent<ObjectRotation>();
        stateMachine = new FSM(this);

        stateMachine.AddState<Idling>((int)FragmentStates.Idling);
        stateMachine.AddState<GoToSpawn>((int)FragmentStates.GoToSpawn);
        stateMachine.AddState<Waiting>((int)FragmentStates.Waiting);
        stateMachine.AddState<Attacking>((int)FragmentStates.Attacking);
        stateMachine.AddState<GoToPlace>((int)FragmentStates.GoToPlace);
        stateMachine.AddState<Repaired>((int)FragmentStates.Repaired);
        stateMachine.AddState<Destroyed>((int)FragmentStates.Destroyed);
        stateMachine.AddState<GoUp>((int)FragmentStates.GoUp);
        stateMachine.AddState<GoToTornado>((int)FragmentStates.GoToTornado);

        stateMachine.Start((int)FragmentStates.GoUp);

        list.CurrentItems.Add(this);
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        if(!isPaused)
        {
            stateMachine.Update();
        }
    }

    void FixedUpdate()
    {
        if (!isPaused)
        {
            stateMachine.FixedUpdate();
        }
    }

    void LateUpdate()
    {
        if (!isPaused)
        {
            stateMachine.LateUpdate();
        }
    }

    public void BeginAttack(Vector3 spawnPosition, SpawnPos dir, float speed)
    {
        moveSpeed = speed;
        if(dir == global::SpawnPos.Left)
        {
            HeadingDir = Vector3.right;
        }
        else if(dir == global::SpawnPos.Right)
        {
            HeadingDir = Vector3.left;
        }

        FragmentHeight height = (FragmentHeight)Random.Range(0, 3);
        spawnPosition.z = PlayerControls.Instance.transform.position.z;
        switch (height)
        {
            case FragmentHeight.High:
                spawnPosition.y = 1.5f;
                break;
            case FragmentHeight.Middle:
                spawnPosition.y = 1;
                break;
            case FragmentHeight.Low:
                spawnPosition.y = 0.2f;
                break;
            default:
                break;
        }
        this.height = height;
        SpawnPos = spawnPosition;
        stateMachine.ChangeState((int)FragmentStates.GoToSpawn);
    }

    public void HitFragment()
    {
        switch (typeAudio)
        {
            case FragmentTypeAudio.jar:
                break;
            case FragmentTypeAudio.panneau:
                break;
            case FragmentTypeAudio.statue:
                break;
            case FragmentTypeAudio.tabouret:
                break;
        }


        stateMachine.ChangeState((int)FragmentStates.GoToPlace);
    }

    public void FragmentDestoyed()
    {
        stateMachine.ChangeState((int)FragmentStates.Destroyed);
    }

    public void PauseFragmentBehaviour()
    {
        isPaused = true;
    }

    public void ResumeFragmentBehaviour()
    {
        isPaused = false;
    }

    public void BakeInitialPosAndRot()
    {
        InitialPosition = transform.position;
        InitialRotation = transform.rotation;
    }

    public bool IsPaused()
    {
        return isPaused;
    }
}
