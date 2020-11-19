using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private float speed = 0.05f;     // 0.05
    private int walkCount = 20;   // 20

    private int currentWalkCount = 0; // 1.6 * 20

    private int moveDir;

    private Animator anim;

    private Player thePlayer;

    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        thePlayer = Player.instance;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void Move()
    {
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        float playerLocX = thePlayer.transform.position.x;
        float zombieLocX = transform.position.x;

        if (zombieLocX - playerLocX < 0)
            moveDir = 1;
        else
            moveDir = -1;

        anim.SetBool("Walk", true);

        while (currentWalkCount < walkCount)
        {
            transform.Translate(moveDir * speed, 0, 0);

            currentWalkCount++;

            // 각 반복당 0.01초 대기함으로써 부드럽게 캐릭터 이동.
            yield return new WaitForSeconds(0.01f);
        }

        currentWalkCount = 0;
        anim.SetBool("Walk", false);
    }
}
