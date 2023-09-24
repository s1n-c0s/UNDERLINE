/*using UnityEngine;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("****First Score****")]
    private PlayerController playerController;
    public Text FirstScore;
    private int freeScore = 10;
    private int langKoon;
    private int firstScore;

    [Header("****First Score****")]
    public GameObject Pass;
    private bool hasPassed = false;

    *//*[Header("****Secound Score****")]
    private EnemyDetector enemyDetector; 
    public Text SecoundScore;
    private int allEnemiesinlevel;
    private int enemyWasDestroy = 0;
    private int secoundScore;*//*



    void Start()
    {
        playerController = GetComponent<PlayerController>();
        //enemyDetector = GetComponent<EnemyDetector>();

        langKoon = playerController.runsRemaining * 30;
        firstScore = langKoon + freeScore;

        // ให้ allEnemiesinlevel เท่ากับ detectedEnemyCount จาก EnemyDetector
        //allEnemiesinlevel = enemyDetector.detectedEnemyCount;

        UpdateScore();
    }

    void Update()
    {
        int currentRunsRemaining = playerController.runsRemaining;

        if (currentRunsRemaining < langKoon)
        {
            langKoon = currentRunsRemaining * 30;
            firstScore = langKoon + freeScore;
            UpdateScore();
        }

       

        // เช็คเงื่อนไขเพื่อคำนวณค่า secoundScore โดยใช้ค่าจาก enemyWasDestroy และ allEnemiesinlevel
        *//*if (allEnemiesinlevel > 0)
        {
            if (enemyWasDestroy < allEnemiesinlevel / 2)
            {
                secoundScore = 1;
            }
            else if (enemyWasDestroy == allEnemiesinlevel / 2)
            {
                secoundScore = 31;
            }
            else if (enemyWasDestroy > allEnemiesinlevel / 2)
            {
                secoundScore = 60;
            }
        }

        // อัปเดตค่า enemyWasDestroy จาก EnemyDetector
        //enemyWasDestroy = enemyDetector.enemyWasDestroyed;

        // อัปเดตคะแนน
        UpdateScore();*//*


    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasEnteredCollider)
        {
            hasEnteredCollider = true;
            UpdateScore();
        }
    }


    void UpdateScore()
    {
        FirstScore.text = "Score: " + firstScore;
        //SecoundScore.text = "Score: " + secoundScore;
    }
}*//*



using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("****First Score****")]
    private PlayerController playerController;
    public Text FirstScore;
    //public Text SecoundScore;
    private int freeScore = 10;
    private int langKoon;
    public int firstScore;

    [Header("****Enemy Detector****")]
    public GameObject Pass;
    private EnemyDetector enemyDetector;

    *//*[Header("****Secound Score****")]
    private EnemyDetector enemyDetector;
    private int allEnemiesinlevel;
    private int enemyWasDestroy = 0;
    private int secoundScore;*//*



    void Start()
    {

        enemyDetector = GetComponent<EnemyDetector>();
        playerController = GetComponent<PlayerController>();
        //enemyDetector = GetComponent<EnemyDetector>();

        langKoon = playerController.runsRemaining * 10;
        firstScore = langKoon + freeScore;

        // ให้ allEnemiesinlevel เท่ากับ detectedEnemyCount จาก EnemyDetector
        //allEnemiesinlevel = enemyDetector.detectedEnemyCount;

        //UpdateScore();   *******good***
    }

    void Update()
    {
        int currentRunsRemaining = playerController.runsRemaining;

        if (currentRunsRemaining < langKoon)
        {
            langKoon = currentRunsRemaining * 10;
            firstScore = langKoon + freeScore;
            //UpdateScore();
        }

        // เช็คเงื่อนไขเพื่อคำนวณค่า secoundScore โดยใช้ค่าจาก enemyWasDestroy และ allEnemiesinlevel
        *//*if (allEnemiesinlevel > 0)
        {
            if (enemyWasDestroy < allEnemiesinlevel / 2)
            {
                secoundScore = 1;
            }
            else if (enemyWasDestroy == allEnemiesinlevel / 2)
            {
                secoundScore = 31;
            }
            else if (enemyWasDestroy > allEnemiesinlevel / 2)
            {
                secoundScore = 60;
            }
        }

        // อัปเดตค่า enemyWasDestroy จาก EnemyDetector
        //enemyWasDestroy = enemyDetector.enemyWasDestroyed;

        // อัปเดตคะแนน
        UpdateScore();*//*


    }

    *//*void UpdateScore()
    {
        FirstScore.text = "Score: " + firstScore;
        //SecoundScore.text = "Score: " + secoundScore;
    }*//*


}*/