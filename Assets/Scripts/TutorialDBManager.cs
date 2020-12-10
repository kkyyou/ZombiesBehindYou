using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public class TutorialDBManager : MonoBehaviour
{
    [System.Serializable]
    public class Data
    {
        public bool viewTutorial;
    }

    public TutorialManager tutoManager;

    public Data data;

    // 게임 오버시 Call Save 호출 됨.
    public void CallSave()
    {
        // 토탈 점수 세이브.
        data.viewTutorial = tutoManager.GetViewTutorialFlag();

        BinaryFormatter bf = new BinaryFormatter(); // 2진 파일로 변환.
        FileStream file = File.Create(Application.persistentDataPath + "/SaveFileTuto.dat");

        bf.Serialize(file, data);
        file.Close();
    }

    public void CallLoad()
    {
        if (File.Exists(Application.persistentDataPath + "/SaveFileTuto.dat"))
        {
            BinaryFormatter bf = new BinaryFormatter(); // 2진 파일로 변환.
            FileStream file = File.Open(Application.persistentDataPath + "/SaveFileTuto.dat", FileMode.Open);

            if (file != null && file.Length > 0)
            {
                data = (Data)bf.Deserialize(file);
                tutoManager.SetViewTutorialFlag(data.viewTutorial);

                file.Close();
            }
        }
        else
        {
            FileStream file = File.Create(Application.persistentDataPath + "/SaveFileTuto.dat");
            file.Close();
        }
    }
}
