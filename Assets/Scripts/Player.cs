using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Animator animator;
    private bool isRight = true; // 히어로 보는 방향 오른쪽 체크.

    private bool nextTurn = false; // 히어로 공격시 좀비들 한 스텝씩 이동.

    public RightTargetZone rightTargetZone; // 히어로 오른쪽 공격 범위.
    public LeftTargetZone leftTargetZone;   // 히어로 왼쪽 공격 범위.


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
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

    }

    public void TurnButton()
    {
        isRight = !isRight;
        Flip();

        animator.SetBool("Attack", true);

        // Attack Sound 랜덤 재생.
        AudioManager.instance.PlayRandomAttackSound();

        // 플래그 변경하여 Zone에서 Zombie가 날아가게 한다.
        if (isRight)
            rightTargetZone.setBeAttackedRight(true);
        else
            leftTargetZone.setBeAttackedLeft(true);

        StartCoroutine(AttackCoroutine());

        nextTurn = true;
    }

    public void AttackButton()
    {
        animator.SetBool("Attack", true);

        // Attack Sound 랜덤 재생.
        AudioManager.instance.PlayRandomAttackSound();

        // 플래그 변경하여 Zone에서 Zombie가 날아가게 한다.
        if (isRight)
            rightTargetZone.setBeAttackedRight(true);
        else
            leftTargetZone.setBeAttackedLeft(true);

        StartCoroutine(AttackCoroutine());

        nextTurn = true;
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
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
