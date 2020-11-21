using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private bool gameOverCheck = true;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Zombie(Clone)" && gameOverCheck)
        {
            GameManager.instance.GameOver();
            gameOverCheck = false;
        }
    }

    public void SetGameOverCheck(bool check)
    {
        gameOverCheck = check;
    }
}
