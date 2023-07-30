using UnityEngine;
using Photon.Pun;
using TMPro;
using Photon.Realtime;

public class PlayerListItem : MonoBehaviourPunCallbacks
{
    [Header("—сылка на текстовое поле имени игрока:")]
    [SerializeField] private TMP_Text _playerNameTextField;

    private Player _player;




    public void UpdatePlayerData(Player player)
    {
        _player = player;
        _playerNameTextField.text = _player.NickName;
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (_player == otherPlayer)
        {
            Destroy(gameObject);
        }
    }

    public override void OnLeftRoom()
    {
        Destroy(gameObject);
    }

}
