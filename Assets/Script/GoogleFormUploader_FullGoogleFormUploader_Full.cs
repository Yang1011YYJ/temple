using System.Collections;
using UnityEngine;
using UnityEngine.Networking;
using System;

public class GoogleFormUploader_Full : MonoBehaviour
{
    // Google Form 的 formResponse URL
    private string formURL = "https://docs.google.com/forms/d/e/1FAIpQLSf4gv7TXPhZImuoyFF15RVNARhHMkegJPWL2HAW9TYk-nbVeA/formResponse";


    // 呼叫這個函式送資料
    public void UploadPlayData(
        string menuTime,
        string firstLevelID,
        string firstStartTime,
        string firstEndTime,
        string firstDuration,
        string secondLevelID,
        string secondStartTime,
        string secondEndTime,
        string secondDuration)
    {
        StartCoroutine(SendData(menuTime, firstLevelID, firstStartTime, firstEndTime, firstDuration,
                               secondLevelID, secondStartTime, secondEndTime, secondDuration));
    }

    IEnumerator SendData(
        string menuTime,
        string firstLevelID,
        string firstStartTime,
        string firstEndTime,
        string firstDuration,
        string secondLevelID,
        string secondStartTime,
        string secondEndTime,
        string secondDuration)
    {
        WWWForm form = new WWWForm();

        // 對應你的欄位 entry ID
        form.AddField("entry.1273981424", menuTime ?? "");
        form.AddField("entry.2127973828", firstLevelID ?? "");
        form.AddField("entry.96938003", firstStartTime ?? "");
        form.AddField("entry.420580623", firstEndTime ?? "");
        form.AddField("entry.1135213531", firstDuration ?? "");
        form.AddField("entry.287541732", secondLevelID ?? "");
        form.AddField("entry.51035449", secondStartTime ?? "");
        form.AddField("entry.45952439", secondEndTime ?? "");
        form.AddField("entry.1056361058", secondDuration ?? "");


        UnityWebRequest www = UnityWebRequest.Post(formURL, form);
        www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded"); // ✅ 修正
        www.method = UnityWebRequest.kHttpVerbPOST; // 強制使用 POST

        yield return www.SendWebRequest();

        if (www.result == UnityWebRequest.Result.Success)
            Debug.Log("資料已成功送出到 Google Sheet");
        else
            Debug.LogError("送出失敗：" + www.error);
    }
}
