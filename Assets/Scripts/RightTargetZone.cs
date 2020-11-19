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

            Rigidbody2D zombieRigid = collision.gameObject.GetComponent<Rigidbody2D>();

            // 좀비 맞는 사운드 랜덤 재생.
            AudioManager.instance.PlayRandomDamageSound();

            shakeCamera.VibrateForTime(0.1f);

            zombieRigid.AddForce(new Vector2(ranForceX, ranForceY));
            zombieRigid.AddTorque(ranTorque);

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
