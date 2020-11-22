using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class DBManager : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public int bestScore;
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


    public void CallSave()
    {
        if (data.bestScore < GameManager.instance.GetBestScore())
        {
            data.bestScore = GameManager.instance.GetBestScore();
            Debug.Log("세이브 BestScore : " + data.bestScore);
        }

        BinaryFormatter bf = new BinaryFormatter(); // 2진 파일로 변환.
        FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");


        bf.Serialize(file, data);
        file.Close();
    }

    // 게임 시작 시 정보 로드.
    public void CallLoad()
    {
        if (File.Exists(Application.dataPath + "/SaveFile.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter(); // 2진 파일로 변환.
            FileStream file = File.Open(Application.dataPath + "/SaveFile.dat", FileMode.Open);

            if (file != null && file.Length > 0)
            {
                data = (Data)bf.Deserialize(file);

                GameManager.instance.SetBestScore(data.bestScore);
                Debug.Log("로드 BestScore : " + GameManager.instance.GetBestScore());
                file.Close();
            }
        }
        else
        {
            Debug.Log("Create");
            FileStream file = File.Create(Application.dataPath + "/SaveFile.dat");
            file.Close();
        }
    }
}
