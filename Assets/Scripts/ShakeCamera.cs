using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    // 카메라 흔들기.
    private Vector3 initialPosition;

    private void Start()
    {
        //transform.position = new Vector3(0.5f, -14f, -10f);
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
