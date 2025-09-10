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

    private void Start()
    { 
        if (GoogleDriveTools.FileList().Count <= 0)
        {
            _loadedText.text = "Данные отсутствуют";
            _loadedNums.text = "Данные отсутствуют";
        }
    }

    public void OnEnterClick()
    {

    }

    public void OnSaveClick()
    {

    }
}
