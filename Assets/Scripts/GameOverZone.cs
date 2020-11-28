using UnityEngine;

public class GameOverZone : MonoBehaviour
{
    private void OnTriggerStay2D(Collider2D collision)
    {
        Zombie zombie = collision.gameObject.GetComponent<Zombie>();
        if (zombie && (zombie.name == "Zombie1" || zombie.name == "Zombie2") && GameManager.instance.GetGameOverCheck())
        {
            GameManager.instance.GameOver();
            GameManager.instance.SetGameOverCheck(false);
        }
    }
}
