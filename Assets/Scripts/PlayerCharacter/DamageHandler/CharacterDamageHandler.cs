using Photon.Pun;
using UnityEngine;

public class CharacterDamageHandler : MonoBehaviour
{
    [Header("������ �� ��������� PhotonView ��������� ������:")] 
    [SerializeField] private PhotonView _photonView;

    [Space(10f)]
    [Header("������ �� ������� ������ ���������:")]
    [SerializeField] private GameObject _player;

    [Space(20f)]
    [Header("�������� ���������:")]
    [SerializeField] private int _health = 10;

    [Space(10f)]
    [Header("������������ �������� ���������:")]
    [SerializeField] private int _maxHealth = 10;

    [Space(10f)]
    [Header("����� ����������� ��� ����������� ����� ������:")]
    [SerializeField] private float _timeToRespawn = 2.0f;







    [PunRPC]
    public void TakeDamage(int photonViewId, int damage)
    {
        if (_photonView != null)
        {
            if (photonViewId == _photonView.ViewID)
            {
                _health -= damage;

                if (_health <= 0)
                {
                    Death();
                }
            }
        }
        else
        {
            Debug.Log("����������� ������ �� ��������� PhotonView!");
        }
    }

    public void Death()
    {
        _player.SetActive(false);
        Invoke("Respawn", _timeToRespawn);
    }

    public void Respawn()
    {
        _health = _maxHealth;
        _player.SetActive(true);
    }
}
