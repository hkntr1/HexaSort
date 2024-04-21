using UnityEngine;
using DG.Tweening;
public class RandomMover : MonoBehaviour
{
    public float speed = 5f;
    private RectTransform rectTransform;
    private Vector2 direction;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        // Rastgele bir başlangıç yönü belirle
        direction = Random.insideUnitCircle.normalized;
    }

    void Update()
    {
       {
        // Hareket yönüne göre pozisyonu güncelle
        rectTransform.anchoredPosition += direction * speed * Time.deltaTime;

        // Ekranın sınırlarını al
        float screenWidth = Screen.width;
        float screenHeight = Screen.height;

        // Sprite'ın sınırlarını al
        float spriteWidth = rectTransform.rect.width;
        float spriteHeight = rectTransform.rect.height;

        // Ekran sınırlarına ulaşıldığında hareket yönünü tersine çevir
        if (rectTransform.anchoredPosition.x < -screenWidth / 2 + spriteWidth / 2 ||
            rectTransform.anchoredPosition.x > screenWidth / 2 - spriteWidth / 2)
        {
            direction.x *= -1;
            transform.DOScale(Vector3.one * 1.1f, 0.1f).OnComplete(() =>
            {
               transform.DOScale(Vector3.one, 0.1f);
            });
        }

        if (rectTransform.anchoredPosition.y < -screenHeight / 2 + spriteHeight / 2 ||
            rectTransform.anchoredPosition.y > screenHeight / 2 - spriteHeight / 2)
        {
            direction.y *= -1;
            transform.DOScale(Vector3.one * 1.2f, 0.1f).OnComplete(() =>
            {
              transform.DOScale(Vector3.one, 0.1f);
            });
        }
    }
    }
}
