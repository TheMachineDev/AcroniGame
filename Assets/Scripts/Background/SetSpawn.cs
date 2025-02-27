﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetSpawn : MonoBehaviour
{
    private bool isSetted = false;

    public bool isFinalRespawn = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.name.Equals("Ada") && !isSetted)
        {
            isSetted = true;

            Player.getInstance().NextSpawn();

            if (isFinalRespawn)
                GameManager.GetInstance().EndLevel();
        } 
    }
}
