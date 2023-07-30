using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CharacterWeapon : MonoBehaviour
{
    [Header("��������� PhotonView ��������� ������:")]
    [SerializeField] private PhotonView _photonView;

    [Space(40f)]
    [Header("���� �������� � ������:")]
    [SerializeField] private int _ammoLoaded = 10;

    [Space(10f)]
    [Header("������������ ����� ���������� ���� � ������:")]
    [SerializeField] private int _maxAmmoLoaded = 10;

    [Space(10f)]
    [Header("���������� ���� � ������:")]
    [SerializeField] private int _ammoReserve = 100;

    [Space(10f)]
    [Header("������������ ����� ���� � ������:")]
    [SerializeField] private int _maxAmmoReserve = 1000;

    [Space(10f)]
    [Header("����� ����������� ������:")]
    [SerializeField] private float _reloadTime = 0.8f;

    [Space(10f)]
    [Header("��������� Transform ������������� ������� ����:")]
    [SerializeField] private Transform _bulletsParentTransform;

    [Space(10f)]
    [Header("��������� Transform ����� ��������� ���� (�� ������):")]
    [SerializeField] private Transform _shotPointTransform;
    [SerializeField] private Transform _scopeTransform;

    [Space(10f)]
    [Header("���� ��������� �����")]
    [SerializeField] private int _bulletDamage = 1;

    [Space(10f)]
    [Header("�������� ������ ����:")]
    [SerializeField] private float _bulletMoveSpeed = 6f;

    [Space(10f)]
    [Header("����� �� ������������ ����:")]
    [SerializeField] private float _bulletTimeToDestroy = 5f;

    [Space(20f)]
    [Header("���� � ������� ����:")]
    [SerializeField] private string _pathToBulletPrefab = "PhotonPrefabs/Bullet";

    private bool _canShot = true;

    private Vector3 _bulletDirection;








    private void SetCanShot(bool canShot)
    {
        _canShot = canShot;
    }

    private void ChangeCanShotStatus()
    {
        _canShot = !_canShot;
    }

    public void TryShot()
    {
        _bulletDirection = _scopeTransform.position - _shotPointTransform.position;

        if (_canShot == true && _ammoLoaded > 0)
        {
            _ammoLoaded--;
            DoShot(_bulletDirection);
        }
        else if (_ammoLoaded <= 0 && _ammoReserve > 0)
        {
            Reload();
        }
        else
        {
            DontShot();
        }
    }

    private void DoShot(Vector3 direction)
    {
        GameObject bullet = PhotonNetwork.Instantiate(_pathToBulletPrefab, _shotPointTransform.position, transform.rotation);
        bullet.GetComponent<BulletBehaviour>().SetInputData(_bulletDamage, direction, _bulletMoveSpeed, _bulletTimeToDestroy);
    }

    public void DontShot()
    {
        Debug.Log("�� ���������� ����������!");
    }

    public void Reload()
    {
        if (_ammoReserve > 0)
        {
            SetCanShot(false);

            if (_ammoReserve + _ammoLoaded < _maxAmmoLoaded)
            {
                _ammoLoaded = _ammoLoaded + _ammoReserve;
                _ammoReserve = 0;
                Invoke("ChangeCanShotStatus", _reloadTime);
                return;
            }
            else if (_ammoReserve + _ammoLoaded >= _maxAmmoLoaded)
            {
                _ammoReserve -= (_maxAmmoLoaded - _ammoLoaded);
                _ammoLoaded = _maxAmmoLoaded;
                Invoke("ChangeCanShotStatus", _reloadTime);
                return;
            }
        }
        else
        {
            DontShot();
        }
    }

    public void AddAmmo(int ammoAmount)
    {
        if (_ammoReserve + ammoAmount > _maxAmmoReserve)
        {
            _ammoReserve = _maxAmmoReserve;
        }
        else
        {
            _ammoReserve += ammoAmount;
        }
    }
}
