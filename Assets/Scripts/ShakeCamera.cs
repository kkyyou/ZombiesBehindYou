using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    // 카메라 흔들기.
    public float shakeAmount;

    // public Canvas canvas;
    private float shakeTime;
    private Vector3 initialPosition;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (shakeTime > 0)
        {
            transform.position = Random.insideUnitSphere * shakeAmount + initialPosition;
            shakeTime -= Time.deltaTime;
        }
        else
        {
            shakeTime = 0.0f;
            transform.position = initialPosition;
            // canvas.renderMode = RenderMode.ScreenSpaceCamera;
        }
    }

    public void VibrateForTime(float time)
    {
        shakeTime = time;

        // canvas.renderMode = RenderMode.ScreenSpaceCamera;
        // canvas.renderMode = RenderMode.WorldSpace;
    }

    public void SetInitialPosition(Vector3 init)
    {
        initialPosition = init;
    }
}
