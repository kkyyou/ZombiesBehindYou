﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Zombie : MonoBehaviour
{
    public string name;
    private float speed = 8f;    
    private int moveDir;

    public Animator anim;

    private void Start()
    {
        CalcMoveDir();
    }

    public void Move()
    {
        StartCoroutine(MoveCoroutine());
    }

    IEnumerator MoveCoroutine()
    {
        //anim.SetBool("Walk", true);

        Vector3 zombieVector = this.transform.position;
        Vector3 target = new Vector3(zombieVector.x + (moveDir * 1), zombieVector.y, zombieVector.z);
        
        // 현재 좀비 위치를 Target 위치로 이동 시킴. 
        while (true)
        {
            zombieVector = Vector3.MoveTowards(zombieVector, target, speed * Time.deltaTime);
            this.transform.position = zombieVector;

            if (this.transform.position == target)
                break;

            yield return null;
        }

        //anim.SetBool("Walk", false);
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

    public void SetMoveDir(int _moveDir)
    {
        moveDir = _moveDir;
    }

    public void CalcMoveDir()
    {
        Vector3 playerVector = Player.instance.transform.position;
        Vector3 zombieVector = this.transform.position;

        if (zombieVector.x - playerVector.x < 0)
        {
            moveDir = 1;
        }
        else if (zombieVector.x - playerVector.x > 0)
        {
            moveDir = -1;
        }
        else
        {
            moveDir = 0;
        }
    }
}
