using Photon.Pun;
using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Rigidbody2D _rigidbody;

    private Vector3 _moveDirection;

    private float _moveSpeed;

    private float _timeToDeativate;


    private int _bulletDamage = 1;

    [Header("Тег персонажей игроков:")]
    [SerializeField] private string _playerTag = "Tag_Player";





    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public void SetInputData(int damage, Vector3 moveDirection, float moveSpeed, float lifeTime)
    {
        _bulletDamage = damage;
        _moveDirection = moveDirection;
        _moveSpeed = moveSpeed;
        _timeToDeativate = lifeTime;
    }

    private void FixedUpdate()
    {
        if (_timeToDeativate > 0)
        {
            _timeToDeativate -= Time.deltaTime;
            _rigidbody.velocity = _moveDirection * _moveSpeed;
        }
        else
        {
            _rigidbody.velocity = Vector2.zero;
            DestroyBullet();
            return;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == _playerTag)
        {
            if (collision.gameObject.TryGetComponent(out PhotonView photonView) == true)
            {
                photonView.RPC("TakeDamage", RpcTarget.All, photonView.ViewID, _bulletDamage);
            }
        }

        DestroyBullet();
    }


    public void DestroyBullet()
    {
        PhotonNetwork.Destroy(gameObject);
    }
}
