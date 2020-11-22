using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Animator animator;
    private bool isRight = true; // 히어로 보는 방향 오른쪽 체크.

    private bool nextTurn = false; // 히어로 공격시 좀비들 한 스텝씩 이동.

    private RightTargetZone rightTargetZone; // 히어로 오른쪽 공격 범위.
    private LeftTargetZone leftTargetZone;   // 히어로 왼쪽 공격 범위.

    public LayerMask zombieLayerMask;

    private int characterNo = 0;

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
        rightTargetZone = FindObjectOfType<RightTargetZone>();
        leftTargetZone = FindObjectOfType<LeftTargetZone>();

        animator = transform.Find("Character").GetComponent<Animator>();
    }

    // 추후에 캐릭터 방향 랜덤이 필요할 까?
    public void RandomCharacterDir()
    {
        int dirRan = Random.Range(0, 2);

        // 0 : Right , 1 : Left.
        if (dirRan == 0)
        {
            isRight = true;
        }
        else
        {
            Flip();
            isRight = false;
        }
    }

    public void TurnButton()
    {
        if (!GameManager.instance.GetPlaying())
            GameManager.instance.SetPlaying(true);

        isRight = !isRight;
        Flip();

        animator.SetBool("Attack", true);

        // Attack Sound 랜덤 재생.
        AudioManager.instance.PlayRandomAttackSound();

        // 플래그 변경하여 Zone에서 Zombie가 날아가게 한다.
        if (isRight)
        {
            Vector2 playerVector = transform.position;
            playerVector = new Vector2(playerVector.x + 1, playerVector.y);
            Collider2D collision = CheckCollision(playerVector);

            if (collision)
            {
                rightTargetZone.ThrowZombie(collision);
                GameManager.instance.SetAttackZombie(true);
            }

        }
        else
        {
            Vector2 playerVector = transform.position;
            playerVector = new Vector2(playerVector.x - 1, playerVector.y);
            Collider2D collision = CheckCollision(playerVector);

            if (collision)
            {
                leftTargetZone.ThrowZombie(collision);
                GameManager.instance.SetAttackZombie(true);
            }
        }

        StartCoroutine(AttackCoroutine());

        nextTurn = true;
    }

    public void AttackButton()
    {
        if (!GameManager.instance.GetPlaying())
            GameManager.instance.SetPlaying(true);

        animator.SetBool("Attack", true);

        // Attack Sound 랜덤 재생.
        AudioManager.instance.PlayRandomAttackSound();

        Collider2D collision;
        Vector2 playerVector = transform.position;

        if (isRight)
        {
            playerVector = new Vector2(playerVector.x + 1, playerVector.y);
        }
        else
        {
            playerVector = new Vector2(playerVector.x - 1, playerVector.y);
        }

        collision = CheckCollision(playerVector);

        if (collision)
        {
            if (isRight)
                rightTargetZone.ThrowZombie(collision);
            else
                leftTargetZone.ThrowZombie(collision);

            GameManager.instance.SetAttackZombie(true);
        }

        StartCoroutine(AttackCoroutine());

        nextTurn = true;
    }

    public void LeftRightAttackButton()
    {
        if (!GameManager.instance.GetPlaying())
            GameManager.instance.SetPlaying(true);

        animator.SetBool("LeftRightAttack", true);

        // Attack Sound 랜덤 재생.
        AudioManager.instance.PlayRandomAttackSound();

        // Right
        Vector2 vector1 = transform.position;
        vector1 = new Vector2(vector1.x + 1, vector1.y);
        Collider2D collision1 = CheckCollision(vector1);

        if (collision1)
        {
            rightTargetZone.ThrowZombie(collision1);
        }

        // Left
        Vector2 vector2 = transform.position;
        vector2 = new Vector2(vector2.x - 1, vector2.y);
        Collider2D collision2 = CheckCollision(vector2);

        if (collision2)
        {
            leftTargetZone.ThrowZombie(collision2);
        }

        // 점수 및 HP 업데이트.
        if (collision1 && collision2)
        {
            GameManager.instance.SetAttackZombie(true);
        }
        else
        {
            // 한쪽만 존재하는데 양쪽 공격사용 시 HP 감소.
            GameManager.instance.RecoveryHP(-15);
        }

        StartCoroutine(LeftRightAttackCoroutine());

        nextTurn = true;
    }

    IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        animator.SetBool("Attack", false);
    }

    IEnumerator LeftRightAttackCoroutine()
    {
        yield return new WaitForSeconds(0.05f);
        animator.SetBool("LeftRightAttack", false);
    }

    public void Flip()
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

    public Collider2D CheckCollision(Vector2 end)
    {
        RaycastHit2D hit;

        hit = Physics2D.Linecast(transform.position, end, zombieLayerMask);

        if (hit.transform != null)
            return hit.collider;

        return null;
    }

    public void RightCharacter()
    {
        characterNo++;

        if (characterNo > CharSelectManager.instance.CharPrefabCount() - 1)
            characterNo = 0;

        CharSelectManager.instance.CharacterInfo(characterNo);
    }

    public void LeftCharacter()
    {
        characterNo--;

        if (characterNo < 0)
            characterNo = CharSelectManager.instance.CharPrefabCount() - 1;

        CharSelectManager.instance.CharacterInfo(characterNo);
    }

    public void ShowSelectCharacterView()
    {
        GameManager.instance.ShowSelectCharcter();
    }

    public void SelectCharacter()
    {
        CharSelectManager.instance.SelectCharacter(characterNo);

        GameManager.instance.ShowTitleView();
    }

    public Animator GetAnimator()
    {
        animator = transform.Find("Character").GetComponent<Animator>();
        return animator;
    }

    public bool GetIsRight()
    {
        return isRight;
    }
    
    public void SetIsRight(bool _isRight)
    {
        isRight = _isRight;
    }

    public int GetSelectedCharacterNumber()
    {
        return characterNo;
    }

    public void SetSelectedCharacterNumber(int charNo)
    {
        characterNo = charNo;
    }

    public void ClickPauseBtn()
    {
        if (!GameManager.instance.GetPlaying())
            return;

        GameManager.instance.SetPlaying(false);
        GameManager.instance.SetControlButtonEnabled(false);

        GameManager.instance.pauseCanvas.SetActive(true);
    }

    public void ClickReplay()
    {
        if (GameManager.instance.GetPlaying())
            return;

        GameManager.instance.SetPlaying(true);
        GameManager.instance.SetControlButtonEnabled(true);

        GameManager.instance.pauseCanvas.SetActive(false);
    }
}
