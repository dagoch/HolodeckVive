using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FRL.IO;
using System;

public class SceneChanger : MonoBehaviour, IGlobalApplicationMenuPressDownHandler {

    public void OnGlobalApplicationMenuPressDown(VREventData eventData)
    {
        index = (index + 1) % 4;
        changemodel();
    }

    public GameObject Stage;
    public GameObject Lab;
    public GameObject LaScala;
    public GameObject PlayArea;

    [SerializeField] private int index;

    private void changemodel()
    {
        if(index == 0)
        {
            Stage.SetActive(false);
            Lab.SetActive(true);
            LaScala.SetActive(false);
            PlayArea.SetActive(false);
        }

        if (index == 1)
        {
            Stage.SetActive(false);
            Lab.SetActive(true);
            LaScala.SetActive(false);
            PlayArea.SetActive(true);
        }

        if (index == 2)
        {
            Stage.SetActive(true);
            Lab.SetActive(true);
            LaScala.SetActive(false);
            PlayArea.SetActive(false);
        }
        if (index == 3)
        {
            Stage.SetActive(false);
            Lab.SetActive(false);
            LaScala.SetActive(true);
            PlayArea.SetActive(false);
        }
    }
    // Use this for initialization
    void Start () {
        index = 0;
        changemodel();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
