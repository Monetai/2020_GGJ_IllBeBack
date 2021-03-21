using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum HitDirection
{
    HitLeft,
    HitRight
}


public class PlayerControls : MonoBehaviour
{
    public static PlayerControls Instance;

    public static int voLevel = 1;

    [SerializeField] Animator m_Animator;
    [SerializeField] float m_HitAnimationDuration;
    private bool m_HitState = false;
    private float m_HitAnimationTimer = 0f;
    private bool m_CanHit = true;
    [SerializeField] float m_FailAnimationDuration;
    private float m_FailAnimationTimer = 0f;
    private bool m_FailState = false;

    private bool toggleMusicGame = true;

    [SerializeField] Collider m_Collider;
    [SerializeField] private float m_DistanceToTouch = 0.1f;

    [SerializeField] private FragmentList m_FragmentsList;

    [SerializeField] private Transform m_HitZoneLeft;
    [SerializeField] private Transform m_HitZoneRight;

    [SerializeField] float m_RoomMinX;
    [SerializeField] float m_RoomMaxX;

    [SerializeField] Transform m_HitEffectParent;
    [SerializeField] GameObject m_HitEffect;

    [SerializeField] GameObject m_DestroyEffect;

    private Fragment[] m_ActiveFragments;
    public GameObject characterMesh;

    public int NbHits = 0;
    public int NbFail = 0;

    public Material normal;
    public Material pasContent;
    public SkinnedMeshRenderer head;
    private void Awake()
    {
        Instance = this;
    }

    /*
    private void ConfigureHitZones()
    {
        Vector3 hitZoneScale = new Vector3(m_DistanceToTouch, 0.2f, 1f);

        m_HitZoneLeft.position = transform.position + new Vector3(-m_DistanceToTouch / 2f, 0f, 0f);
        m_HitZoneLeft.localScale = hitZoneScale;
        m_HitZoneRight.position = transform.position + new Vector3(m_DistanceToTouch / 2f, 0f, 0f);
        m_HitZoneRight.localScale = hitZoneScale;

        m_HitZoneLeft.position = transform.position;

        m_HitZoneRight.position = transform.position;
    }
    */

    void Update()
    {
        if (!menuontroller.Instance.canplay)
            return;

        //ConfigureHitZones();
        m_HitZoneLeft.gameObject.SetActive(false);
        m_HitZoneRight.gameObject.SetActive(false);

        m_ActiveFragments = m_FragmentsList.GetFragmentsOfState(FragmentStates.Attacking);

        for (int i = 0; i < m_ActiveFragments.Length; ++i)
        {
            Fragment fragment = m_ActiveFragments[i];

            if (fragment.IsPaused())
                return;

            // Test if fragment hit the player
            if (Vector3.Distance(m_Collider.ClosestPoint(fragment.transform.position), fragment.transform.position) < 0.001f)
            {
                DestroyFragment(fragment);
            }

            // Show hitzones
            //float targetDirection = Mathf.Sign((fragment.transform.position - transform.position).x);
            //Vector3 hitPosition = m_Collider.ClosestPoint(fragment.transform.position);
            //float distance = Vector3.Distance(hitPosition, fragment.transform.position);
            //if (distance < m_DistanceToTouch)
            //{
            //    if (targetDirection < 0f)
            //        m_HitZoneLeft.gameObject.SetActive(true);
            //    if (targetDirection > 0f)
            //        m_HitZoneRight.gameObject.SetActive(true);
            //}

        }

        // animation management
        if (m_HitState)
        {
            m_HitAnimationTimer += Time.deltaTime;

            if (m_HitAnimationTimer > m_HitAnimationDuration)
            {
                head.material = normal;

                m_HitState = false;
                m_HitAnimationTimer = 0f;
                m_Animator.SetTrigger("Idle");
                m_CanHit = true;
            }
        }
        if (m_FailState)
        {
            m_FailAnimationTimer += Time.deltaTime;
            if (m_FailAnimationTimer > m_FailAnimationDuration)
            {
                head.material = normal;

                m_FailState = false;
                m_FailAnimationTimer = 0f;
                m_Animator.SetTrigger("Idle");
            }
        }

        if (m_CanHit)
        {
            if (Input.GetButtonDown("Fire3"))
            {
                Fragment fragment = Hit(HitDirection.HitLeft);
                transform.LookAt(transform.position + new Vector3(-1, 0, 0));
                Vector3 scale = transform.localScale;
                if (scale.x < 0)
                {
                    scale.x *= -1;
                }
                transform.localScale = scale;
                StartHitAnimation(fragment);
            }
            if (Input.GetButtonDown("Fire2"))
            {
                Fragment fragment = Hit(HitDirection.HitRight);
                transform.LookAt(transform.position + new Vector3(1, 0, 0));
                Vector3 scale = transform.localScale;
                if (scale.x > 0)
                {
                    scale.x *= -1;
                }
                transform.localScale = scale;
                StartHitAnimation(fragment);
            }
        }
    }

    public Fragment Hit(HitDirection direction)
    {
        head.material = pasContent;
        m_CanHit = false;
        for (int i = 0; i < m_ActiveFragments.Length; ++i)
        {
            Fragment fragment = m_ActiveFragments[i];
            float targetDirection = Mathf.Sign((fragment.transform.position - transform.position).x);
            if ((direction == HitDirection.HitLeft && targetDirection > 0f) || (direction == HitDirection.HitRight && targetDirection < 0f))
                continue;

            Vector3 hitPosition = m_Collider.ClosestPoint(fragment.transform.position);
            float distance = Vector3.Distance(hitPosition, fragment.transform.position);
            if (distance < m_DistanceToTouch)
            {
                HitFragment(fragment);
                return fragment;
            }
        }
        return null;
    }

    private void HitFragment(Fragment fragment)
    {
        voLevel = 2;
        Vector3 newPos = transform.position;
        Vector3 fragPos = fragment.transform.position;
        Vector3 dir;
        if (fragment.transform.position.x < newPos.x)
        {
            newPos.x -= (0.5f * (m_RoomMinX - newPos.x) / m_RoomMinX);
            fragPos.x = newPos.x + Vector3.left.x * 0.75f;
            fragment.transform.position = fragPos;
            dir = Vector3.left;
        }
        else
        {
            newPos.x += (0.5f * (m_RoomMaxX - newPos.x) / m_RoomMaxX);
            fragPos.x = newPos.x + Vector3.right.x * 0.75f;
            fragment.transform.position = fragPos;
            dir = Vector3.right;
        }

        newPos.x = Mathf.Clamp(newPos.x, m_RoomMinX, m_RoomMaxX);
        Instantiate(m_HitEffect, fragment.transform.position, Quaternion.identity);
        
        transform.position = newPos;

        //if(Random.Range(0,100) < 10)
        if (fragment.typeAudio == FragmentTypeAudio.panneau || fragment.typeAudio == FragmentTypeAudio.tabouret || Random.Range(0, 100) < 10)
        {
            voLevel = 3;
            CameraEffectManager.Instance.Zoom(dir, () => 
            { 
                fragment.HitFragment(); 
                SetStateMusicGame(); 
            });
        }
        else
        {
            fragment.HitFragment();
            TimeEffectManager.Instance.SetTimeScale(0.75f, 1f);
        }
        m_CanHit = true;
        NbHits++;
    }

    private void DestroyFragment(Fragment fragment)
    {
        head.material = pasContent;

        Instantiate(m_DestroyEffect, fragment.transform.position, Quaternion.identity);
        fragment.FragmentDestoyed();

        // animations
        m_Animator.SetTrigger("Fail");
        m_Animator.ResetTrigger("Idle");
        m_FailState = true;
        m_FailAnimationTimer = 0f;
        NbFail++;
    }

    private void StartHitAnimation(Fragment fragment)
    {
        switch (voLevel)
        {
            case 1:
                break;
            case 2:
                voLevel = 1;
                break;
            case 3:
                voLevel = 1;
                break;
        }


        string[] hitAnimations = new string[] { "AttackBottom1", "AttackBottom2", "AttackMiddle1", "AttackMiddle2", "AttackTop1", "AttackTop2" };
        string animationToStart = hitAnimations[Random.Range(0, hitAnimations.Length)];

        string[] hitBottomAnimations = new string[] { "AttackBottom1", "AttackBottom2" };
        string[] hitMiddleAnimations = new string[] { "AttackMiddle1", "AttackMiddle2" };
        string[] hitTopAnimations = new string[] { "AttackTop1", "AttackTop2" };

        if (fragment != null)
        {
            if (fragment.height == FragmentHeight.High)
            {
                animationToStart = hitTopAnimations[Random.Range(0, hitTopAnimations.Length)];
            }
            if (fragment.height == FragmentHeight.Middle)
            {
                animationToStart = hitMiddleAnimations[Random.Range(0, hitMiddleAnimations.Length)];
            }
            if (fragment.height == FragmentHeight.Low)
            {
                animationToStart = hitBottomAnimations[Random.Range(0, hitBottomAnimations.Length)];
            }
        }


        m_Animator.SetTrigger(animationToStart);
        m_Animator.ResetTrigger("Idle");
        m_HitState = true;
        m_HitAnimationTimer = 0f;
    }

    private void SetStateMusicGame()
    {

        print("dbg");
        if (toggleMusicGame)
        {
            print("dbg_game1a");
            toggleMusicGame = false;
        }
        else
        {
            print("dbg_game1b");
            toggleMusicGame = true;
        }
    }
}
