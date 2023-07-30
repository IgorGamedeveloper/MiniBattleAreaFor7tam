using UnityEngine;

public class CommandColorHandler : MonoBehaviour
{
    [Header("Компонент SpriteRenderer эмблемы принадлежности комманды персонажа:")]
    [SerializeField] private SpriteRenderer _characterEmblemSpriteRenderer;

    private void Start()
    {
        if (_characterEmblemSpriteRenderer != null)
        {
            _characterEmblemSpriteRenderer.color = Random.ColorHSV();
        }
        else
        {
            Debug.LogError("Отсутствует ссылка на компонент SpriteRenderer эмблемы персонажа!");
        }
    }
}
