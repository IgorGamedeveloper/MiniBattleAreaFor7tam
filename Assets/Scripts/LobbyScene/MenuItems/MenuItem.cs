using UnityEngine;

namespace IMG.UI
{
    public class MenuItem : MonoBehaviour
    {
        [SerializeField] private string _menuName;
        public string MenuName { get { return _menuName; } }







        public void SetActive(bool isActive)
        {
            gameObject.SetActive(isActive);
        }
    }
}
