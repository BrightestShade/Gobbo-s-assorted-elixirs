using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quit : MonoBehaviour
{

 

    public void QuitGame()
    {
        // Quit the application in build
        Application.Quit();
        Debug.Log("Game Closed");
    }
}
