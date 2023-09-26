/*using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{
    [Header("****Running Points****")]
    private PlayerController playerController; //ยืม component runsRemaining ให้คะแนนการยิงน้อบ แต่ไม่ใช้ละ
    public Text RunningScore;
    public int runScore;

    //public Text SecoundScore;
    //private int freeScore = 10;
    //private int langKoon;


    [Header("****Enemy Detector****")] // อาจจะไม่ใช้
    public int maxEnemies;
    private EnemyDetector enemyDetector;

    //public GameObject Pass;
    *//*[Header("****Secound Score****")]
    private EnemyDetector enemyDetector;
    private int allEnemiesinlevel;
    private int enemyWasDestroy = 0;
    private int secoundScore;
*//*


    void Start()
    {

        enemyDetector = GetComponent<EnemyDetector>();
        playerController = GetComponent<PlayerController>();

        runScore = playerController.runsRemaining;
        

        *//*langKoon = playerController.runsRemaining * 10; //ใช้คำนวนคะแนนการยิง
        firstScore = langKoon + freeScore;*//*

        // ให้ allEnemiesinlevel เท่ากับ detectedEnemyCount จาก EnemyDetector
        //allEnemiesinlevel = enemyDetector.detectedEnemyCount;

        //UpdateScore();   *******good***
    }

    void Update()
    {
        maxEnemies = enemyDetector.detectedEnemyCount;
        *//*int currentRunsRemaining = playerController.runsRemaining;

        if (currentRunsRemaining < langKoon)
        {
            langKoon = currentRunsRemaining * 10;
            firstScore = langKoon + freeScore;
            //UpdateScore();
        } จนถึงตรงนี้คือสมการคำนวนการยิง*//*

        if (maxEnemies != 0 )
        {
            
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
        }*//*

        // อัปเดตค่า enemyWasDestroy จาก EnemyDetector
        //enemyWasDestroy = enemyDetector.enemyWasDestroyed;

        // อัปเดตคะแนน



        


        UpdateScore();


    }

    void UpdateScore()
    {
        RunningScore.text = "Score: " + runScore;
        //SecoundScore.text = "Score: " + secoundScore;
    }
}


*/