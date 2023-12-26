using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase]
public class MovingPlatform : MonoBehaviour
{
    public enum MovingPlatformType
    {
        BACK_FORTH,
        LOOP,
        ONCE
    }
    protected Player player;
    protected bool Playerisin;
    public float speed = 1.0f;
    public float temp;

    public MovingPlatformType platformType;

    public bool startMovingOnlyWhenVisible;
    public bool isMovingAtStart = true;
    [HideInInspector]
    public Vector3[] localNodes = new Vector3[1];

    public float[] waitTimes = new float[1];

    public AnimationCurve[] speedCurve = new AnimationCurve[1];

    public bool[] usespeedCurve = new bool[1];

    public Vector3[] worldNode { get { return m_WorldNode; } }

    protected Vector3[] m_WorldNode;

    protected int m_Current = 0;
    protected int m_Next = 0;
    protected int m_Dir = 1;
    protected bool usingCurve = false;

    protected float m_WaitTime = -1.0f;

    //protected Rigidbody2D m_Rigidbody2D;
    protected Vector3 m_Velocity;

    protected bool m_Started = false;
    protected bool m_VeryFirstStart = false;

    protected Vector2 direction;
    protected float timer = 0f;
    public float startDis = 0f;
    protected float nowDis = 0;
    public float add;
    public Vector2 Velocity
    {
        get { return m_Velocity; }
    }
    protected void Reset()
    {
        //we always have at least a node which is the local position
        localNodes[0] = Vector3.zero;
        waitTimes[0] = 0;


    }

    protected void Start()
    {
        //  m_Rigidbody2D = GetComponent<Rigidbody2D>();
        // m_Rigidbody2D.isKinematic = true;

        //Allow to make platform only move when they became visible
        Renderer[] renderers = GetComponentsInChildren<Renderer>();
        for (int i = 0; i < renderers.Length; ++i)
        {
            /*var b = renderers[i].gameObject.AddComponent<VisibleBubbleUp>();
            b.objectBecameVisible = BecameVisible;*/
        }

        //we make point in the path being defined in local space so game designer can move the platform & path together
        //but as the platform will move during gameplay, that would also move the node. So we convert the local nodes
        // (only used at edit time) to world position (only use at runtime)
        m_WorldNode = new Vector3[localNodes.Length];
        for (int i = 0; i < m_WorldNode.Length; ++i)
            m_WorldNode[i] = transform.TransformPoint(localNodes[i]);

        Init();
        transform.position += (m_WorldNode[m_Next] - m_WorldNode[m_Current]) * startDis;
    }

    protected void Init()
    {
        m_Current = 0;
        m_Dir = 1;
        m_Next = localNodes.Length > 1 ? 1 : 0;
        usingCurve = usespeedCurve[m_Next];
        // chasingDis = (m_WorldNode[m_Next] - m_WorldNode[m_Current]).sqrMagnitude;//距离长度平方

        m_WaitTime = waitTimes[0];
        timer = 0f;
        m_VeryFirstStart = false;
        if (isMovingAtStart)
        {
            m_Started = !startMovingOnlyWhenVisible;
            m_VeryFirstStart = true;
        }
        else
            m_Started = false;
    }

    protected void FixedUpdate()
    {
        if (!m_Started)
            return;

        //no need to update we have a single node in the path
        if (m_Current == m_Next)
            return;

        if (m_WaitTime > 0)
        {
            m_WaitTime -= Time.deltaTime;
            return;
        }
        timer += Time.deltaTime;
        direction = m_WorldNode[m_Next] - transform.position;
        nowDis = direction.sqrMagnitude;

        if (nowDis < 0.01f)//切换节点
        {
            transform.position = m_WorldNode[m_Next];

            m_Current = m_Next;
            m_WaitTime = waitTimes[m_Current];

            timer = 0f;
            if (m_Dir > 0)
            {
                m_Next += 1;
                if (m_Next >= m_WorldNode.Length)
                { //we reach the end

                    switch (platformType)
                    {
                        case MovingPlatformType.BACK_FORTH:
                            m_Next = m_WorldNode.Length - 2;
                            m_Dir = -1;
                            break;
                        case MovingPlatformType.LOOP:
                            m_Next = 0;
                            break;
                        case MovingPlatformType.ONCE:
                            m_Next -= 1;
                            StopMoving();
                            break;
                    }
                }
            }
            else
            {
                m_Next -= 1;
                if (m_Next < 0)
                { //reached the beginning again

                    switch (platformType)
                    {
                        case MovingPlatformType.BACK_FORTH:
                            m_Next = 1;
                            m_Dir = 1;
                            break;
                        case MovingPlatformType.LOOP:
                            m_Next = m_WorldNode.Length - 1;
                            break;
                        case MovingPlatformType.ONCE:
                            m_Next += 1;
                            StopMoving();
                            break;
                    }
                }
            }
            usingCurve = usespeedCurve[m_Next];
            //chasingDis = (m_WorldNode[m_Next] - m_WorldNode[m_Current]).sqrMagnitude;//距离长度平方
        }

        if (!usingCurve)
        {
            m_Velocity = direction.normalized * speed * Time.fixedDeltaTime;
            transform.position = transform.position + m_Velocity;
            add = speed;
            if (Playerisin)
                player.AccelVelocity(direction, add);
           
        }
        else
        {
            add = Mathf.Lerp(0, speed, speedCurve[m_Next].Evaluate(timer));
            m_Velocity = direction.normalized * add * Time.fixedDeltaTime;
            if (Playerisin)
                player.AccelVelocity(direction, add);
            transform.position = transform.position + m_Velocity;
        }

    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player")&&! Playerisin)
        {
            player = Player.instance;
            temp = player.rigidbody2.gravityScale;
            Playerisin = true;
            player.SetGracity(0);

        }
    }
    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player") )
        {
            player = Player.instance;
            temp = player.rigidbody2.gravityScale;
            Playerisin = true;
            player.SetGracity(0);
           
        }
        Debug.Log(" PlatformOnTriggerEnter2Dplayer.SetGracity(0);");

    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Debug.Log(" PlatformOnTriggerExit2D");
        Playerisin = false;
        player.SetGracity(temp); 
        player.RestAddVelocity();
    }

    public void StartMoving()
    {
        m_Started = true;
    }

    public void StopMoving()
    {
        m_Started = false;
    }

    public void ResetPlatform()
    {
        transform.position = m_WorldNode[0];
        Init();
    }

    /* private void BecameVisible(VisibleBubbleUp obj)
     {
         if (m_VeryFirstStart)
         {
             m_Started = true;
             m_VeryFirstStart = false;
         }
     }*/
}
