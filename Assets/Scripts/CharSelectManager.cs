using UnityEngine;
using UnityEngine.UI;

public class CharSelectManager : MonoBehaviour
{
    public enum Characters
    {
        Fire,
        DoubleBarrel,
        PunchGirl,
        ThunderMage,
        FireFighter,
        Archer,
        Healer,
        Ice,
        Ninja,
        KnifeGirl,
        CatPunchGirl,
        SwordHero,
        PinkPunchGirl,
        AxeHeroGirl,
        Viking,
        Cowboy,
        LightningSwordGirl,
        Samurai,
        SpeedSwordMan,
        Lancer,
        Vampire
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

        int value;

        switch(character)
        {
            case Characters.Fire:
                SelectButtonEnableTrue();
                RequireText.text = "Default";
                RequireText.color = Color.green;
                RequireInfoText.text = "";
                break;
            case Characters.PunchGirl:
                value = 500;
                RequireText.text = GetTotalRequirementString(value);
                RequireText.color = Color.white;

                if (IsTotalScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.DoubleBarrel:
                value = 150;

                RequireText.text = GetTotalRequirementString(value);
                RequireText.color = Color.white;

                if (IsTotalScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.ThunderMage:
                value = 50;
                RequireText.text = GetBestRequirementString(value);
                RequireText.color = Color.white;

                if (IsBestScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value); ;
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetBestScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.FireFighter:
                value = 300;
                RequireText.text = GetGameOverRequirementString(value);
                RequireText.color = Color.white;

                if (IsGameOverSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalGameOverCount(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Archer:
                value = 1;
                RequireText.text = GetPlayTimeRequirementString(value);
                RequireText.color = Color.white;

                if (IsPlayTimeSatisfied(value))
                {
                    Debug.Log("Total Hour" + GameManager.instance.GetTotalHour());
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "1:00/1:00";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalPlayTimeText() + "/01:00";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Healer:
                value = 100;
                RequireText.text = GetBestRequirementString(value);
                RequireText.color = Color.white;

                if (IsBestScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetBestScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Ice:
                value = 5000;
                RequireText.text = GetTotalRequirementString(value);
                RequireText.color = Color.white;

                if (IsTotalScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Ninja:
                value = 10000;
                RequireText.text = GetTotalRequirementString(value);
                RequireText.color = Color.white;

                if (IsTotalScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.CatPunchGirl:
                value = 150;
                RequireText.text = GetBestRequirementString(value);
                RequireText.color = Color.white;

                if (IsBestScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetBestScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.SwordHero:
                value = 1000;
                RequireText.text = GetGameOverRequirementString(value);
                RequireText.color = Color.white;

                if (IsGameOverSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalGameOverCount(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.PinkPunchGirl:
                value = 300;
                RequireText.text = GetBestRequirementString(value);
                RequireText.color = Color.white;

                if (IsBestScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetBestScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.AxeHeroGirl:
                value = 2;
                RequireText.text = GetPlayTimeRequirementString(value);
                RequireText.color = Color.white;

                if (IsPlayTimeSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = "02:00/02:00";
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GameManager.instance.GetTotalPlayTimeText() + "/02:00";
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Viking:
                value = 200;
                RequireText.text = GetBestRequirementString(200);
                RequireText.color = Color.white;

                if (IsBestScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetBestScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Cowboy:
                value = 15000;
                RequireText.text = GetTotalRequirementString(value);
                RequireText.color = Color.white;

                if (IsTotalScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Lancer:
                value = 30000;
                RequireText.text = GetTotalRequirementString(value);
                RequireText.color = Color.white;

                if (IsTotalScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.LightningSwordGirl:
                value = 20000;
                RequireText.text = GetTotalRequirementString(value);
                RequireText.color = Color.white;

                if (IsTotalScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Samurai:
                value = 400;
                RequireText.text = GetBestRequirementString(value);
                RequireText.color = Color.white;

                if (IsBestScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetBestScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;

            case Characters.SpeedSwordMan:
                value = 25000;
                RequireText.text = GetTotalRequirementString(value);
                RequireText.color = Color.white;

                if (IsTotalScoreSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalScore(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.KnifeGirl:
                value = 500;
                RequireText.text = GetGameOverRequirementString(value);
                RequireText.color = Color.white;

                if (IsGameOverSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalGameOverCount(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
            case Characters.Vampire:
                value = 2000;
                RequireText.text = GetGameOverRequirementString(value);
                RequireText.color = Color.white;

                if (IsGameOverSatisfied(value))
                {
                    SelectButtonEnableTrue();

                    RequireInfoText.text = GetValueSlashValueString(value, value);
                    RequireInfoText.color = Color.green;
                }
                else
                {
                    SelectButtonEnableFalse();

                    RequireInfoText.text = GetValueSlashValueString(GameManager.instance.GetTotalGameOverCount(), value);
                    RequireInfoText.color = Color.red;
                }
                break;
        }
    }

    public string GetValueSlashValueString(int value1, int value2)
    {
        return value1 + "/" + value2;
    }

    public string GetTotalRequirementString(int value)
    {
        return "Total" + " " + value;
    }

    public string GetBestRequirementString(int value)
    {
        return "Best" + " " + value;
    }

    public string GetGameOverRequirementString(int value)
    {
        return "Game Over " + value;
    }

    public string GetPlayTimeRequirementString(int value)
    {
        return "Game Play " + value + " Hour";
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

    public bool IsTotalScoreSatisfied(int value)
    {
        if (GameManager.instance.GetTotalScore() >= value)
            return true;

        return false;
    }

    public bool IsBestScoreSatisfied(int value)
    {
        if (GameManager.instance.GetBestScore() >= value)
            return true;

        return false;
    }

    public bool IsGameOverSatisfied(int value)
    {
        if (GameManager.instance.GetTotalGameOverCount() >= value)
            return true;

        return false;
    }

    public bool IsPlayTimeSatisfied(int value)
    {
        if (GameManager.instance.GetTotalHour() >= value)
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
