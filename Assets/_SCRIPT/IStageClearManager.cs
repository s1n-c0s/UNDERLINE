using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IStageClearManager : MonoBehaviour
{
    private bool isGameClear;
    public GameObject StateClearPanel;
    
    private EnemyDetectorArea _enemyDetectorArea;
    public IPauseMenu pauseMenuScript;
    
    
    // Start is called before the first frame update
    void Start()
    {
        isGameClear = false;
        StateClearPanel.SetActive(false);
        pauseMenuScript = gameObject.GetComponent<IPauseMenu>();
        _enemyDetectorArea = GameObject.FindGameObjectWithTag("EnemyDetector").GetComponent<EnemyDetectorArea>();
    }
    
    void Update()
    {
        if (!isGameClear && _enemyDetectorArea.GetCurrentEnemy() == 0)
        {
            GameClear();
        }
    }

    /*
    private bool isGameClear
    {
        get { return isGameClear; }
    }
    */

    public void GameClear()
    {
        isGameClear = true;
        pauseMenuScript.SetTimeScaleAndPause(true);
        StateClearPanel.SetActive(gameObject);
    }
}
