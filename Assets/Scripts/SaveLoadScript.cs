using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityGoogleDrive;

public class SaveLoadScript : MonoBehaviour
{
    [SerializeField] private InputField _textField;
    [SerializeField] private InputField _numField;

    [SerializeField] private Text _enteredText;
    [SerializeField] private Text _enteredNums;
    [SerializeField] private Text _loadedText;
    [SerializeField] private Text _loadedNums;   

    [SerializeField] private string _fileName;
    private string _filePath;

    public void OnEnterClick()
    {
        if (_textField.text != "" && _numField.text != "")
        {
            _enteredText.text = _textField.text;
            _enteredNums.text = _numField.text;
        }
        else
            Debug.Log("Заполните все поля!");
        
    }

    public void OnSaveClick()
    {
        SaveData();
    }

    public void OnLoadClick()
    {
        LoadData();
    }    

    private void SaveData()
    {
        if (_enteredText.text != "" && _enteredNums.text != "")
        {
            GameData gameData = new GameData();

            gameData.SavedText = _enteredText.text;
            gameData.SavedNums = _enteredNums.text;

            string json = JsonUtility.ToJson(gameData, true);

            var file = new UnityGoogleDrive.Data.File()
            {
                Name = _fileName,
                Content = System.Text.Encoding.UTF8.GetBytes(json)
            };

            GoogleDriveFiles.Create(file).Send();

            Debug.Log("Saved");
        }
        else
            Debug.Log("Не все поля заполнены");
    }

    private void LoadData()
    {
        _filePath = Path.Combine(Application.persistentDataPath, _fileName);

        StartCoroutine(DownloadFromGoogleDrive());
    }
    
    private void SetLoadData()
    {
        string jsonData = File.ReadAllText(_filePath);
        
        GameData gameData = new GameData();

        gameData = JsonUtility.FromJson<GameData>(jsonData);

        _loadedText.text = gameData.SavedText;
        _loadedNums.text = gameData.SavedNums;
    }

    private IEnumerator DownloadFromGoogleDrive()
    {
        var listRequest = GoogleDriveFiles.List();
        listRequest.Fields = new List<string> { "files(id, name)" };
        listRequest.Q = $"name = '{_fileName}'";

        yield return listRequest.Send();

        if (listRequest.ResponseData?.Files == null || listRequest.Fields.Count == 0)
        {
            _loadedText.text = "Данные отсутствуют";
            _loadedNums.text = "Данные отсутствуют";

            yield break;
        }       

        var fileId = listRequest.ResponseData.Files[0].Id;

        var downloadRequest = GoogleDriveFiles.Download(fileId);
        yield return downloadRequest.Send();

        File.WriteAllBytes(_filePath, downloadRequest.ResponseData.Content);

        SetLoadData();
    }
}
