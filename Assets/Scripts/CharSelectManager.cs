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
        CatPunchGirl,
        SwordHero,
        PinkPunchGirl,
        AxeHeroGirl,
        Viking,
        Cowboy,
        Lancer
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

        Characters character = (Characters)number;

        switch(character)
        {
            case Characters.Fire:
                SelectButtonEnableTrue();
                RequireText.text = "Default";
                RequireText.color = Color.green;
                RequireInfoText.text = "";
                break;
            case Characters.PunchGirl:
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
                break;
            case Characters.DoubleBarrel:
                RequireText.text = "Best 50";
                RequireText.color = Color.white;

                if (IsDoubleBarrelConditonFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "50/50";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetBestScore() + "/50";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.ThunderMage:
                RequireText.text = "Best 150";
                RequireText.color = Color.white;

                if (IsThunderMageConditonFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "150/150";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetBestScore() + "/150";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.FireFighter:
                RequireText.text = "Total 500";
                RequireText.color = Color.white;

                if (IsFireFighterConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "500/500";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalScore() + "/500";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Archer:
                RequireText.text = "Total 3000";
                RequireText.color = Color.white;

                if (IsArcherConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "3000/10300000";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalScore() + "/3000";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Healer:
                RequireText.text = "Best 170";
                RequireText.color = Color.white;

                if (IsHealerConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "170/170";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetBestScore() + "/170";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Ice:
                RequireText.text = "Total 555";
                RequireText.color = Color.white;

                if (IsIceHeroConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "555/555";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalScore() + "/555";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Ninja:
                RequireText.text = "Total 10000";
                RequireText.color = Color.white;

                if (IsNinjaConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "10000/10000";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalScore() + "/10000";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.CatPunchGirl:
                RequireText.text = "Total 5000";
                RequireText.color = Color.white;

                if (IsCatPunchGirlConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "5000/5000";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalScore() + "/5000";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.SwordHero:
                RequireText.text = "Best 300";
                RequireText.color = Color.white;

                if (IsSwordHeroConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "300/300";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetBestScore() + "/300";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.PinkPunchGirl:
                RequireText.text = "Total 1500";
                RequireText.color = Color.white;

                if (IsPinkPunchGirlConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "1500/1500";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalScore() + "/1500";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.AxeHeroGirl:
                RequireText.text = "Best 500";
                RequireText.color = Color.white;

                if (IsAxeHeroGirlConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "500/500";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetBestScore() + "/500";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Viking:
                RequireText.text = "Best 500";
                RequireText.color = Color.white;

                if (IsAxeHeroGirlConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "500/500";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetBestScore() + "/500";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Cowboy:
                RequireText.text = "Total 2000";
                RequireText.color = Color.white;

                if (IsCowboyConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "2000/2000";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalScore() + "/2000";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Lancer:
                RequireText.text = "Total 2500";
                RequireText.color = Color.white;

                if (IsLancerConditionFulfill())
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "2500/2500";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalScore() + "/2500";
                    RequireInfoText.color = Color.red;
                }
                break;

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
        if (GameManager.instance.GetBestScore() >= 50)
            return true;

        return false;
    }

    public bool IsThunderMageConditonFulfill()
    {
        if (GameManager.instance.GetBestScore() >= 150)
            return true;

        return false;
    }

    public bool IsFireFighterConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 500)
            return true;

        return false;
    }

    public bool IsArcherConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 3000)
            return true;

        return false;
    }

    public bool IsHealerConditionFulfill()
    {
        if (GameManager.instance.GetBestScore() >= 170)
            return true;

        return false;
    }

    public bool IsIceHeroConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 555)
            return true;

        return false;
    }

    public bool IsNinjaConditionFulfill()
    {
        if (GameManager.instance.GetBestScore() >= 10000)
            return true;

        return false;
    }

    public bool IsCatPunchGirlConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 5000)
            return true;

        return false;
    }

    public bool IsSwordHeroConditionFulfill()
    {
        if (GameManager.instance.GetBestScore() >= 300)
            return true;

        return false;
    }

    public bool IsPinkPunchGirlConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 1500)
            return true;

        return false;
    }

    public bool IsAxeHeroGirlConditionFulfill()
    {
        if (GameManager.instance.GetBestScore() >= 500)
            return true;

        return false;
    }

    public bool IsVikingConditionFulfill()
    {
        if (GameManager.instance.GetBestScore() >= 500)
            return true;

        return false;
    }

    public bool IsCowboyConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 2000)
            return true;

        return false;
    }

    public bool IsLancerConditionFulfill()
    {
        if (GameManager.instance.GetTotalScore() >= 2500)
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
