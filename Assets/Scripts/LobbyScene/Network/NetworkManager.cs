using UnityEngine;
using Photon.Pun;
using TMPro;
using IMG.UI;
using Photon.Realtime;
using System.Collections.Generic;
using IMG.SceneManagement;

namespace IMG.Network
{
    public class NetworkManager : MonoBehaviourPunCallbacks
    {
        public static NetworkManager Instance { get; private set; }

        //  ���� ����������� � �������

        [Header("���� ����� ����� ������:")]
        [SerializeField] private TMP_InputField _playerNickNameInputField;


        // ���� �������� �������:
        [Space(20f)]
        [Header("��������� ���� ����� �������� ������� ��� ��������:")]
        [SerializeField] private TMP_InputField _createRoomNameInputTextField;

        //  ���� ������������� � �������:

        [Space(50f)]
        [Header("������ ������� ������ ������:")]
        [SerializeField] private GameObject _roomListArea;

        [Space(10f)]
        [Header("������ ������ �������� � �������:")]
        [SerializeField] private GameObject _roomButton;

        //  ���� �������:

        [Space(50f)]
        [Header("��������� ���� ��� ����������� ����� �������:")]
        [SerializeField] private TMP_Text _roomNameTextField;

        [Space(10f)]
        [Header("������ ������� ������ �������:")]
        [SerializeField] private GameObject _playersListArea;

        [Space(10f)]
        [Header("������ ���������� ���� � ������ ������:")]
        [SerializeField] private GameObject _playerNameField;

        [Space(10f)]
        [Header("������ �� ������ �������� � �������:")]
        [SerializeField] private GameObject _startGameButton;



        private string _loadMenuName = "LoadMenu";
        private string _mainMenuName = "MainMenu";
        private string _roomMenuName = "RoomMenu";
        private string _errorMenuName = "ErrorMenu";

        private string _defaultNickName = "�����_";







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

        public void ConnectToMaster()
        {
            PhotonNetwork.ConnectUsingSettings();
            MenuItemsManager.Instance.OpenMenu(_loadMenuName);

            if (string.IsNullOrEmpty(_playerNickNameInputField.text) == false)
            {
                PhotonNetwork.NickName = _playerNickNameInputField.text;
            }
            else
            {
                PhotonNetwork.NickName = $"{_defaultNickName}{Random.Range(1, 9998)}";
            }
        }

        public override void OnConnectedToMaster()
        {
            PhotonNetwork.JoinLobby();
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        public override void OnJoinedLobby()
        {
            MenuItemsManager.Instance.OpenMenu(_mainMenuName);
        }




        //  �������� �������:

        public void CreateRoom()
        {
            if (_createRoomNameInputTextField != null)
            {
                if (string.IsNullOrEmpty(_createRoomNameInputTextField.text) == false)
                {
                    PhotonNetwork.CreateRoom(_createRoomNameInputTextField.text);   //  ������� ������� � ������ �� ���������� ���� �����.
                }
                else
                {
                    Debug.LogError("������������ ������ �������� �������!");
                    return;
                }
            }
            else
            {
                Debug.LogError("����������� ������ �� ���� ����� �������� �������!");
                return;
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)   //  ��� ������ �������� �������.
        {
            ShowError(message);
        }



        //  ������������� � �������:

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            for (int i = 0; i < _roomListArea.transform.childCount; i++)    //  ���������� ������������ ������.
            {
                Destroy(_roomListArea.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < roomList.Count; i++)    //  ������� ����� ������ ������ �� ������ ��������� ������.
            {
                if (roomList[i].RemovedFromList == true)
                {
                    continue;
                }

                Instantiate(_roomButton, _roomListArea.transform).GetComponent<RoomListItem>().UpdateData(roomList[i]);
            }
        }

        public void JoinRoom(RoomInfo roomInfo)
        {
            PhotonNetwork.JoinRoom(roomInfo.Name);  //  �������������� � ������� � ������.
            MenuItemsManager.Instance.OpenMenu(_loadMenuName);
        }

        public override void OnJoinedRoom()
        {
            _roomNameTextField.text = PhotonNetwork.CurrentRoom.Name;
            MenuItemsManager.Instance.OpenMenu(_roomMenuName);

            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < _playersListArea.transform.childCount; i++)   //  ���������� ������������ ������.
            {
                Destroy(_playersListArea.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < players.Length; i++)    //  ������� ����� ������ ������� �� �� ����������.
            {
                Instantiate(_playerNameField, _playersListArea.transform).GetComponent<PlayerListItem>().UpdatePlayerData(players[i]);
            }

            _startGameButton.SetActive(PhotonNetwork.IsMasterClient);  //  ������ �������� ������ �������� � ������� ������ ��� �������.
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            ShowError(message);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)  //  ��������� ��� ������ ������ � ������ ��� �������� � �������.
        {
            Instantiate(_playerNameField, _playersListArea.transform).GetComponent<PlayerListItem>().UpdatePlayerData(newPlayer);
        }



        //  ����� �� �������:

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            MenuItemsManager.Instance.OpenMenu(_mainMenuName);
        }

        public override void OnLeftRoom()
        {
            MenuItemsManager.Instance.OpenMenu(_mainMenuName);
        }



        //  ������� � ������� �������:

        public void StartGame()
        {
            AsyncSceneLoadManager.Instance.StartLoadSceneAsync();
        }



        //  ��������� ������:

        private void ShowError(string message)  //  ������� � ���� ������ ������ � ������� ��������� ���� ������.
        {
            MenuItemsManager.Instance.OpenMenu(_errorMenuName);
            ErrorTextHandler.Instance.SetErrorText(message);
        }
    }
}
