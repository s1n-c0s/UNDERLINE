using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class ActiveUI : MonoBehaviour
{
    public Text activeUI;
    //private bool isEnterZone = false;

    // Start is called before the first frame update
    void Start()
    {
        activeUI.gameObject.SetActive(false);
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //isEnterZone = true;
            activeUI.gameObject.SetActive(true);
            //UpdateScore();
        }
        else
        {
            //isEnterZone = false; 
            activeUI.gameObject.SetActive(false);

        }
    }
    // Update is called once per frame
    /*void Update()
    {
        if (isInZone = true)
        {
            activeUI.gameObject.SetActive(true);
        }
    }*/


}
