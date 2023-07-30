using UnityEngine;

namespace IMG.UI
{

    public class MenuItemsManager : MonoBehaviour
    {
        public static MenuItemsManager Instance { get; private set; }

        private MenuItem[] _menuItems;

        private bool _menuFinded = false;

        [Header("���� ������� ������� ������� ��� ������:")]
        [SerializeField] private string _startMenuName = "ConnectMenu";








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

            _menuItems = FindObjectsOfType<MenuItem>(true);

            OpenMenu(_startMenuName);
        }

        public void OpenMenu(string menuName)
        {
            if (string.IsNullOrEmpty(menuName) == false)
            {
                if (_menuItems != null && _menuItems.Length > 0)
                {
                    for (int i = 0; i < _menuItems.Length; i++)
                    {
                        if (_menuItems[i].MenuName == menuName)
                        {
                            _menuFinded = true;
                            _menuItems[i].SetActive(true);
                        }
                        else
                        {
                            _menuItems[i].SetActive(false);
                        }
                    }

                    if (_menuFinded == false)
                    {
                        Debug.LogError($"���� � ������������� ������ {menuName} �� �������!");
                    }
                    else
                    {
                        _menuFinded = false;
                        return;
                    }
                }
                else
                {
                    Debug.LogError("������ ������� ���� ����!");
                }
            }
            else
            {
                Debug.LogError("����������� ��� ������ ����!");
            }
        }
    }
}
