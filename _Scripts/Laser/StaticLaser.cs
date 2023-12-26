using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticLaser : MonoBehaviour
{
    public LineRenderer lineRenderer;
    public Transform startPoint;
    public Transform endPoint;

    private List<ParticleSystem> particles = new List<ParticleSystem>();

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log("激光脚本调用");
        EnableLaser();// 游戏开始时启用
    }

    // Update is called once per frame
    void Update()
    {
        UpdateLaser();
    }


    void EnableLaser()
    {
        lineRenderer.enabled = true;

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Play();
        }
    }

    void UpdateLaser()
    {

        lineRenderer.SetPosition(0, startPoint.position);
        lineRenderer.SetPosition(1, endPoint.position);


        Vector2 direction = endPoint.position - startPoint.position;
        RaycastHit2D hit = Physics2D.Raycast(startPoint.position, direction.normalized, direction.magnitude);

        if (hit)
        {
            if (hit.collider.CompareTag ( "Player"))
                Debug.Log("物体进入,角色死亡");//此处为玩家触碰到激光后的表现
            
        }

        endPoint.transform.position = lineRenderer.GetPosition(1);//设置终止粒子坐标
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
    }

}
