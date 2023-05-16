using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class BattleUnit : MonoBehaviour
{
    [SerializeField] GeneralBase _base;
    [SerializeField] int level;
    [SerializeField] bool isPlayerUnit;

    public Battle entity { get; set; }

    Image image;
    RectTransform imageRectTransform;
    Vector3 originalPos;
    Color originalColor;

    private void Awake()
    {
        image = GetComponent<Image>();
        imageRectTransform = image.GetComponent<RectTransform>();
        originalPos = imageRectTransform.localPosition;
        originalColor = image.color;
    }

    public void Setup()
    {
        entity = new Battle(_base, level);

        if (isPlayerUnit)
        {
            image.sprite = entity.Base.backSprite;
        }
        else
        {
            image.sprite = entity.Base.frontSprite;
        }

        image.color = originalColor;

        PlayEnterAnimation();
    }

    public void PlayEnterAnimation()
    {
        if (isPlayerUnit)
        {
            imageRectTransform.localPosition = new Vector3(-550f, originalPos.y);
        }
        else
        {
            imageRectTransform.localPosition = new Vector3(550f, originalPos.y);
        }

        var sequence = DOTween.Sequence();
        sequence.Append(imageRectTransform.DOLocalMoveX(originalPos.x, 1f));
        sequence.Join(imageRectTransform.DOLocalMoveY(originalPos.y, 1f));
    }

    public void PlayAttackAnimation()
    {
        var sequence = DOTween.Sequence();

        if (isPlayerUnit)
        {
            sequence.Append(imageRectTransform.DOLocalMoveX(originalPos.x + 50f, 0.25f));
        }
        else
        {
            sequence.Append(imageRectTransform.DOLocalMoveX(originalPos.x - 50f, 0.25f));
        }

        sequence.Append(imageRectTransform.DOLocalMoveX(originalPos.x, 0.25f));
    }

    public void PlayHitAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(image.DOColor(Color.red, 0.15f));
        sequence.Append(image.DOColor(originalColor, 0.15f));
    }

    public void PlayFaintAnimation()
    {
        var sequence = DOTween.Sequence();

        sequence.Append(imageRectTransform.DOLocalMoveY(originalPos.y - 150f, 0.5f));
        sequence.Join(image.DOFade(0f, 0.5f));
    }
}

