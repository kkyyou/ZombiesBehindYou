using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    public static Player instance;

    private Animator animator;
    private bool isRight = true; // 히어로 보는 방향 오른쪽 체크.

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
        {
            GameManager.instance.SetPlaying(true);
            GameManager.instance.controlCanvas.transform.Find("Info").gameObject.SetActive(false);
        }

        isRight = !isRight;
        Flip();

        animator.SetBool("Attack", true);

        // Attack Sound 랜덤 재생.
        if (GameManager.instance.GetListenSfx())
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
                GameManager.instance.attackZombieSuccess();
                EnemyManager.instance.GoNextTurn(collision.gameObject);
            }
            else
            {
                EnemyManager.instance.GoNextTurn();
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
                GameManager.instance.attackZombieSuccess();
                EnemyManager.instance.GoNextTurn(collision.gameObject);
            }
            else
            {
                EnemyManager.instance.GoNextTurn();
            }
        }

        StartCoroutine(AttackCoroutine());
    }

    public void AttackButton()
    {
        if (!GameManager.instance.GetPlaying())
        {
            GameManager.instance.SetPlaying(true);
            GameManager.instance.controlCanvas.transform.Find("Info").gameObject.SetActive(false);
        }

        animator.SetBool("Attack", true);

        // Attack Sound 랜덤 재생.
        if (GameManager.instance.GetListenSfx())
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
            // Hit인데 강철 좀비인 경우는 해당 방향 좀비들은 움직임을 멈춰야 함.
            // 강철좀비 반대편 좀비들은 움직여야 함.

            if (isRight)
                rightTargetZone.ThrowZombie(collision);
            else
                leftTargetZone.ThrowZombie(collision);

            GameManager.instance.attackZombieSuccess();

            EnemyManager.instance.GoNextTurn(collision.gameObject);
        }
        else
        {
            // No Hit인 경우는 좀비들 움직이는게 맞음.
            Debug.Log("No Hit");
            EnemyManager.instance.GoNextTurn();
        }
        
        StartCoroutine(AttackCoroutine());
    }

    public void LeftRightAttackButton()
    {
        if (!GameManager.instance.GetPlaying())
        {
            GameManager.instance.SetPlaying(true);
            GameManager.instance.controlCanvas.transform.Find("Info").gameObject.SetActive(false);
        }

        animator.SetBool("LeftRightAttack", true);

        // Attack Sound 랜덤 재생.
        if (GameManager.instance.GetListenSfx())
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
            GameManager.instance.attackZombieSuccess();
            EnemyManager.instance.GoNextTurn(collision1.gameObject, collision2.gameObject);
        }
        else if (collision1)
        {
            // 한쪽만 존재하는데 양쪽 공격사용 시 HP 감소.
            EnemyManager.instance.GoNextTurn(collision1.gameObject);

            GameManager.instance.RecoveryHP(-15);
        }
        else if (collision2)
        {
            // 한쪽만 존재하는데 양쪽 공격사용 시 HP 감소.
            EnemyManager.instance.GoNextTurn(collision2.gameObject);

            GameManager.instance.RecoveryHP(-15);
        }

        StartCoroutine(LeftRightAttackCoroutine());
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
        if (GameManager.instance.GetListenBgm())
            AudioManager.instance.Play("background");

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

    public void ClickSettingBtn()
    {
        GameManager.instance.ShowSettingView();
    }

    public int GetCharacterNumber()
    {
        return characterNo;
    }
}
