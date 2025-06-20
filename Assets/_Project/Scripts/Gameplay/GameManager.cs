using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Huy_Core;
using Newtonsoft.Json;
using UnityEngine.Serialization;

namespace Huy
{
    public class GameManager : SingletonMono<GameManager>
    {
        public GameSave GameSave { get; set; }
        
        public UIGameplay uiGameplay;

        [Header("---Transform and Game Object---")] 
        [SerializeField] private List<GameObject> lsPrefabArrows = new List<GameObject>();

        [SerializeField] private List<Transform> lsContainSpawnArrow = new List<Transform>();
        [SerializeField] private List<Transform> lsContainSpawnEnemyArrow = new List<Transform>();

        [SerializeField] private List<Transform> lsTransTargetArrows = new List<Transform>();
        [SerializeField] private List<Transform> lsPositionSpawnArrows = new List<Transform>();

        public GameObject goGameContent;
        
        [Header("---Animator---")] 
        [SerializeField] private CharacterDataBinding boyDataBinding;
        [SerializeField] private CharacterDataBinding girlDataBinding; 
        [SerializeField] private CharacterDataBinding bossDataBinding;
        private CharacterDataBinding enemyDataBinding;
        
        [Header("---Data---")] 
        private List<ArrowDataItem> lsArrowDataItems = new List<ArrowDataItem>();

        [Header("---Variables ---")] 
        private float prevTimeArrow = 0;
        private float distanceMoveArrow = 0;
        private int curIndexArrow = 0;

        private float timeMoveArrow;

        [SerializeField, Range(1, 3)] private float defaultTimeMoveArrow = 1.8f;

        public GameState gameState;

        private float timerSong;
        public float TimerSong
        {
            get => timerSong;
            set
            {
                timerSong = value;
                if (uiGameplay != null)
                {
                    uiGameplay.UpdateTimerText(timerSong);
                }
            }
        }
        private float deltaTime;
        
        public string nameSong;

        private int miss;
        public int Miss
        {
            get => miss;
            set
            {
                miss = value;
                if (uiGameplay != null)
                {
                    uiGameplay.UpdateMissText(miss);
                }
            }
        }

        private int score;

        public int Score
        {
            get => score;
            set
            {
                score = value;
                if (uiGameplay != null)
                {
                    uiGameplay.UpdateScoreText(score);
                }
            }
        }

        private int indexSongOfWeek;
        private int indexWeek;
        private int indexMode;
        
        private Difficult difficult;
        
        private ConfigWeekData configWeekData;
        private ConfigSongData configSongData;
        private GameplaySongData gameplaySongData;
        
        public List<TargetArrow> lsTargetArrows = new List<TargetArrow>();

        private void Awake()
        {
            Input.multiTouchEnabled = true;
            goGameContent.SetActive(false);
            GameSave = SaveManager.Instance.LoadSave();
            
            UIManager.Instance.Init(() =>
            {
                UIManager.Instance.ShowUI(UIIndex.UILoading);
            });
        }
        private IEnumerator Start()
        {
            //Get List Position Spawn Arrow
            lsPositionSpawnArrows = uiGameplay.GetListTransformSpawnArrow();
            //Get List Target Arrow
            lsTransTargetArrows = uiGameplay.GetListTargetArrow();
            
            yield return new WaitForSeconds(0.1f);
            //SetupGameplay(0, 1, 0, Difficult.Easy);
        }

        public void SetupGameplay(int indexMode, int indexWeek, int indexSong, Difficult difficult)
        {
            this.indexMode = indexMode;
            this.indexWeek = indexWeek;
            this.indexSongOfWeek = indexSong;
            this.difficult = difficult;
            
            curIndexArrow = 0;

            configWeekData = ConfigMode.ConfigWeekData(indexMode, indexWeek);
            configSongData = ConfigMode.ConfigSongData(indexMode, indexWeek, indexSongOfWeek);
            
            gameState = GameState.None;
            
            goGameContent.SetActive(true);
                
            GetSongGameplay(indexMode, configSongData.nameJson);
            SoundManager.Instance.PlaySoundBGM();
            float lengthSong = SoundManager.Instance.GetLengthBGM();
            Debug.Log("lengthSong: " + lengthSong);
            
            SetupGameplayUI(lengthSong);
            
            Huy_Core.GameAnalyticsManager.Instance.PlayStart(configSongData.nameSong);
        }

        public void SetupGameplayUI(float lengthSong)
        {
            prevTimeArrow = 0;
            distanceMoveArrow = uiGameplay.GetDistanceMoveArrow();
            timeMoveArrow = defaultTimeMoveArrow * 1;
            
            //Get data json
            RootItem rootItem =
                JsonConvert.DeserializeObject<RootItem>(Resources.Load<TextAsset>("Jsons/" + configSongData.nameJson + "-easy").text);
            SongDataItem songItem = rootItem.song;

            Debug.Log("json: " + rootItem.ToString());

            lsArrowDataItems.Clear();
            for (int i = 0; i < songItem.notes.Count; i++)
            {
                for (int j = 0; j < songItem.notes[i].sectionNotes.Count; j++)
                {
                    ArrowDataItem arrowDataItem = new ArrowDataItem(songItem.notes[i].sectionNotes[j][0],
                        (int)(songItem.notes[i].sectionNotes[j][1] % 4), songItem.notes[i].sectionNotes[j][2],
                        songItem.notes[i].mustHitSection);
                    lsArrowDataItems.Add(arrowDataItem);
                }
            }

            lsArrowDataItems.Sort(SortByTimeAppear);

            timerSong = lengthSong;
            deltaTime = timeMoveArrow - 0.1f;
            
            SoundManager.Instance.StopSoundSFX(SoundFXIndex.SoundMenu);
            UIManager.Instance.ShowUI(UIIndex.UIGameplay,new GameplayParam()
            {
                difficult = difficult,
                maxValueSlider = 50,
                nameSong = configSongData.nameJson,
            });
            
            gameplaySongData = ConfigGameplay.ConfigSongData(indexMode, indexWeek, indexSongOfWeek);
            uiGameplay.SetSpriteIconBoss(gameplaySongData.spriteIconLose,gameplaySongData.spriteIcon);
            
            Miss = 0;
            Score = 0;
            
            SetupRuntimeAnimatorCharacter();
            SetupCharacter();
            
            uiGameplay.UpdateTimerText(timerSong);
            
            //gameState = GameState.Playing;
        }

        public void SetupCharacter()
        {
            if (configSongData.nameJson == "tutorial")
            {
                bossDataBinding.gameObject.SetActive(false);
                enemyDataBinding = girlDataBinding;
            }
            else
            {
                bossDataBinding.gameObject.SetActive(true);
                girlDataBinding.SetAnimationCharacter(0);
                bossDataBinding.GetComponent<Animator>().runtimeAnimatorController = gameplaySongData.enemyAnimator;
                enemyDataBinding = bossDataBinding;
            }
        }

        private void SetupRuntimeAnimatorCharacter()
        {
            int curSkinBoy = GameSave.CurrentIndexBoy;
            int curSkinGirl = GameSave.CurrentIndexGirl;

            ConfigSkinData configSkinBoyData = ConfigSkin.GetConfigSkinDataBoy(curSkinBoy);
            ConfigSkinData configSkinGirlData = ConfigSkin.GetConfigSkinDataGirl(curSkinGirl);
            
            boyDataBinding.SetAnimatorController(configSkinBoyData.skinAnimator);
            girlDataBinding.SetAnimatorController(configSkinGirlData.skinAnimator);
        }

        private void Update()
        {
            if (gameState == GameState.Playing && timerSong >= 0)
            {
                ShowTimerSong();
                if (indexMode == 0 && indexWeek == 0 && indexSongOfWeek == 0)
                {
                    LoadNoteNew(SoundManager.Instance.GetCurrentTimeSoundBGM() + deltaTime);
                }
                else
                {
                    CalculateCreateArrow(SoundManager.Instance.GetCurrentTimeSoundBGM() + deltaTime);
                }
            }
        }

        public void ShowTimerSong()
        {
            TimerSong -= Time.deltaTime;
            if (TimerSong <= 0)
            {
                //Show timer text
                uiGameplay.UpdateTimerText(0);
                //Check win/lose
                if (uiGameplay.CheckGameWin())
                {
                    ShowGameWin();
                }
                else
                {
                    goGameContent.SetActive(false);
                    ShowGameLose();
                }
            }
        }

        private int SortByTimeAppear(ArrowDataItem obj1, ArrowDataItem obj2)
        {
            return obj1.timeAppear.CompareTo(obj2.timeAppear);
        }

        public void LoadNoteNew(float time)
        {
            if (curIndexArrow == lsArrowDataItems.Count - 1)
            {
                /*if (lsArrowDataItems[curIndexArrow - 1].timeAppear > time * 1000)
                {
                    return;
                }
                else*/
                {
                    if (((lsArrowDataItems[curIndexArrow].timeAppear / 1000) - time) < -0.001f &&
                        ((lsArrowDataItems[curIndexArrow].timeAppear / 1000) - time) >= -0.15f)
                    {
                        //Create arrow
                        CreateArrow();
                        return;
                    }
                }
            }
            else
            {
                if (curIndexArrow < lsArrowDataItems.Count - 1)
                {
                    if (lsArrowDataItems[curIndexArrow].timeAppear == 0 ||
                        lsArrowDataItems[curIndexArrow].timeAppear < 1000)
                    {
                        //Create arrow
                        CreateArrow();
                        return;
                    }
                    else
                    {
                        if (lsArrowDataItems[curIndexArrow].timeAppear > time * 1000)
                        {
                            return;
                        }
                        else
                        {
                            if (lsArrowDataItems[curIndexArrow + 1].timeAppear > time * 1000)
                            {
                                //Create arrow
                                CreateArrow();
                            }
                        }
                    }
                }
            }
        }
        
        private void CalculateCreateArrow(float time)
        {
            if (curIndexArrow >= lsArrowDataItems.Count)
            {
                return;
            }

            if ((lsArrowDataItems[curIndexArrow].timeAppear / 1000) < time && (time - prevTimeArrow > 0.1f))
            {
                Debug.Log("arrow: " + curIndexArrow + " " + lsArrowDataItems[curIndexArrow].timeAppear + " " + lsArrowDataItems.Count);
                CreateArrow();
                prevTimeArrow = time;
            }
        }

        private void CreateArrow()
        {
            if (lsArrowDataItems[curIndexArrow] != null)
            {
                int indexArrowClone = lsArrowDataItems[curIndexArrow].indexArrow;
                int sumArrow = lsArrowDataItems.Count;
                if (lsArrowDataItems[curIndexArrow].mustHit)
                {
                    //Create arrow from list prefab
                    GameObject goArrow = Instantiate(lsPrefabArrows[indexArrowClone],
                        lsContainSpawnArrow[indexArrowClone]);
                    goArrow.transform.localPosition = lsPositionSpawnArrows[indexArrowClone].position;
                    Arrow arrowMove = goArrow.GetComponent<Arrow>();
                    //Setup arrow
                    arrowMove.SetupArrow(timeMoveArrow, lsArrowDataItems[curIndexArrow].timeTail / 1000,
                        lsArrowDataItems[curIndexArrow].indexArrow, lsArrowDataItems[curIndexArrow].mustHit,
                        distanceMoveArrow, curIndexArrow, sumArrow);
                }
                else
                {
                    //Create arrow from list prefab
                    GameObject goArrow = Instantiate(lsPrefabArrows[indexArrowClone],
                        lsContainSpawnEnemyArrow[indexArrowClone]);
                    goArrow.transform.localPosition = lsPositionSpawnArrows[indexArrowClone].position;
                    Arrow arrowMove = goArrow.GetComponent<Arrow>();
                    //Setup arrow
                    arrowMove.SetupArrow(timeMoveArrow, lsArrowDataItems[curIndexArrow].timeTail / 1000,
                        lsArrowDataItems[curIndexArrow].indexArrow, lsArrowDataItems[curIndexArrow].mustHit,
                        distanceMoveArrow, curIndexArrow, sumArrow);
                }
            }

            curIndexArrow++;
        }

        private void GetSongGameplay(int indexMod, string nameSong)
        {
            if (indexMod == 0)
            {
                AudioClip songAudioClip = Resources.Load("Sounds/Inst-" + nameSong) as AudioClip;
                SoundManager.Instance.AddSoundBGM(songAudioClip);
            }
            else
            {
                // Get From Asset Bundle
            }
        }

        public void OnButtonClickDown(int index)
        {
            lsTargetArrows[index].IsPress = true;
        }

        public void OnButtonClickUp(int index)
        {
            lsTargetArrows[index].IsPress = false;
            for (int i = 0; i < lsContainSpawnArrow[index].childCount; i++)
            {
                lsContainSpawnArrow[index].GetChild(i).GetComponent<Arrow>().IsPress = false;
            }
                
        }

        public void SetAnimationBoy(float index, float timeLoop = 0)
        {
            boyDataBinding.SetAnimationCharacter(index);
            if (timeLoop == 0)
            {
                timeLoop = 0.5f;
            }

            float speedMove = distanceMoveArrow / timeMoveArrow;
            float newTimeMove = (timeLoop * 10) / speedMove;
            CancelInvoke("DelayFinishAnimBoy");
            Invoke("DelayFinishAnimBoy", newTimeMove);
        }

        public void DelayFinishAnimBoy()
        {
            boyDataBinding.SetAnimationCharacter(0);
        }

        public void SetAnimationEnemy(float index, float timeLoop = 0)
        {
            if (enemyDataBinding == null || !enemyDataBinding.gameObject.activeSelf)
                return;
            
            enemyDataBinding.SetAnimationCharacter(index);
            if (timeLoop == 0)
            {
                timeLoop = 0.5f;
            }
            
            float speedMove = distanceMoveArrow / timeMoveArrow;
            float newTimeMove = (timeLoop * 10) / speedMove;
            
            CancelInvoke("DelayFinishAnimEnemy");
            Invoke("DelayFinishAnimEnemy", newTimeMove);
        }

        public void DelayFinishAnimEnemy()
        {
            enemyDataBinding.SetAnimationCharacter(0);
        }

        public void AddScore()
        {
            //Show Combo text correct
            Score += 100;
            uiGameplay.SetSliderHP(-1);
        }

        public void SubScore()
        {
            Miss++;
            uiGameplay.SetSliderHP(1);
        }
        public void ShowGameLose()
        {
            if (gameState != GameState.EndGame)
            {
                SoundManager.Instance.StopSoundBGM();
                //Clear all arrow
                ClearAllArrow();
                UIManager.Instance.HideUI(UIIndex.UIGameplay);
                gameState = GameState.EndGame;
                Huy_Core.GameAnalyticsManager.Instance.PlayEnd(configSongData.nameSong, "Lose", score, miss, timerSong);
                UIManager.Instance.ShowUI(UIIndex.UILose);
            }
        }

        public void ShowGameWin()
        {
            GameSave.ModeSaves[indexMode].WeekSaves[indexWeek].SongSaves[indexSongOfWeek].Score = score;
            gameState = GameState.EndGame;
            Huy_Core.GameAnalyticsManager.Instance.PlayEnd(configSongData.nameSong, "Win", score, miss, timerSong);
            UIManager.Instance.ShowUI(UIIndex.UIWin, new WinParam()
            {
                coinReward = 100
            });
        }

        public void RestartGame()
        {
            Time.timeScale = 1;
            gameState = GameState.EndGame;
            SetupGameplay(indexMode, indexWeek, indexSongOfWeek, difficult);
            SoundManager.Instance.StopSoundBGM();
            ClearAllArrow();
        }

        public void GoToHome()
        {
            Time.timeScale = 1;
            gameState = GameState.EndGame;
            UIManager.Instance.HideUI(UIIndex.UIGameplay);
            SoundManager.Instance.StopSoundBGM();
            //Hide Game Object Content
            goGameContent.SetActive(false);
            ClearAllArrow();
        }

        public void ResumeGame()
        {
            Time.timeScale = 1;
            gameState = GameState.Playing;
            SoundManager.Instance.ResumeSoundBGM();
        }

        private void ClearAllArrow()
        {
            for (int i = 0; i < lsContainSpawnArrow.Count; i++)
            {
                if (lsContainSpawnArrow[i].childCount > 0)
                {
                    for (int j = 0; j < lsContainSpawnArrow[i].childCount; j++)
                    {
                        lsContainSpawnArrow[i].GetChild(j).GetComponent<Arrow>().DestroySelf();
                    }
                }
            }

            for (int i = 0; i < lsContainSpawnEnemyArrow.Count; i++)
            {
                if (lsContainSpawnEnemyArrow[i].childCount > 0)
                {
                    for (int j = 0; j < lsContainSpawnEnemyArrow[i].childCount; j++)
                    {
                        lsContainSpawnEnemyArrow[i].GetChild(j).GetComponent<Arrow>().DestroySelf();
                    }
                }
            }
        }
    }
    
    [Serializable]
    public class ArrowDataItem
    {
        public float timeAppear;
        public int indexArrow;
        public float timeTail;
        public bool mustHit;

        public ArrowDataItem(float timeAppear, int indexArrow, float timeTail, bool mustHit)
        {
            this.timeAppear = timeAppear;
            this.indexArrow = indexArrow;
            this.timeTail = timeTail;
            this.mustHit = mustHit;
        }
    }

    [Serializable]
    public class NoteSongItem
    {
        public int lengthInStep;
        public bool mustHitSection;
        public List<float[]> sectionNotes = new List<float[]>();

        public NoteSongItem(int lengthInStep, bool mustHitSection, List<float[]> sectionNotes)
        {
            this.lengthInStep = lengthInStep;
            this.mustHitSection = mustHitSection;
            this.sectionNotes = sectionNotes;
        }
    }

    [Serializable]
    public class SongDataItem
    {
        public List<NoteSongItem> notes = new List<NoteSongItem>();
    }

    [Serializable]
    public class RootItem
    {
        public SongDataItem song;
    }

    public enum GameState
    {
        None = 0,
        PauseGame = 1,
        Playing = 2,
        Ready = 3,
        EndGame = 4
    }

    public enum Difficult
    {
        Easy = 0,
        Normal = 1,
        Hard = 2
    }
}



