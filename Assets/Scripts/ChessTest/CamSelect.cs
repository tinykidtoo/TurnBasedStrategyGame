﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSelect : MonoBehaviour {
    [SerializeField] LayerMask selectable;
    Camera m_Cam;
    [SerializeField] GameObject currentSelected;
    string teamSelect = "Team0";
    public bool Network = false;

    void Start() {
        m_Cam = gameObject.GetComponent<Camera>();  //Setup the main camera reference for raycasting
    }

    public void SetTeamSelect(string team) {
        teamSelect = team;
    }
    public string GetTeamSelect(string team) {
        return teamSelect;
    }

    void SelectRay() {
        Ray myRay = m_Cam.ScreenPointToRay(Input.mousePosition);  //Create a ray from the main camera using the mouse pointer
        RaycastHit hit;
        if (Physics.Raycast(myRay, out hit, selectable)) {
            if (hit.collider.CompareTag(teamSelect)) {
                if (currentSelected) {
                    if (Network)
                        SendMsg(currentSelected, "ClearDisplayOptions");
                    else
                        currentSelected.SendMessage("ClearDisplayOptions");
                    currentSelected.SendMessage("ClearDisplayOptions");
                }
                currentSelected = hit.collider.gameObject;
                SendMsg(hit.collider.gameObject, "DisplayOptions");
                //hit.collider.gameObject.SendMessage("DisplayOptions");
            }
            else if (hit.collider.CompareTag("Grid") && currentSelected != null) {
                SendMsg(currentSelected, "MoveTo", hit.collider.gameObject.transform.position);
                //currentSelected.SendMessage("MoveTo",  hit.collider.gameObject.transform.position);
                SendMsg(currentSelected, "ClearDisplayOptions");
                //currentSelected.SendMessage("ClearDisplayOptions");
            }
        }
    }

    //void CMD

    void SendMsg(GameObject obj, string Func, Vector3 pos) {
        if (!Network)
            obj.SendMessage(Func, pos);
        else
            Debug.Log("SendNetCmd");
    }
    void SendMsg(GameObject obj, string Func) {
        if (!Network)
            obj.SendMessage(Func);
        else
            Debug.Log("SendNetCmd");
    }

    // Update is called once per frame
    void Update() {

        if (Input.GetMouseButtonDown(0)) {
            SelectRay();
        }
        if (Input.GetMouseButtonDown(1) && currentSelected != null) {
            currentSelected.SendMessage("ClearDisplayOptions");
        }
    }
}
