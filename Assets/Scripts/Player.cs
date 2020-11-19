using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Animator animator;
    private bool isRight = true; // 히어로 보는 방향 오른쪽 체크.

    private bool leftAttack = false; // 히어로 왼쪽 공격.
    private bool rightAttack = false; // 히어로 오른쪽 공격.

    private bool nextTurn = false; // 히어로 공격시 좀비들 한 스텝씩 이동.

    public RightTargetZone rightTargetZone; // 히어로 오른쪽 공격 범위.
    public LeftTargetZone leftTargetZone;   // 히어로 왼쪽 공격 범위.


    private void Awake()
    {
        if (instance == null)
        {
            DontDestroyOnLoad(this.gameObject);
            instance = this;
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (leftAttack && rightAttack)
        {

        }

    }

    public void LeftButton()
    {
        leftAttack = true;

        if (isRight)
        {
            Flip();
            isRight = false;
        }

        animator.SetBool("Attack", true);

        leftTargetZone.setBeAttackedLeft(true); // 플래그 변경하여 Zone에서 Zombie가 날아가게 한다.

        StartCoroutine(AttackCoroutine());

        leftAttack = false;

        nextTurn = true;
    }

    public void RightButton()
    {
        rightAttack = true;

        if (!isRight)
        {
            Flip();
            isRight = true;
        }

        animator.SetBool("Attack", true);

        rightTargetZone.setBeAttackedRight(true);

        StartCoroutine(AttackCoroutine());

        rightAttack = false;

        nextTurn = true;
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.1f);
        animator.SetBool("Attack", false);
    }

    void Flip()
    {
        Vector3 theScale = transform.localScale;
        theScale = new Vector3(theScale.x * -1, theScale.y, theScale.z);
        transform.localScale = theScale;
    }

    public bool GetNextTurn()
    {
        return nextTurn; 
    }

    public void SetNextTurn(bool nextTurn)
    {
        this.nextTurn = nextTurn;
    }
}
