using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MenuController : MonoBehaviour
{
    [SerializeField] Image bg;
    // Start is called before the first frame update
    void Start()
    {
        Color.RGBToHSV(bg.color,out float h, out float s, out float v);

        DOTween.To(() => h, x => h = x, 1, 20f)
            .OnUpdate(() =>
            {
                  Color newColor = Color.HSVToRGB(h, s, v);
                  bg.color = newColor;
            }).SetLoops(-1, LoopType.Yoyo);
    }
}
