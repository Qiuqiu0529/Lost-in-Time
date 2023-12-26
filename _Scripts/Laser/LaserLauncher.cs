using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserLauncher : MonoBehaviour
{
    [SerializeField] private Camera camera;
    public LineRenderer lineRenderer;
    public Transform firePoint;
    public GameObject startVFX;
    public GameObject endVFX;

    private Quaternion rotation;
    private List<ParticleSystem> particles = new List<ParticleSystem>();

    void Start()
    {
        FillLists();
        DisableLaser();//游戏开始时禁用
    }


    void Update()
    {
        if(Input.GetButtonDown("Fire1"))
        {
            EnableLaser();
        }
        if(Input.GetButton("Fire1"))
        {
            UpdateLaser();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            DisableLaser();
        }
        RotateToMouse();
    }


    void EnableLaser()
    {
        lineRenderer.enabled = true;

        for(int i=0;i<particles.Count;i++)
        {
            particles[i].Play();
        }
    }

    void UpdateLaser()
    {
        //Debug.Log(camera.ScreenToWorldPoint(Input.mousePosition));
        var mousePos = (Vector2)camera.ScreenToWorldPoint(Input.mousePosition);

        lineRenderer.SetPosition(0, (Vector2)firePoint.position);
        lineRenderer.SetPosition(1, mousePos);

        startVFX.transform.position = (Vector2)firePoint.position;//设置起始粒子坐标

        Vector2 direction = mousePos - (Vector2)transform.position;
        RaycastHit2D hit = Physics2D.Raycast((Vector2)transform.position, direction.normalized, direction.magnitude);

        if(hit)
        {
            lineRenderer.SetPosition(1, hit.point);
        }

        endVFX.transform.position = lineRenderer.GetPosition(1);//设置终止粒子坐标
    }

    void DisableLaser()
    {
        lineRenderer.enabled = false;

        for (int i = 0; i < particles.Count; i++)
        {
            particles[i].Stop();
        }
    }

    void RotateToMouse()
    {
        Vector2 direction = camera.ScreenToWorldPoint(Input.mousePosition) - transform.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        rotation.eulerAngles = new Vector3(0, 0, angle);

        transform.rotation = rotation;
    }

    void FillLists()
    {
        for(int i=0;i<startVFX.transform.childCount;i++)
        {
            var ps = startVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if(ps!=null)
            {
                particles.Add(ps);
            }
        }

        for (int i = 0; i < endVFX.transform.childCount; i++)
        {
            var ps = endVFX.transform.GetChild(i).GetComponent<ParticleSystem>();
            if (ps != null)
            {
                particles.Add(ps);
            }
        }
    }
}
