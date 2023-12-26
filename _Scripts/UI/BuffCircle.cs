using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuffCircle : MonoBehaviour
{
    [Header("BuffImage")]
    public Image buffImage;

    [Header("MinuteHeader")]
    public Image minuteHeaderImage;

    [Header("buff����")]
    public GameObject buffObject;

    // Start is called before the first frame update
    void Start()
    {
        buffObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /* buffʣ��ʱ�� */
        float leftBuffTime = Player.instance.LeftBuffTime;

        /* ���buff����ʱ�� */
        float maxBuffTime = Player.instance.MaxBuffTime;

        /* û��buff��ʱ�����ر��� */
        if (leftBuffTime <= 0||maxBuffTime<=0)
        {
            buffObject.SetActive(false);
            return;
        }
        else
        {
            buffObject.SetActive(true);
            /* ����buffʣ��ʱ�������ɫԲ�����Ƕ� */
            buffImage.fillAmount = 1.0f * leftBuffTime / maxBuffTime;

            /* ����buffʣ��ʱ���������Ƕ� */
            minuteHeaderImage.transform.localEulerAngles = new Vector3(0, 0, -90 + 360 * leftBuffTime / maxBuffTime);

        }

    }
}
