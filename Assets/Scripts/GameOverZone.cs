using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private bool gameOverCheck = true;

    private void Update()
    {
        if (GameManager.instance.hpSlider.value <= 0 && gameOverCheck)
        {
            GameManager.instance.GameOver();
            gameOverCheck = false;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        Zombie zombie = collision.gameObject.GetComponent<Zombie>();
        if (zombie && (zombie.name == "Zombie1" || zombie.name == "Zombie2") && gameOverCheck)
        {
            GameManager.instance.GameOver();
            gameOverCheck = false;
        }
    }

    public void SetGameOverCheck(bool check)
    {
        gameOverCheck = check;
    }

    public bool GetGameOverCheck()
    {
        return gameOverCheck;
    }
}
