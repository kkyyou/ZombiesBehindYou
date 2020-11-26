using UnityEngine;
using UnityEngine.UI;

public class CharSelectManager : MonoBehaviour
{
    enum Characters
    {
        Fire,
        PunchGirl,
        DoubleBarrel,
        ThunderMage,
        FireFighter,
        Archer,
        Healer,
        Ice,
        Ninja,
        CatPunchGirl
    }

    public static CharSelectManager instance;

    public GameObject[] charPrefabs;
    public GameObject player;

    public Text RequireText;
    public Text RequireInfoText;

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

    public void ChangeCharacter(int number)
    {
        Destroy(Player.instance.transform.Find("Character").gameObject);
        player = Instantiate(charPrefabs[(number)]);
        player.gameObject.name = "Character";
        player.transform.parent = FindObjectOfType<Player>().gameObject.transform;
        player.transform.position = Player.instance.transform.position;
    }

    public void CharacterInfo(int number)
    {
        ChangeCharacter(number);

        if (number == (int)Characters.Fire)
        {
            SelectButtonEnableTrue();

            RequireText.text = "Default";
            RequireText.color = Color.green;

            RequireInfoText.text = "";
        }
        else if (number == (int)Characters.PunchGirl)
        {
            RequireText.text = "Total 250";
            RequireText.color = Color.white;

            if (IsPunchGirlConditionFulfill())
            {
                SelectButtonEnableTrue();

                RequireInfoText.text = "250/250";
                RequireInfoText.color = Color.green;
            }
            else
            {
                SelectButtonEnableFalse();

                RequireInfoText.text = GameManager.instance.GetTotalScore() + "/250";
                RequireInfoText.color = Color.red;
            }
        }
        else if (number == (int)Characters.DoubleBarrel)
        {
            RequireText.text = "Best 100";
            RequireText.color = Color.white;

            if (IsDoubleBarrelConditonFulfill())
            {
                SelectButtonEnableTrue();

                RequireInfoText.text = "100/100";
                RequireInfoText.color = Color.green;
            }
            else
            {
                SelectButtonEnableFalse();

                RequireInfoText.text = GameManager.instance.GetBestScore() + "/100";
                RequireInfoText.color = Color.red;
            }
        }
        else if (number == (int)Characters.ThunderMage)
        {
            RequireText.text = "Best 100";
            RequireText.color = Color.white;

            if (IsThunderMageConditonFulfill())
            {
                SelectButtonEnableTrue();

                RequireInfoText.text = "100/100";
                RequireInfoText.color = Color.green;
            }
            else
            {
                SelectButtonEnableFalse();

                RequireInfoText.text = GameManager.instance.GetBestScore() + "/100";
                RequireInfoText.color = Color.red;
            }
        }
        else if (number == (int)Characters.FireFighter)
        {
            RequireText.text = "Total 100";
            RequireText.color = Color.white;

            if (IsFireFighterConditionFulfill())
            {
                SelectButtonEnableTrue();

                RequireInfoText.text = "100/100";
                RequireInfoText.color = Color.green;
            }
            else
            {
                SelectButtonEnableFalse();

                RequireInfoText.text = GameManager.instance.GetScore() + "/100";
                RequireInfoText.color = Color.red;
            }
        }
        else if (number == (int)Characters.Archer)
        {
            RequireText.text = "Total 100";
            RequireText.color = Color.white;

            if (IsArcherConditionFulfill())
            {
                SelectButtonEnableTrue();

                RequireInfoText.text = "100/100";
                RequireInfoText.color = Color.green;
            }
            else
            {
                SelectButtonEnableFalse();

                RequireInfoText.text = GameManager.instance.GetScore() + "/100";
                RequireInfoText.color = Color.red;
            }
        }
        else if (number == (int)Characters.Healer)
        {
            RequireText.text = "Total 100";
            RequireText.color = Color.white;

            if (IsHealerConditionFulfill())
            {
                SelectButtonEnableTrue();

                RequireInfoText.text = "100/100";
                RequireInfoText.color = Color.green;
            }
            else
            {
                SelectButtonEnableFalse();

                RequireInfoText.text = GameManager.instance.GetScore() + "/100";
                RequireInfoText.color = Color.red;
            }
        }
        else if (number == (int)Characters.Ice)
        {
            RequireText.text = "Total 100";
            RequireText.color = Color.white;

            if (IsIceHeroConditionFulfill())
            {
                SelectButtonEnableTrue();

                RequireInfoText.text = "100/100";
                RequireInfoText.color = Color.green;
            }
            else
            {
                SelectButtonEnableFalse();

                RequireInfoText.text = GameManager.instance.GetScore() + "/100";
                RequireInfoText.color = Color.red;
            }
        }
        else if (number == (int)Characters.Ninja)
        {
            RequireText.text = "Total 100";
            RequireText.color = Color.white;

            if (IsNinjaConditionFulfill())
            {
                SelectButtonEnableTrue();

                RequireInfoText.text = "100/100";
                RequireInfoText.color = Color.green;
            }
            else
            {
                SelectButtonEnableFalse();

                RequireInfoText.text = GameManager.instance.GetScore() + "/100";
                RequireInfoText.color = Color.red;
            }
        }
        else if (number == (int)Characters.CatPunchGirl)
        {
            RequireText.text = "Total 100";
            RequireText.color = Color.white;

            if (IsCatPunchGirlConditionFulfill())
            {
                SelectButtonEnableTrue();

                RequireInfoText.text = "100/100";
                RequireInfoText.color = Color.green;
            }
            else
            {
                SelectButtonEnableFalse();

                RequireInfoText.text = GameManager.instance.GetScore() + "/100";
                RequireInfoText.color = Color.red;
            }
        }
    }

    public void SelectCharacter(int number)
    {
        // 선택되었던 캐릭터 넘버 세이브.
        DBManager.instance.data.selectedCharacterNumber = number;
        Debug.Log("Save Selected Character Number : " + number);

        DBManager.instance.SaveCurrentData();
    }

    public int CharPrefabCount()
    {
        return charPrefabs.Length;
    }

    public bool IsPunchGirlConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 250)
            return true;

        return false;
    }

    public bool IsDoubleBarrelConditonFulfill()
    {
        if (GameManager.instance.GetBestScore() >= 100)
            return true;

        return false;
    }

    public bool IsThunderMageConditonFulfill()
    {
        if (GameManager.instance.GetBestScore() >= 100)
            return true;

        return false;
    }

    public bool IsFireFighterConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 100)
            return true;

        return false;
    }

    public bool IsArcherConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 100)
            return true;

        return false;
    }

    public bool IsHealerConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 100)
            return true;

        return false;
    }

    public bool IsIceHeroConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 100)
            return true;

        return false;
    }

    public bool IsNinjaConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 100)
            return true;

        return false;
    }

    public bool IsCatPunchGirlConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 100)
            return true;

        return false;
    }

    public void SelectButtonEnableTrue()
    {
        GameManager.instance.shopSelectButton.enabled = true;
        GameManager.instance.shopSelectButton.gameObject.transform.Find("SelectNo").gameObject.SetActive(false);
    }

    public void SelectButtonEnableFalse()
    {
        GameManager.instance.shopSelectButton.enabled = false;
        GameManager.instance.shopSelectButton.gameObject.transform.Find("SelectNo").gameObject.SetActive(true);
    }
}
