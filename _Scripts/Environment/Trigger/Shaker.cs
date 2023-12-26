using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public  class Shaker : MonoBehaviour
{
    protected Vector3 lastVector;
    protected float shakeTime = 0.0f;
    public float shakeIntensity = 0.2f;
    protected Vector3 startPos;
    private void Start()
    {
        startPos = transform.localPosition;
    }
    public void Shake(float amount, float time)
    {
        shakeTime = time;
        shakeIntensity = amount;
        StartCoroutine(Shaking());
    }
    public IEnumerator Shaking()
    {
        while (shakeTime > 0)
        {
            lastVector = Random.insideUnitCircle * shakeIntensity;
            transform.localPosition = startPos + lastVector;
            yield return new WaitForSeconds(Time.deltaTime);
            transform.localPosition = transform.localPosition - lastVector;
            shakeTime -= Time.deltaTime;
        }
        transform.localPosition = startPos;
    }
}
