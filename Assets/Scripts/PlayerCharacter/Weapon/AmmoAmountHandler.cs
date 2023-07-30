using Photon.Pun;
using TMPro;
using UnityEngine;

public class AmmoAmountHandler : MonoBehaviour
{
    [Header("������ �� ��������� CharacterWeapon ��������� ������:")]
    [SerializeField] private CharacterWeapon _weapon;

    [Header("������ �� ��������� ���� ����������� ���������� ����:")]
    [SerializeField] private TMP_Text _ammoAmountTextField;

    [Space(10f)]
    [Header("������ �� ��������� ���� ���������� ���������� ����:")]
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
            Debug.LogError("����������� ������ �� ��������� CharacterWeapon! ���������� UI ������� ���� ����������!");
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
            Debug.LogError("����������� ������ �� ��������� ���� ���������� ����! �������� ������ ����������!");
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
            Debug.LogError("����������� ������ �� ��������� ���� ���������� ���������� ����! �������� ������ ����������!");
        }
    }
}
