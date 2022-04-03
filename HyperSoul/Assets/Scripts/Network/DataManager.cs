using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class DataManager : MonoBehaviour
{
    const string URL = "https://docs.google.com/spreadsheets/d/1smTaItZFLP5k4agzZ8nvxX_KSp0n_y9UjP701PXgMWs/export?format=tsv&range=A1:C";

    private IEnumerator Start()
    {
        UnityWebRequest www = UnityWebRequest.Get(URL);
        yield return www.SendWebRequest();

        if(www.isDone)
        {
            print(www.downloadHandler.text);
        }
        else
        {
            print("웹 응답 없음");
        }
    }
}

