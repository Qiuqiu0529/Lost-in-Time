using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandController : MonoBehaviour
{
    private enum Dir {ANTICLOCKWISE, CLOCKWISE }

    [Header("玩家附着")]
    public CatchPoint catchPoint;
    
    [Header("状态参量")]
    public bool canRotate;
    public int rotationDir;//1:顺时针 0:逆时针

    [Header("时针")]
    public Transform hourHand;
    public float hourHandSpeed;

    [Header("分针")]
    public Transform minuteHand;
    public float minuteHandSpeed;
    public float loadingStepSize;//加速步长
    public float extraSpeed;//玩家到达安全区后分针的额外速度
    public float maxRotateAngle;


    [Header("累积变量")]
    [SerializeField] private float totalRotateAngle = 0;//检测转过角度
    [SerializeField] private float totalIncreaseSpeed = 0;//监测增加速度值
    

    private void Awake()
    {
        //canRotate = true;//起始状态指针不可以转动
        //catchPoint = GameObject.FindWithTag("CatchPoint").GetComponent<CatchPoint>();
    }

    void Start()
    {
        //根据不同关卡初始化 rotationDir
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                {
                    rotationDir = (int)Dir.CLOCKWISE;//顺时针
                    break;
                }
            case 1:
                {
                    rotationDir = (int)Dir.ANTICLOCKWISE;//逆时针
                    break;
                }
        }
    }


    void Update()
    {
        Rotate();
    }

    void Rotate()
    {
        if (canRotate)
        {
            Debug.Log("旋转运行");
            if (catchPoint.isInSafeArea == 1)//玩家到达安全区
            {
                Debug.Log("到达安全区");
                LoadSpeed();
            }
            float minuteRotateAngle = (minuteHandSpeed + catchPoint.isInSafeArea * totalIncreaseSpeed) * Time.deltaTime;
            float hourRotateAngle = (hourHandSpeed + catchPoint.isInSafeArea * totalIncreaseSpeed) * Time.deltaTime;
            totalRotateAngle += minuteRotateAngle;
            if (rotationDir == (int)Dir.CLOCKWISE)//顺时针
            {
         
                hourHand.RotateAround(this.transform.position, Vector3.back, hourRotateAngle);
                minuteHand.RotateAround(this.transform.position, Vector3.back, minuteRotateAngle);
            }
            else//逆时针
            {
                hourHand.RotateAround(this.transform.position, Vector3.forward, hourRotateAngle);
                minuteHand.RotateAround(this.transform.position, Vector3.forward, minuteRotateAngle);
            }
        }
        if (totalRotateAngle >= maxRotateAngle)
        {
            canRotate = false;
            totalRotateAngle = 0;//将转过角度置0
        }
    }

    void LoadSpeed()
    {
        if (totalIncreaseSpeed <= extraSpeed)
            totalIncreaseSpeed += loadingStepSize;
    }

}
