using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    bool IsPaused => Time.timeScale < 0.00001f;

    private void Start()
    {
        Pause(); // Starts paused
    }

    void Pause()
    {
        Time.timeScale = 0;
    }
    void ResumeGame()
    {
        Time.timeScale = 1;
    }

    void Update()
    {
        Keyboard keyboard = Keyboard.current;
        if (keyboard.spaceKey.wasPressedThisFrame)
        {
            if (IsPaused)
            {
                ResumeGame();
            }
            else
            {
                Pause();
            }
        }

        if (keyboard.tKey.wasPressedThisFrame)
        {
            CollisionManager manager = FindObjectOfType<CollisionManager>();
            manager.CycleSpacePartition();
        }
    }
}
