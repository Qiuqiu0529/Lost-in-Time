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

    [Header("buff表盘")]
    public GameObject buffObject;

    // Start is called before the first frame update
    void Start()
    {
        buffObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        /* buff剩余时间 */
        float leftBuffTime = Player.instance.LeftBuffTime;

        /* 最大buff持续时间 */
        float maxBuffTime = Player.instance.MaxBuffTime;

        /* 没有buff的时候隐藏表盘 */
        if (leftBuffTime <= 0||maxBuffTime<=0)
        {
            buffObject.SetActive(false);
            return;
        }
        else
        {
            buffObject.SetActive(true);
            /* 根据buff剩余时间调整蓝色圆的填充角度 */
            buffImage.fillAmount = 1.0f * leftBuffTime / maxBuffTime;

            /* 根据buff剩余时间调整分针角度 */
            minuteHeaderImage.transform.localEulerAngles = new Vector3(0, 0, -90 + 360 * leftBuffTime / maxBuffTime);

        }

    }
}
