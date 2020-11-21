using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectManager : MonoBehaviour
{
    enum Characters
    {
        Fire,
        PunchGirl
    }

    public static CharSelectManager instance;

    public GameObject[] charPrefabs;
    public GameObject player;

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
        //player = Instantiate(charPrefabs[(int)Characters.Fire]);
        //player.transform.position = transform.position;
    }

    public void SelectCharacter(int number)
    {
        Destroy(Player.instance.transform.Find("Character").gameObject);
        player = Instantiate(charPrefabs[(number)]);
        player.gameObject.name = "Character";
        player.transform.parent = FindObjectOfType<Player>().gameObject.transform;
        player.transform.position = Player.instance.transform.position;
    }
    
    public int CharPrefabCount()
    {
        return charPrefabs.Length;
    }
}
