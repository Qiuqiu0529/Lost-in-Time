using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthCircle : MonoBehaviour
{
    [Header("HImage")]
    public Image hImage;

    [Header("MinuteHeader")]
    public Image minuteHeaderImage;

    public float maxLife = 100;
    
    // Update is called once per frame
    void Update()
    {
        float life = Player.instance.GetComponent<PLayerInteract>().life;
        
        /* 根据血量调整红色圆的填充角度 */
        hImage.fillAmount = 1.0f * life / maxLife;

        /* 根据血量调整分针角度 */
        minuteHeaderImage.transform.localEulerAngles = new Vector3(0, 0, -90 + 360 * life / maxLife);
    }
}
