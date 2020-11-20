using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RightTargetZone : MonoBehaviour
{
    private bool beAttackedRight = false;

    ShakeCamera shakeCamera;

    private void Start()
    {
        shakeCamera = GameObject.FindWithTag("MainCamera").GetComponent<ShakeCamera>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Zombie(Clone)" && beAttackedRight)
        {
            collision.gameObject.GetComponent<BoxCollider2D>().enabled = false;

            int ranForceX = Random.Range(1000, 1200);
            int ranForceY = Random.Range(600, 800);
            int ranTorque = Random.Range(200, 400);

            // 좀비 맞는 사운드 랜덤 재생.
            AudioManager.instance.PlayRandomDamageSound();

            // 카메라 흔들기.
            shakeCamera.VibrateForTime(0.1f);

            Rigidbody2D zombieRigid = collision.gameObject.GetComponent<Rigidbody2D>();
            zombieRigid.AddForce(new Vector2(ranForceX, ranForceY));
            zombieRigid.AddTorque(ranTorque);

            // 점수 획득.
            GameManager.instance.AddScore(1);

            // Hp 회복.
            GameManager.instance.RecoveryHP(10);

            // 리스트에서 해당 좀비 프리팹 삭제.
            EnemyManager.instance.RemoveZombie(collision.gameObject);
            StartCoroutine(DeleteZombieCoroutine(collision.gameObject));
        }

        beAttackedRight = false;
    }

    IEnumerator DeleteZombieCoroutine(GameObject zombiePrefab)
    {
        yield return new WaitForSeconds(1f);
        Destroy(zombiePrefab);
    }

    public void setBeAttackedRight(bool attack)
    {
        beAttackedRight = attack;
    }

    public bool getBeAttackedRight()
    {
        return beAttackedRight;
    }
}
