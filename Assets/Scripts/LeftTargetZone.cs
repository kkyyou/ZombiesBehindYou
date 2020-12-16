using System.Collections;
using UnityEngine;

public class LeftTargetZone : MonoBehaviour
{
    ShakeCamera shakeCamera;

    private void Start()
    {
        shakeCamera = GameObject.FindWithTag("MainCamera").GetComponent<ShakeCamera>();
    }

    public void ThrowZombie(Collider2D collision)
    {
        // 강철 좀비는 다른 사운드 재생하고 좀비 자체는 날리지 않고 투구만 날린다.
        bool isArmorZombie = false;
        Zombie collisionZombie = collision.GetComponent<Zombie>();
        isArmorZombie = collisionZombie.name == "ArmorZombie" ? true : false;

        if (!isArmorZombie)
        {
            GameObject go = collision.gameObject;
            go.GetComponent<BoxCollider2D>().enabled = false;
            go.GetComponent<Zombie>().hp -= 1;

            int ranForceX = Random.Range(-1000, -1200);
            int ranForceY = Random.Range(600, 1200);
            int ranTorque = Random.Range(-200, -400);

            // 좀비 맞는 사운드 랜덤 재생.
            if (GameManager.instance.GetListenSfx())
                AudioManager.instance.PlayRandomDamageSound();

            // 카메라 흔들기.
            StartCoroutine(shakeCamera.Shake(0.035f, 0.15f));

            Rigidbody2D zombieRigid = go.GetComponent<Rigidbody2D>();
            zombieRigid.AddForce(new Vector2(ranForceX, ranForceY));
            zombieRigid.AddTorque(ranTorque);

            // 씬 리스트에서 해당 좀비 프리팹 삭제.
            EnemyManager.instance.RemoveZombie(go);
        }
        else
        {
            int ranForceX = Random.Range(-1000, -1200);
            int ranForceY = Random.Range(600, 1200);
            int ranTorque = Random.Range(-200, -400);

            // 카메라 흔들기.
            StartCoroutine(shakeCamera.Shake(0.035f, 0.15f));

            GameObject armorZombieGO = collision.gameObject;
            Zombie armorZombie = armorZombieGO.GetComponent<Zombie>();
            armorZombie.hp -= 1;

            if (armorZombie.hp == 1)
            {
                //// 헬멧 깡! 하는 소리 재생.
                //if (GameManager.instance.GetListenSfx())
                //    AudioManager.instance.PlayRandomArmorAttackSound();

                // 좀비 맞는 사운드 랜덤 재생.
                if (GameManager.instance.GetListenSfx())
                    AudioManager.instance.PlayRandomDamageSound();

                // 헬멧 생성 해서 날리기.
                GameObject armorZombieHelmetObject = Instantiate(EnemyManager.instance.armorHelmetPrefab, Vector3.zero, Quaternion.identity);
                armorZombieHelmetObject.transform.position = new Vector3(collision.gameObject.transform.position.x, collision.gameObject.transform.position.y + 0.3f, collision.gameObject.transform.position.z);

                Rigidbody2D zombieHelmetRigid = armorZombieHelmetObject.GetComponent<Rigidbody2D>();
                zombieHelmetRigid.AddForce(new Vector2(ranForceX, ranForceY));
                zombieHelmetRigid.AddTorque(ranTorque);

                armorZombieGO.GetComponent<SpriteRenderer>().sprite = EnemyManager.instance.spriteZombie1;
                StartCoroutine(DeleteHelmetCoroutine(armorZombieHelmetObject));
            }
            else if (armorZombie.hp == 0)
            {
                armorZombieGO.GetComponent<BoxCollider2D>().enabled = false;

                // 좀비 맞는 사운드 랜덤 재생.
                if (GameManager.instance.GetListenSfx())
                    AudioManager.instance.PlayRandomDamageSound();

                // 카메라 흔들기.
                StartCoroutine(shakeCamera.Shake(0.035f, 0.15f));

                Rigidbody2D zombieRigid = armorZombieGO.GetComponent<Rigidbody2D>();
                zombieRigid.AddForce(new Vector2(ranForceX, ranForceY));
                zombieRigid.AddTorque(ranTorque);

                // 씬 리스트에서 해당 좀비 프리팹 삭제.
                EnemyManager.instance.RemoveZombie(armorZombieGO);
            }
        }
    }

    IEnumerator DeleteHelmetCoroutine(GameObject armorZombieObject)
    {
        yield return new WaitForSeconds(0.5f);
        Destroy(armorZombieObject);
    }

}
