using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ErrorTextHandler : MonoBehaviour
{
    public static ErrorTextHandler Instance { get ; private set; }

    [SerializeField] private TMP_Text _errotTextField;




    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(this);
        }
    }

    public void SetErrorText(string errorText)
    {
        if (_errotTextField != null)
        {
            _errotTextField.text = $"Возникла ошибка! {errorText}";
        }
        else
        {
            Debug.LogError("Отсутствует ссылка на текстовое поле вывода ошибки!");
        }
    }
}
