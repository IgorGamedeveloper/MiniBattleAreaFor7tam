using UnityEngine;

public class CommandColorHandler : MonoBehaviour
{
    [Header("��������� SpriteRenderer ������� �������������� �������� ���������:")]
    [SerializeField] private SpriteRenderer _characterEmblemSpriteRenderer;

    private void Start()
    {
        if (_characterEmblemSpriteRenderer != null)
        {
            _characterEmblemSpriteRenderer.color = Random.ColorHSV();
        }
        else
        {
            Debug.LogError("����������� ������ �� ��������� SpriteRenderer ������� ���������!");
        }
    }
}
