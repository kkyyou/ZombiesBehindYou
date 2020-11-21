using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Zombie(Clone)")
        {
            GameManager.instance.GameOver();
        }
    }
}
