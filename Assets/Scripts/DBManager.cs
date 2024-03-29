﻿using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class DBManager : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public int bestScore;
        public int totalScore;
        public int selectedCharacterNumber;
        public bool listenSfx;
        public bool listenBgm;
        public int totalPlayHour;
        public int totalPlayMin;
        public int gameOverCount;
        public bool reviewChecked = false;
    }

    public Data data;

    public static DBManager instance;

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

    // 게임 오버시 Call Save 호출 됨.
    public void CallSave()
    {
        // 최고 점수면 세이브.
        if (data.bestScore < GameManager.instance.GetBestScore())
        {
            data.bestScore = GameManager.instance.GetBestScore();
            Debug.Log("Save BestScore : " + data.bestScore);
        }

        // 토탈 점수 세이브.
        data.totalScore += GameManager.instance.GetScore();
        Debug.Log("Save TotalScore : " + data.totalScore);

        // 게임 오버 카운트 세이브.
        data.gameOverCount = GameManager.instance.GetTotalGameOverCount();

        BinaryFormatter bf = new BinaryFormatter(); // 2진 파일로 변환.
        FileStream file = File.Create(Application.persistentDataPath + "/SaveFile.dat");

        bf.Serialize(file, data);
        file.Close();
    }

    public void SaveCurrentData()
    {
        BinaryFormatter bf = new BinaryFormatter(); // 2진 파일로 변환.
        FileStream file = File.Create(Application.persistentDataPath + "/SaveFile.dat");

        bf.Serialize(file, data);
        file.Close();
    }

    // 게임 시작 시 정보 로드.
    public void CallLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFile.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter(); // 2진 파일로 변환.
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFile.dat", FileMode.Open);

            if (file != null && file.Length > 0)
            {
                data = (Data)bf.Deserialize(file);

                // 최고 점수 로드.
                GameManager.instance.SetBestScore(data.bestScore);
                Debug.Log("Load BestScore : " + GameManager.instance.GetBestScore());

                // 총합 점수 로드.
                GameManager.instance.SetTotalScore(data.totalScore);
                Debug.Log("Load TotalScore : " + GameManager.instance.GetTotalScore());

                // 선택되었던 캐릭터 넘버 로드.
                Player.instance.SetSelectedCharacterNumber(data.selectedCharacterNumber);
                Debug.Log("Load Selected Character Number : " + Player.instance.GetSelectedCharacterNumber());

                // 효과음, 배경음 듣기 로드.
                GameManager.instance.SetListenSfx(data.listenSfx);
                GameManager.instance.SetListenBgm(data.listenBgm);

                // 플레이 타임 로드.
                GameManager.instance.SetTotalHour(data.totalPlayHour);
                Debug.Log("Load Total Hour : " + data.totalPlayHour);

                GameManager.instance.SetTotalMin(data.totalPlayMin);
                Debug.Log("Load Total Min : " + data.totalPlayMin);

                // 게임 오버 카운트 로드.
                GameManager.instance.SetTotalGameOverCount(data.gameOverCount);
                Debug.Log("Load Total GameOver Count : " + data.gameOverCount);

                // 리뷰 체크 로드.
                GameManager.instance.SetReviewChecked(data.reviewChecked);

                file.Close();
            }
        }
        else
        {
            Debug.Log("Create");
            FileStream file = File.Create(Application.persistentDataPath + "/SaveFile.dat");
            file.Close();
        }
    }
}
