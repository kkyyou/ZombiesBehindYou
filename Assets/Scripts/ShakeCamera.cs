using System.Collections;
using UnityEngine;

public class ShakeCamera : MonoBehaviour
{
    // 카메라 흔들기.
    private Vector3 initialPosition;

    private void Awake()
    {
        // 해상도 대응.
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)16 / 9); // (가로 / 세로)
        float scalewidth = 1f / scaleheight;
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
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

    void OnPreCull() => GL.Clear(true, true, Color.black);
}
