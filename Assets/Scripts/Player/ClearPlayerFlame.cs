﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClearPlayerFlame : MonoBehaviour
{
    private bool active = true;
    
    void OnTriggerEnter2D(Collider2D col)
    {
        if (active && col.gameObject.tag == "Player" && (KillPlayer.hasBlueFlame || KillPlayer.hasRedFlame)) {
            GameObject currentFlame = null;
            Transform[] children = col.gameObject.GetComponentsInChildren<Transform>();
            foreach (Transform child in children) {
                if (child.tag == "Flame") {
                    currentFlame = child.gameObject;
                }
            }
            Destroy(currentFlame);
            KillPlayer.hasBlueFlame = false;  
            KillPlayer.hasRedFlame = false;
            active = false;  
        }
    }
}
