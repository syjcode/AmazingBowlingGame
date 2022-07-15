using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public UnityEvent onEvents;
    public static GameManager instance;
    public GameObject readyPannel;
    public Text scoreText;
    public Text bestScoreText;
    public Text messageText;
    public bool isRoundActive = false;
    private int score = 0;
    public ShooterRotator shooterRotator;
    public CamFollow cam;

    private void Awake() {
        instance = this;
        UpdateUI();

    }

    public void AddScore(int newScore){
        score +=newScore;
        UpdateBestScore();
        UpdateUI();
    }

    void UpdateBestScore(){
        if(GetBestScore()<score){
            PlayerPrefs.SetInt("BestScore",score);

        }
    }
    private int GetBestScore(){
        int bestScore = PlayerPrefs.GetInt("BestScore");
        return bestScore;

    }
    // Start is called before the first frame update
    void UpdateUI(){
        scoreText.text = "Score: " + score;
        bestScoreText.text = "Best Score: " + GetBestScore();
    }
   public void OnBallDestroy(){
    UpdateUI();
    isRoundActive=false;

   }

   public void Reset(){
    score=0;
    UpdateUI();
    StartCoroutine("RoundRoutine");

   }


   IEnumerator RoundRoutine(){
    onEvents.Invoke();
    readyPannel.SetActive(true);
    cam.SetTarget(shooterRotator.transform,CamFollow.State.Idle);
    shooterRotator.enabled=false;
    isRoundActive=false;
    messageText.text = "Ready...";

    yield return new WaitForSeconds(3f);
    
    isRoundActive=true;
    readyPannel.SetActive(false);
    shooterRotator.enabled = true;
    cam.SetTarget(shooterRotator.transform,CamFollow.State.Ready);
    
    while(isRoundActive){
        yield return null;
    }
    
    yield return new WaitForSeconds(3f);
    
    readyPannel.SetActive(true);
    shooterRotator.enabled=false;
    messageText.text = "Wait For Next Round...";
    yield return new WaitForSeconds(3f);
    Reset();
   }
    void Start()
    {
          StartCoroutine("RoundRoutine");

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
