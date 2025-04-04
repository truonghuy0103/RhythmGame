using System.Collections;
using System.Collections.Generic;
using Huy;
using UnityEngine;
using UnityEngine.UI;
using Huy_Core;
using TMPro;
using UnityEditor;

public class Huy_UIGameplay : BaseUI
{
    [Header("-----Text-----")]
    [SerializeField] private TextMeshProUGUI txtSong;
    [SerializeField] private TextMeshProUGUI txtMiss;
    [SerializeField] private TextMeshProUGUI txtScore;
    
    [SerializeField] private TextMeshProUGUI txtTimer;

    [Header("-----Image-----")] 
    [SerializeField] private Image imgReady;
    [SerializeField] private Image imgDifficult;

    [SerializeField] private Image imgIconBoy;
    [SerializeField] private Image imgIconBoss;
    
    [SerializeField] private List<Image> lsArrowTops = new List<Image>();
    [SerializeField] private List<Image> lsImgBtns = new List<Image>();
    
    [Header("-----Transform & GameObject-----")]
    [SerializeField] private Transform transTarget;
    public List<Transform> lsTransSpawnArrowBot = new List<Transform>();
    [SerializeField] private List<GameObject> lsGoEffectArrows = new List<GameObject>();
    
    [SerializeField] private List<GameObject> lsGoTexts = new List<GameObject>();
    
    [Header("-----Sprite-----")]
    [SerializeField] private List<Sprite> lsSpriteArrowNormals = new List<Sprite>();
    [SerializeField] private List<Sprite> lsSpriteArrowCorrects = new List<Sprite>();
    
    [SerializeField] private List<Sprite> lsSpriteDifficults = new List<Sprite>();
    
    [SerializeField] private List<Sprite> lsSpriteCountdowns = new List<Sprite>();
    
    [SerializeField] private List<Sprite> lsSpriteButtonOns = new List<Sprite>();
    [SerializeField] private List<Sprite> lsSpriteButtonOffs = new List<Sprite>();
    
    [SerializeField] private Sprite spriteBoyNormal;
    [SerializeField] private Sprite spriteBoyLose;

    private Sprite spriteEnemyNormal;
    private Sprite spriteEnemyLose;
    
    [Header("-----Variable-----")]
    private GameplayParam gameplayParam;

    [SerializeField] private Slider sliderHP;

    [SerializeField] private Vector3 defaultScaleBtn = new Vector3(0.9f, 0.9f, 0);
    private GameState gameState;

    public override void OnInit()
    {
        base.OnInit();
    }

    public override void OnSetup(UIParam param = null)
    {
        base.OnSetup(param);
        
        gameplayParam = (GameplayParam)param;
        txtSong.text = gameplayParam.nameSong;
        imgDifficult.sprite = lsSpriteDifficults[(int)gameplayParam.difficult];
        imgDifficult.SetNativeSize();
        sliderHP.maxValue = gameplayParam.maxValueSlider;
        sliderHP.value = gameplayParam.maxValueSlider / 2;

        imgReady.sprite = lsSpriteCountdowns[0];
        imgReady.SetNativeSize();

        for (int i = 0; i < lsGoTexts.Count; i++)
        {
            lsGoTexts[i].SetActive(false);
        }

        imgIconBoy.sprite = spriteBoyNormal;
        imgIconBoy.SetNativeSize();

        for (int i = 0; i < lsImgBtns.Count; i++)
        {
            lsImgBtns[i].sprite = lsSpriteButtonOffs[i];
            lsImgBtns[i].SetNativeSize();
            
            lsImgBtns[i].transform.localScale = defaultScaleBtn;
        }

        StartCoroutine(CountdownToStart(1f));
    }

    IEnumerator CountdownToStart(float timer)
    {
        gameState = GameState.Ready;    
        imgReady.gameObject.SetActive(true);
        for (int i = 0; i < lsSpriteCountdowns.Count; i++)
        {
            imgReady.sprite = lsSpriteCountdowns[i];
            imgReady.SetNativeSize();
            
            Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.One + 1);
            
            yield return new WaitForSeconds(timer);
        }
        
        imgReady.gameObject.SetActive(false);
        Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Go);
        
        yield return new WaitForSeconds(timer);
        gameState = GameState.Playing;
        Huy_SoundManager.Instance.PlaySoundBGM();
    }

    public float GetDistanceMoveArrow()
    {
        return Vector2.Distance(transTarget.position, lsTransSpawnArrowBot[0].position);
    }

    public List<Transform> GetListTransformSpawnArrow()
    {
        return lsTransSpawnArrowBot;
    }

    public List<Transform> GetListTargetArrow()
    {
        List<Transform> lsTransTarget = new List<Transform>();
        for (int i = 0; i < lsArrowTops.Count; i++)
        {
            lsTransTarget.Add(lsArrowTops[i].transform);
        }
        
        return lsTransTarget;
    }

    public void OnButtonClickDown(int index)
    {
        lsImgBtns[index].sprite = lsSpriteButtonOns[index];
        lsImgBtns[index].SetNativeSize();

        lsImgBtns[index].transform.localScale = Vector3.one;
        
        Huy_GameManager.Instance.OnButtonClickDown(index);
    }

    public void OnButtonClickUp(int index)
    {
        lsImgBtns[index].sprite = lsSpriteButtonOffs[index];
        lsImgBtns[index].SetNativeSize();

        lsImgBtns[index].transform.localScale = defaultScaleBtn;
        
        Huy_GameManager.Instance.OnButtonClickUp(index);
    }

    private void Update()
    {
        //Key down
        if (Input.GetKeyDown(KeyCode.A))
        {
            OnButtonClickDown(0);
        }
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            OnButtonClickDown(1);
        }
        
        if (Input.GetKeyDown(KeyCode.W))
        {
            OnButtonClickDown(2);
        }
        
        if (Input.GetKeyDown(KeyCode.D))
        {
            OnButtonClickDown(3);
        }
        
        //Key up
        if (Input.GetKeyUp(KeyCode.A))
        {
            OnButtonClickUp(0);
        }
        
        if (Input.GetKeyUp(KeyCode.S))
        {
            OnButtonClickUp(1);
        }
        
        if (Input.GetKeyUp(KeyCode.W))
        {
            OnButtonClickUp(2);
        }
        
        if (Input.GetKeyUp(KeyCode.D))
        {
            OnButtonClickUp(3);
        }
    }

    public void SetCorrectArrow(int index)
    {
        StartCoroutine(PlayAnimationCorrectArrow(index));
    }

    IEnumerator PlayAnimationCorrectArrow(int index)
    {
        lsArrowTops[index].sprite = lsSpriteArrowCorrects[index];
        lsArrowTops[index].SetNativeSize();
        
        GameObject goEffect = Instantiate(lsGoEffectArrows[index]);
        goEffect.transform.position = lsArrowTops[index].transform.position;
        goEffect.SetActive(true);
        
        yield return new WaitForSeconds(0.5f);
        
        lsArrowTops[index].sprite = lsSpriteArrowNormals[index];
        lsArrowTops[index].SetNativeSize();
    }

    public void UpdateScoreText(int score)
    {
        txtScore.text = "Score: " + score.ToString();
    }

    public void UpdateMissText(int miss)
    {
        txtMiss.text = string.Format("|     Miss: {0}    |", miss.ToString());
    }

    public void UpdateTimerText(float timer)
    {
        txtTimer.text = Mathf.FloorToInt(timer / 60).ToString("00") + " : " +
                        Mathf.FloorToInt(timer % 60).ToString("00");
    }

    public void ShowTextCorrect(int index)
    {
        lsGoTexts[index].SetActive(true);
    }

    public void SetSliderHP(float delta)
    {
        sliderHP.value += delta;
        if (sliderHP.value > sliderHP.maxValue / 2)
        {
            imgIconBoy.sprite = spriteBoyLose;
            imgIconBoss.sprite = spriteEnemyNormal;
        }
        else
        {
            imgIconBoy.sprite = spriteBoyNormal;
            imgIconBoss.sprite = spriteEnemyLose;
        }

        if (sliderHP.value >= sliderHP.maxValue)
        {
            //Show game lose
            Huy_GameManager.Instance.ShowGameLose();
        }
    }

    public bool CheckGameWin()
    {
        return sliderHP.value <= sliderHP.maxValue / 2;
    }

    public void SetSpriteIconBoss(Sprite spriteBossLose, Sprite spriteBossNormal)
    {
        this.spriteEnemyNormal = spriteBossNormal;
        this.spriteEnemyLose = spriteBossLose;
        SetSliderHP(0);
    }

    public void OnPauseGame_Clicked()
    {
        //Play click SFX
        Huy_SoundManager.Instance.PlaySoundSFX(SoundFXIndex.Click);
        Huy_SoundManager.Instance.PauseSoundBGM();
        gameState = GameState.PauseGame;
        UIManager.Instance.ShowUI(UIIndex.UIPause);
    }
}
