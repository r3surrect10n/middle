using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SaveLoadScript : MonoBehaviour
{
    [SerializeField] private InputField _textField;
    [SerializeField] private InputField _numField;

    [SerializeField] private Text _enteredText;
    [SerializeField] private Text _enteredNums;
    [SerializeField] private Text _loadedText;
    [SerializeField] private Text _loadedNums;
    [SerializeField] private Text _gdLoadedText;
    [SerializeField] private Text _gdLoadedNums;

    private string _filePath;

    private void Start()
    {
        _filePath = Path.Combine(Application.persistentDataPath, "gameData.json");

        CheckData();

        //Debug.Log(GoogleDriveTools.FileList().Count);
    }

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

        GoogleDriveTools.Upload(_filePath);
        
        Debug.Log("Saved");
    }

    public void OnLoadClick()
    {
        LoadData();
    }

    private void SaveData()
    {
        GameData gameData = new GameData();

        if (_enteredText.text != "" && _enteredNums.text != "")
        {
            gameData.SavedText = _enteredText.text;
            gameData.SavedNums = _enteredNums.text;

            string json = JsonUtility.ToJson(gameData, true);

            File.WriteAllText(_filePath, json);
        }

    }

    private void LoadData()
    {
        if (File.Exists(_filePath))
        {
            string json = File.ReadAllText(_filePath);

            GameData gameData = JsonUtility.FromJson<GameData>(json);

            _loadedText.text = gameData.SavedText;
            _loadedNums.text = gameData.SavedNums;
        }
    }

    private void CheckData()
    {
        if (GoogleDriveTools.FileList().Count <= 0)
        {
            _gdLoadedText.text = "Данные отсутствуют";
            _gdLoadedNums.text = "Данные отсутствуют";
        }
        else        

        if (!File.Exists(_filePath))
        {
            _loadedText.text = "Данные отсутствуют";
            _loadedNums.text = "Данные отсутствуют";
        }
    }
}
