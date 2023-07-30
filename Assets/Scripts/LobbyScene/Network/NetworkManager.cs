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

        //  МЕНЮ ПОДКЛЮЧЕНИЯ К СЕРВЕРУ

        [Header("Поле ввода имени игрока:")]
        [SerializeField] private TMP_InputField _playerNickNameInputField;


        // МЕНЮ СОЗДАНИЯ КОМНАТЫ:
        [Space(20f)]
        [Header("Текстовое поле ввода названия комнаты при создании:")]
        [SerializeField] private TMP_InputField _createRoomNameInputTextField;

        //  МЕНЮ ПРИСОЕДИНЕНИЯ К КОМНАТЕ:

        [Space(50f)]
        [Header("Обьект области списка комнат:")]
        [SerializeField] private GameObject _roomListArea;

        [Space(10f)]
        [Header("Префаб кнопки перехода в комнату:")]
        [SerializeField] private GameObject _roomButton;

        //  МЕНЮ КОМНАТЫ:

        [Space(50f)]
        [Header("Текстовое поле для отображения имени комнаты:")]
        [SerializeField] private TMP_Text _roomNameTextField;

        [Space(10f)]
        [Header("Обьект области списка игроков:")]
        [SerializeField] private GameObject _playersListArea;

        [Space(10f)]
        [Header("Префаб текстового поля с именем игрока:")]
        [SerializeField] private GameObject _playerNameField;

        [Space(10f)]
        [Header("Ссылка на кнопку перехода в комнату:")]
        [SerializeField] private GameObject _startGameButton;



        private string _loadMenuName = "LoadMenu";
        private string _mainMenuName = "MainMenu";
        private string _roomMenuName = "RoomMenu";
        private string _errorMenuName = "ErrorMenu";

        private string _defaultNickName = "Игрок_";







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




        //  СОЗДАНИЕ КОМНАТЫ:

        public void CreateRoom()
        {
            if (_createRoomNameInputTextField != null)
            {
                if (string.IsNullOrEmpty(_createRoomNameInputTextField.text) == false)
                {
                    PhotonNetwork.CreateRoom(_createRoomNameInputTextField.text);   //  Создаем комнату с именем из текстового поля ввода.
                }
                else
                {
                    Debug.LogError("Недопустимый формат названия комнаты!");
                    return;
                }
            }
            else
            {
                Debug.LogError("Отсутствует ссылка на поле ввода названия комнаты!");
                return;
            }
        }

        public override void OnCreateRoomFailed(short returnCode, string message)   //  При ошибке создания комнаты.
        {
            ShowError(message);
        }



        //  ПРИСОЕДИНЕНИЕ К КОМНАТЕ:

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            for (int i = 0; i < _roomListArea.transform.childCount; i++)    //  Уничтожаем существующий список.
            {
                Destroy(_roomListArea.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < roomList.Count; i++)    //  Создаем новый список комнат из списка созданных комнат.
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
            PhotonNetwork.JoinRoom(roomInfo.Name);  //  Присоединяемся к комнате с именем.
            MenuItemsManager.Instance.OpenMenu(_loadMenuName);
        }

        public override void OnJoinedRoom()
        {
            _roomNameTextField.text = PhotonNetwork.CurrentRoom.Name;
            MenuItemsManager.Instance.OpenMenu(_roomMenuName);

            Player[] players = PhotonNetwork.PlayerList;

            for (int i = 0; i < _playersListArea.transform.childCount; i++)   //  Уничтожаем существующий список.
            {
                Destroy(_playersListArea.transform.GetChild(i).gameObject);
            }

            for (int i = 0; i < players.Length; i++)    //  Создаем новый список игроков по их количеству.
            {
                Instantiate(_playerNameField, _playersListArea.transform).GetComponent<PlayerListItem>().UpdatePlayerData(players[i]);
            }

            _startGameButton.SetActive(PhotonNetwork.IsMasterClient);  //  Делаем активной кнопку перехода в комнату только для мастера.
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            ShowError(message);
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)  //  Добавляем имя нового игрока в список при переходе в комнату.
        {
            Instantiate(_playerNameField, _playersListArea.transform).GetComponent<PlayerListItem>().UpdatePlayerData(newPlayer);
        }



        //  ВЫХОД ИЗ КОМНАТЫ:

        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
            MenuItemsManager.Instance.OpenMenu(_mainMenuName);
        }

        public override void OnLeftRoom()
        {
            MenuItemsManager.Instance.OpenMenu(_mainMenuName);
        }



        //  ПЕРЕХОД В ИГРОВУЮ КОМНАТУ:

        public void StartGame()
        {
            AsyncSceneLoadManager.Instance.StartLoadSceneAsync();
        }



        //  ОБРАБОТКА ОШИБОК:

        private void ShowError(string message)  //  Перейти в меню вывода ошибки и вывести полученый тект ошибки.
        {
            MenuItemsManager.Instance.OpenMenu(_errorMenuName);
            ErrorTextHandler.Instance.SetErrorText(message);
        }
    }
}
