using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutoRightTargetZone : MonoBehaviour
{
    ShakeCamera shakeCamera;

    private void Start()
    {
        GameObject mainCamera = GameObject.FindWithTag("MainCamera");
        shakeCamera = mainCamera.GetComponent<ShakeCamera>();

        shakeCamera.SetInitialPosition(mainCamera.transform.position);
    }

    public void ThrowZombie(Collider2D collision)
    {
        GameObject go = collision.gameObject;
        go.GetComponent<BoxCollider2D>().enabled = false;
        go.GetComponent<TutoZombie>().hp -= 1;

        int ranForceX = Random.Range(1000, 1200);
        int ranForceY = Random.Range(600, 1200);
        int ranTorque = Random.Range(200, 400);

        AudioManager.instance.PlayRandomDamageSound();

        // 카메라 흔들기.
        StartCoroutine(shakeCamera.Shake(0.03f, 0.15f));

        Rigidbody2D zombieRigid = go.GetComponent<Rigidbody2D>();
        zombieRigid.AddForce(new Vector2(ranForceX, ranForceY));
        zombieRigid.AddTorque(ranTorque);

        StartCoroutine(DeleteZombieCoroutine(go));
    }

    IEnumerator DeleteZombieCoroutine(GameObject zombiePrefab)
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(zombiePrefab);
    }
}
