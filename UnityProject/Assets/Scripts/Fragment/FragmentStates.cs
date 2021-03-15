using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FragmentStates
{
    Invalid = -1,
    GoUp,
    GoToTornado,
    Idling,
    Waiting,
    Attacking,
    GoToPlace,
    GoToSpawn,
    Repaired,
    Destroyed
}

public enum FragmentTypeAudio
{
    jar,
    tabouret,
    panneau,
    statue
}

public class GoUp : FSMState
{
    private Fragment controller;
    private Vector3 targetPos = Vector3.up * 2;
    private Vector3 basePos;
    private Vector3 target;
    float currentTime = 0f;
    float timeToMove = 1f;
    float heightCursor = 4;
    float cursorSpeed = 0.5f;
    bool moving = false;
    Vector3 posToBe;
    public override void OnEnter()
    {
        controller = fsm.GetController<Fragment>();
        target = controller.transform.position + (Vector3.up * 1.3f) * Random.Range(0.2f,1f);
        basePos = controller.transform.position;
    }

    public override void Update()
    {
        base.Update();
        heightCursor -= cursorSpeed * Time.deltaTime;

        if (controller.transform.position.y > heightCursor)
        {
            moving = true;
        }

        if (moving)
        {
            currentTime += Time.deltaTime;
            posToBe  = Vector3.Lerp(basePos, target, Mathf.Clamp01(currentTime / timeToMove));
            controller.transform.position = posToBe + Random.insideUnitSphere * Random.Range(0.0f, 0.05f);
        }

        if (heightCursor < 0)
        {
            ChangeState((int)FragmentStates.GoToTornado);
        }

    }
}

public class GoToTornado : FSMState
{
    private Fragment controller;
    private Vector3 targetPos;
    private Vector3 basePos;
    private Vector3 target;
    float currentTime = 0f;
    float timeToMove = 0.5f;
    float delay;
    float timeToDelay;
    bool moving = false;

    Vector3 initialPos;
    public override void OnEnter()
    {
        controller = fsm.GetController<Fragment>();
        initialPos = controller.transform.position;
        targetPos = Ghost.Instance.transform.position;
        float offset = Random.Range(-0.5f, 1.5f);
        float height = targetPos.y + offset;
        float radius = Random.Range(1f, 1.5f);
        target = targetPos;
        target.y = height;

        float value = Random.Range(0f, 6.28f);
        controller.baseSinValue = value;
        target.x = Mathf.Sin(value) * radius + Ghost.Instance.transform.position.x;
        target.z = Mathf.Cos(value) * radius + Ghost.Instance.transform.position.z;

        basePos = controller.transform.position;
        delay = Random.Range(0.5f, 3f);
    }

    float map(float s, float a1, float a2, float b1, float b2)
    {
        return b1 + (s - a1) * (b2 - b1) / (a2 - a1);
    }

    public override void Update()
    {
        base.Update();

        if(timeToDelay > delay )
        {
            moving = true;
        }
        else
        {
            timeToDelay += Time.deltaTime;
            controller.transform.position = initialPos + Random.insideUnitSphere * Random.Range(0.0f, 0.05f);
        }

        if (moving &&  currentTime <= timeToMove)
        {
            currentTime += Time.deltaTime;
            controller.transform.position = Vector3.Lerp(basePos, target, currentTime / timeToMove);
        }
        else if(moving && currentTime >= timeToMove)
        {
            ChangeState((int)FragmentStates.Idling);
        }
    }
}

public class Idling : FSMState
{
    private Fragment controller;
    private Vector3 targetPos = Vector3.up*2;
    private Vector3 target;
    float radius;
    float value = 0f;
    float speed = 0f;
    public override void OnEnter()
    {
        controller = fsm.GetController<Fragment>();
        FragmentSpawner.Instance.canSpawn = true;

        Vector3 reference = Ghost.Instance.transform.position;
        reference.y = controller.transform.position.y;
        radius = Vector3.Distance(reference, controller.transform.position);
        target = targetPos + Random.insideUnitSphere;
        speed = Random.Range(4f, 6f);
    }

    public override void Update()
    {
        base.Update();
        controller.baseSinValue += speed * Time.deltaTime;
        target.x = Mathf.Sin(controller.baseSinValue) * radius + Ghost.Instance.transform.position.x;
        target.z = Mathf.Cos(controller.baseSinValue) * radius + Ghost.Instance.transform.position.z;

        controller.transform.position = target;
    }
}

public class GoToSpawn : FSMState
{
    private Fragment controller;
    private Vector3 basePos;
    private Vector3 target;
    private Vector3 p1;
    float currentTime = 0f;
    float timeToMove = 2f;

    public override void OnEnter()
    {
        controller = fsm.GetController<Fragment>();
        controller.FragmentRotation.enabled = true;
        target = controller.SpawnPos;
        basePos = controller.transform.position;

        Vector3 offset = -controller.HeadingDir * 1.5f;
        offset.y += 4.5f;
        p1 = target + offset;
    }

    public override void Update()
    {
        base.Update();

        currentTime += Time.deltaTime;
        if (currentTime > timeToMove)
        {
            controller.transform.position = target;
            fsm.ChangeState((int)FragmentStates.Attacking);
        }
        else
        {
            float t = currentTime / timeToMove;
            //controller.transform.position = Vector3.Lerp(basePos, target, t);
            controller.transform.position = BezierHelper.Bezier(target, p1, basePos, t);
        }
    }
}

public class Waiting : FSMState
{

}

public class Attacking : FSMState
{
    private Fragment controller;

    private float m_Ampl;
    private float m_Freq;

    public override void OnEnter()
    {
        controller = fsm.GetController<Fragment>();
        controller.transform.position = controller.SpawnPos;

        m_Ampl = Random.Range(1f, 1.5f);
        if (controller.height == FragmentHeight.Low)
        {
            m_Ampl = Random.Range(0.2f, 0.5f);
        }
        else if (controller.height == FragmentHeight.High)
        {
            m_Ampl = Random.Range(0.3f, 1f);
        }
        m_Freq = Random.Range(7f, 10f);
    }

    public override void Update()
    {
        base.Update();
        Vector3 sinOffset = new Vector3(0f, Mathf.Sin(Time.time * m_Freq) * m_Ampl * Time.deltaTime, 0f);

        Vector3 offset = controller.HeadingDir * controller.moveSpeed * Time.deltaTime;
        Vector3 newPosition = controller.transform.position + offset + sinOffset;

        controller.transform.position = newPosition;
    }

    public override void OnExit()
    {
        controller.FragmentRotation.enabled = false;
        base.OnExit();
    }
}

public class GoToPlace : FSMState
{
    private Fragment controller;
    float currentTime = 0f;
    float timeToMove = 1f;
    private Vector3 p1;
    private Vector3 basePos;
    private Quaternion baseRotation;

    public override void OnEnter()
    {
        base.OnEnter();
        controller = fsm.GetController<Fragment>();
        controller.FragmentRotation.enabled = false;

        basePos = controller.transform.position;
        baseRotation = controller.transform.rotation;
        Vector3 movement = (controller.InitialPosition - basePos);
        Vector3 upVector = new Vector3(0f, Random.Range(3f, 5.2f), 0f);
        p1 = basePos + (movement / 2f) + upVector;
    }


    public override void Update()
    {
        base.Update();
        currentTime += Time.deltaTime;
        if (currentTime > timeToMove)
        {
            controller.transform.position = controller.InitialPosition;
            controller.transform.rotation = controller.InitialRotation;
            ChangeState((int)FragmentStates.Repaired);
        }
        else
        {
            float t = currentTime / timeToMove;
            //controller.transform.position = Vector3.Lerp(basePos, target, t);
            controller.transform.position = BezierHelper.Bezier(controller.InitialPosition, p1, basePos, t);
            controller.transform.rotation = Quaternion.Slerp(baseRotation, controller.InitialRotation, t);
        }
    }
}

public class Repaired : FSMState
{
    public override void OnEnter()
    {
        base.OnEnter();

        switch (fsm.GetController<Fragment>().typeAudio)
        {
            case FragmentTypeAudio.jar:
                AkSoundEngine.PostEvent("debris_fixed_jar", fsm.GetController<Fragment>().gameObject);

                break;
            case FragmentTypeAudio.panneau:
                AkSoundEngine.PostEvent("debris_fixed_panneau", fsm.GetController<Fragment>().gameObject);
                break;
            case FragmentTypeAudio.statue:
                AkSoundEngine.PostEvent("debris_fixed_statue", fsm.GetController<Fragment>().gameObject);

                break;
            case FragmentTypeAudio.tabouret:
                AkSoundEngine.PostEvent("debris_fixed_tabouret", fsm.GetController<Fragment>().gameObject);
                    
                break;

        }











    }
}

public class Destroyed : FSMState
{
    private Fragment controller;
    private Vector3 targetPos;
    private Vector3 basePos;
    private Vector3 p1;
    float currentTime = 0f;
    float timeToMove = 0.7f;

    public override void OnEnter()
    {
        controller = fsm.GetController<Fragment>();
        controller.FragmentRotation.enabled = true;
        targetPos = controller.SpawnPos;
        basePos = controller.transform.position;
        targetPos = basePos + new Vector3(Random.Range(-0.3f, 0.3f), 0, -5f);
        targetPos.y = -0.5f;

        Vector3 dir = (targetPos - basePos);
        p1 = basePos + (dir / 2f) + new Vector3(0f, 5f, 0f);
    }

    public override void Update()
    {
        base.Update();

        currentTime += Time.deltaTime;
        if (currentTime > timeToMove)
        {
            controller.transform.position = targetPos;
        }
        else
        {
            float t = currentTime / timeToMove;
            //controller.transform.position = Vector3.Lerp(basePos, targetPos, t);
            controller.transform.position = BezierHelper.Bezier(targetPos, p1, basePos, t);
        }
    }

}
