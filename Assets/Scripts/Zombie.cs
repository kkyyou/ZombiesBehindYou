using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    private float speed = 0.05f;     // 0.05
    private int walkCount = 20;   // 20

    private int currentWalkCount = 0; // 0.05 * 20

    private int moveDir;
    
    public Animator anim;

    private void Start()
    {
        float playerLocX = Player.instance.transform.position.x;
        float zombieLocX = transform.position.x;

        if (zombieLocX - playerLocX < 0)
            moveDir = 1;
        else
            moveDir = -1;
    }

    public void Move()
    {
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        anim.SetBool("Walk", true);

        while (currentWalkCount < walkCount)
        {
            transform.Translate(moveDir * speed, 0, 0);

            currentWalkCount++;

            // 각 반복당 0.01초 대기함으로써 부드럽게 캐릭터 이동.
            yield return new WaitForSeconds(0.001f);
        }

        currentWalkCount = 0;
        anim.SetBool("Walk", false);
    }
    
    public void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale = new Vector3(theScale.x * -1, theScale.y, theScale.z);
        transform.localScale = theScale;
    }

    public int GetMoveDir()
    {
        return moveDir;
    }
}
