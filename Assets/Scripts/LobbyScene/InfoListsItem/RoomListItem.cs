using IMG.Network;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class RoomListItem : MonoBehaviour
{
    [Header("—сылка на текстовое поле названи€ комнаты:")]
    [SerializeField] private TMP_Text _roomNameTextField;

    private RoomInfo _roomInfo;




    public void UpdateData(RoomInfo roomInfo)
    {
        _roomInfo = roomInfo;
        _roomNameTextField.text = _roomInfo.Name;
    }

    public void OnClick()
    {
        NetworkManager.Instance.JoinRoom(_roomInfo);
    }
}
