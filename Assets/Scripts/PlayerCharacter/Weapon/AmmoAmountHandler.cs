using Photon.Pun;
using TMPro;
using UnityEngine;

public class AmmoAmountHandler : MonoBehaviour
{
    [Header("Ссылка на компонент CharacterWeapon персонажа игрока:")]
    [SerializeField] private CharacterWeapon _weapon;

    [Header("Ссылка на текстовое поле заряженного количества пуль:")]
    [SerializeField] private TMP_Text _ammoAmountTextField;

    [Space(10f)]
    [Header("Ссылка на текстовое поле резервного количества пуль:")]
    [SerializeField] private TMP_Text _ammoReserveTextField;





    private void OnEnable()
    {
        if (_weapon != null)
        {
            _weapon.ammoValueChanged += UpdateLoadedAmmo;
            _weapon.ammoReserveValueChanged += UpdateReserveAmmo;
        }
        else
        {
            Debug.LogError("Отсутствует ссылка на компонент CharacterWeapon! Обновления UI статуса пуль невозможна!");
        }
    }

    private void OnDisable()
    {
        _weapon.ammoValueChanged -= UpdateLoadedAmmo;
        _weapon.ammoReserveValueChanged -= UpdateReserveAmmo;
    }

    private void UpdateLoadedAmmo(int ammoValue)
    {
        if (_ammoAmountTextField != null)
        {
            _ammoAmountTextField.text = ammoValue.ToString() + " /";
        }
        else
        {
            Debug.LogError("Отсутствует ссылка на текстовое поле количества пуль! Обновить статус невозможно!");
        }
    }

    private void UpdateReserveAmmo(int ammoValue)
    {
        if (_ammoReserveTextField != null)
        {
            _ammoReserveTextField.text = ammoValue.ToString();
        }
        else
        {
            Debug.LogError("Отсутствует ссылка на текстовое поле резервного количества пуль! Обновить статус невозможно!");
        }
    }
}
