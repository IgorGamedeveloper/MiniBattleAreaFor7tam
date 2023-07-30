using UnityEngine;
using Photon.Pun;

public class DuplicateDestroyer : MonoBehaviour
{
    [SerializeField] private PhotonView _photonView;

    [Space(15f)]
    [SerializeField] private GameObject[] _checkedMineGameObjects;
    [Space(10f)]
    [SerializeField] private Component[] _checkedMineComponents;






    private void Start()
    {
        DestroyComponents();
    }

    private void DestroyComponents()
    {
        if (_photonView != null)
        {
            if (_photonView.IsMine == false)
            {
                if (_checkedMineGameObjects != null && _checkedMineGameObjects.Length > 0)
                {
                    for (int i = 0; i < _checkedMineGameObjects.Length; i++)
                    {
                        Destroy(_checkedMineGameObjects[i]);
                    }
                }

                if (_checkedMineComponents != null && _checkedMineComponents.Length > 0)
                {
                    for (int j = 0; j < _checkedMineComponents.Length; j++)
                    {
                        Destroy(_checkedMineComponents[j]);
                    }
                }
            }
        }
        else
        {
            Debug.LogError($"Не удалось обратсяться к компоненту PhotonView! {this}");
        }

    }
}
