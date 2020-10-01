﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttuneRedFlame : MonoBehaviour
{
    public GameObject player;
    public GameObject flameParent;

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Player" && !KillPlayer.hasRedFlame) {
            if (KillPlayer.hasBlueFlame) {
                GameObject currentFlame = player.transform.GetChild(2).gameObject;
                currentFlame.transform.parent = flameParent.transform;
                currentFlame.transform.position = this.transform.position;
                StartCoroutine(waitToChangeFlameValue());
            }
            KillPlayer.hasRedFlame = true;
            this.transform.position = player.transform.position;
            this.transform.parent = player.transform;
        }
    }
    IEnumerator waitToChangeFlameValue() {
        yield return new WaitForSeconds(1.0f);
        KillPlayer.hasBlueFlame = false;
    }
}
