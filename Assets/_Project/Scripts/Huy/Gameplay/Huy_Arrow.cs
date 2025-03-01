using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Huy_Arrow : MonoBehaviour
{
    [SerializeField] private Sprite spriteArrowMustHit;
    [SerializeField] private Sprite spriteArrow;
    [SerializeField] private Sprite spriteTailMustHit;
    [SerializeField] private Sprite spriteTail;
    
    [SerializeField] private SpriteRenderer spriteRendererArrow;
    [SerializeField] private SpriteRenderer spriteRendererTail;

    [SerializeField] private BoxCollider2D collider2D;
    
    
    private bool isMustHit;
    private bool isPress;
    public bool isCollider;
    
    [SerializeField] private bool isCorrectArrow;
    
    private float timerAnim;
    private int indexArrow;

    private const float DeltaMoveMustHit = 4;

    public bool IsPress
    {
        get => isPress;
        set => isPress = value;
    }

    private void Awake()
    {
        spriteRendererArrow = GetComponent<SpriteRenderer>();
        spriteRendererTail = GetComponentInChildren<SpriteRenderer>();
        collider2D = GetComponent<BoxCollider2D>();
    }

    public void SetupArrow(float timeMove, float timeTail, int indexArrow, bool isMustHit, float deltaMove, int serial,
        int sumArrow)
    {
        this.isMustHit = isMustHit;
        transform.name = serial.ToString();
        isCollider = false;
        isCorrectArrow = false;

        if (sumArrow > 0)
        {
            Vector3 posCur = transform.position;
            posCur.z = (float)serial/(float)sumArrow;
            transform.position = posCur;
        }
        
        this.indexArrow = indexArrow;
        timeTail -= 0.22f;
        if (timeTail <= 0.22f)
        {
            spriteRendererTail.enabled = false;
            collider2D.size = new Vector2(1.25f, 1.25f);
            collider2D.offset = Vector2.zero;
            timeTail = 0;
        }
        else
        {
            spriteRendererTail.enabled = true;
            float sizeY = timeTail * 10;
            collider2D.size = new Vector2(1.25f, 1.25f + sizeY);
            spriteRendererTail.size = new Vector2(0.5f, sizeY);
        }

        timerAnim = timeTail;
        float newTimeMove = deltaMove / timeMove;
        float posDesY = transform.position.y + timeTail * 10 + deltaMove;
        
        if (isMustHit)
        {
            spriteRendererArrow.sortingOrder = 20;
            spriteRendererTail.sortingOrder = 19;

            newTimeMove = (timeTail * 10 + deltaMove + DeltaMoveMustHit) / newTimeMove;
            spriteRendererArrow.sprite = spriteArrowMustHit;
            spriteRendererTail.sprite = spriteTailMustHit;
            posDesY += DeltaMoveMustHit;
        }
        else
        {
            spriteRendererArrow.sortingOrder = 15;
            spriteRendererTail.sortingOrder = 13;
            
            newTimeMove = (timeTail * 10 + deltaMove) / newTimeMove;
            spriteRendererArrow.sprite = spriteArrow;
            spriteRendererTail.sprite = spriteTail;
        }
        
        transform.DOMoveY(posDesY, newTimeMove).SetEase(Ease.Linear).OnComplete(() =>
        {
            //Destroy Self
            DestroySelf();
        });
    }

    private void DestroySelf()
    {
        DOTween.Kill(transform);
        Destroy(gameObject);
    }
}
