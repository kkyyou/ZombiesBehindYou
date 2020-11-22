using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    // 카메라 흔들기.

    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        //if (shakeTime > 0)
        //{
        //    transform.position = Random.insideUnitSphere * shakeAmount + initialPosition;
        //    shakeTime -= Time.deltaTime;
        //}
        //else
        //{
        //    shakeTime = 0.0f;
        //    transform.position = initialPosition;
        //    // canvas.renderMode = RenderMode.ScreenSpaceCamera;
        //}
    }

    public IEnumerator Shake(float _amount, float _duration)
    {
        float timer = 0;
        while (timer <= _duration)
        {
            transform.localPosition = (Vector3)Random.insideUnitSphere * _amount + initialPosition;

            timer += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = initialPosition;
    }

    public void SetInitialPosition(Vector3 init)
    {
        initialPosition = init;
    }
}
