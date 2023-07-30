using UnityEngine;

public class SpawnPointItem : MonoBehaviour
{
    public bool IsBusy { get; private set; }






    public void SetBusy(bool isBusy)
    {
        IsBusy = isBusy;
    }

    public void OnTriggerEnter2D(Collider2D collision)
    {
        SetBusy(false);
    }

    public void OnTriggerExit2D(Collider2D collision)
    {
        SetBusy(true);
    }
}
