﻿using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public string fase;
    static Player instance;
    static Movimento player_movement;
    static Inventory player_inventory;
    int player_currency = 500;

    public int[] spawnsDistribution;

    bool[] lifes = { true, true, true };

    private void Awake()
    {
        instance = this;
        player_movement = GetComponent<Movimento>();
        player_inventory = new Inventory();
    }

    public static Player getInstance()
    {
        return instance;
    }

    public Movimento GetPlayerMovement()
    {
        return player_movement;
    }

    public Inventory GetPlayerInventory()
    {
        return player_inventory;
    }

    public int GetPlayerCurrency()
    {
        return player_currency;
    }

    public GameObject[] spawns;
    private int index_spawn = 0;

    public void TeleportToSpawn()
    {
        transform.position = spawns[index_spawn].transform.position;
        FirebaseMethods.firebaseMethods.getFireBaseMethodsInstance().IncrementQttPlayed(fase);
    }

    public void ResetSpawn()
    {
        index_spawn = 0;
        TeleportToSpawn();
    }

    public void NextSpawn()
    {

        if (index_spawn < spawns.Length)
        {
            index_spawn++;
            if (index_spawn == (spawns.Length - 1))
            {
                //Final
                AudioManager.GetInstance().Play("bgm-fim-da-fase");
                ChangeColorScript.getInstance().Animate("black");
                LevelCompletedMenu.GetInstance().Open();
            }
        }
        else
            Debug.LogError("Tá faltando spawns na lista do player!");
    }

    public int RemoveFromPlayerCurrency(int amount)
    {
        if (player_currency >= amount)
            player_currency -= amount;

        return player_currency;
    }

    public int AddPlayerCurrency(int amount)
    {
        player_currency += amount;

        return player_currency;
    }

    internal Collider2D[] GetColliders()
    {
        return GetComponents<Collider2D>();
    }

    public void TakeAHeart()
    {
        int lifeCount = 3;
        for(int i = 2; i >= 0; i--)
        {
            if(lifes[i])
            {
                lifes[i] = false;
                lifeCount = i;
                AudioManager.GetInstance().Play("sfx-dano");
                break;
            }
        }

        if (!lifes[0])
        {
            AudioManager.GetInstance().Play("bgm-morte");
            DeathMenu.GetInstance().Open();
            return;
        }

        LevelCompletedMenu.GetInstance().SetLifeText(lifeCount);
        FindObjectOfType<LifeManager>().SetHearts(lifes);
    }
}