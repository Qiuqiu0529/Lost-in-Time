using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HandController : MonoBehaviour
{
    private enum Dir {ANTICLOCKWISE, CLOCKWISE }

    [Header("��Ҹ���")]
    public CatchPoint catchPoint;
    
    [Header("״̬����")]
    public bool canRotate;
    public int rotationDir;//1:˳ʱ�� 0:��ʱ��

    [Header("ʱ��")]
    public Transform hourHand;
    public float hourHandSpeed;

    [Header("����")]
    public Transform minuteHand;
    public float minuteHandSpeed;
    public float loadingStepSize;//���ٲ���
    public float extraSpeed;//��ҵ��ﰲȫ�������Ķ����ٶ�
    public float maxRotateAngle;


    [Header("�ۻ�����")]
    [SerializeField] private float totalRotateAngle = 0;//���ת���Ƕ�
    [SerializeField] private float totalIncreaseSpeed = 0;//��������ٶ�ֵ
    

    private void Awake()
    {
        //canRotate = true;//��ʼ״ָ̬�벻����ת��
        //catchPoint = GameObject.FindWithTag("CatchPoint").GetComponent<CatchPoint>();
    }

    void Start()
    {
        //���ݲ�ͬ�ؿ���ʼ�� rotationDir
        switch(SceneManager.GetActiveScene().buildIndex)
        {
            case 0:
                {
                    rotationDir = (int)Dir.CLOCKWISE;//˳ʱ��
                    break;
                }
            case 1:
                {
                    rotationDir = (int)Dir.ANTICLOCKWISE;//��ʱ��
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
            Debug.Log("��ת����");
            if (catchPoint.isInSafeArea == 1)//��ҵ��ﰲȫ��
            {
                Debug.Log("���ﰲȫ��");
                LoadSpeed();
            }
            float minuteRotateAngle = (minuteHandSpeed + catchPoint.isInSafeArea * totalIncreaseSpeed) * Time.deltaTime;
            float hourRotateAngle = (hourHandSpeed + catchPoint.isInSafeArea * totalIncreaseSpeed) * Time.deltaTime;
            totalRotateAngle += minuteRotateAngle;
            if (rotationDir == (int)Dir.CLOCKWISE)//˳ʱ��
            {
         
                hourHand.RotateAround(this.transform.position, Vector3.back, hourRotateAngle);
                minuteHand.RotateAround(this.transform.position, Vector3.back, minuteRotateAngle);
            }
            else//��ʱ��
            {
                hourHand.RotateAround(this.transform.position, Vector3.forward, hourRotateAngle);
                minuteHand.RotateAround(this.transform.position, Vector3.forward, minuteRotateAngle);
            }
        }
        if (totalRotateAngle >= maxRotateAngle)
        {
            canRotate = false;
            totalRotateAngle = 0;//��ת���Ƕ���0
        }
    }

    void LoadSpeed()
    {
        if (totalIncreaseSpeed <= extraSpeed)
            totalIncreaseSpeed += loadingStepSize;
    }

}
