using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBG : MonoBehaviour
{
    Transform cameraTrans;
    Vector3 lastCameraPos;
    Vector3 InitDistance;
    float textureUnitSizeX;
    float textureUnitSizeY;
    [SerializeField] Vector2 paraEffect;
    [SerializeField] bool infiniteHori,infiniteVerti,LockY;
    public float posZ;
    void Start()
    {
        cameraTrans = Camera.main.transform;
        lastCameraPos = cameraTrans.position;
        InitDistance = lastCameraPos - transform.position;
        posZ = transform.position.z;
        Sprite sprite = GetComponent<SpriteRenderer>().sprite;
        Texture2D texture = sprite.texture;
        textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
        Debug.Log( textureUnitSizeX);
        textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        Debug.Log(textureUnitSizeY);
    }

    private void LateUpdate()
    {
        Vector3 deltaMove = cameraTrans.position - lastCameraPos;
        if (!LockY)
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x + paraEffect.x * deltaMove.x,
              transform.position.y + paraEffect.y * deltaMove.y, transform.position.z), 0.1f);
        }
        else
        {
            transform.position = Vector3.Slerp(transform.position, new Vector3(transform.position.x + paraEffect.x * deltaMove.x,
              cameraTrans.position.y-InitDistance.y, transform.position.z), 0.1f);
        }
        lastCameraPos = cameraTrans.position;
        if (infiniteHori)
        {
            if (Mathf.Abs(cameraTrans.position.x - transform.position.x) >= textureUnitSizeX)
            {
                Debug.Log("infiniteHori");
                float offsetPosX = (cameraTrans.position.x - transform.position.x)% textureUnitSizeX;
                transform.position = new Vector3(cameraTrans.position.x + offsetPosX,
                    transform.position.y, transform.position.z);
            }
        }
        if (infiniteVerti)
        {
            if (Mathf.Abs(cameraTrans.position.y - transform.position.y) >= textureUnitSizeY)
            {
                Debug.Log("infiniteVerti");
                float offsetPosY = (cameraTrans.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x,
                   cameraTrans.position.y + offsetPosY, transform.position.z);
            }
        }
        transform.position = new Vector3(transform.position.x,
                transform.position.y, posZ);
    }
}
