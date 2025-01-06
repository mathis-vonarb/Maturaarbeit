using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    public BoolVariable isDead;

    void Update()
    {
        if (isDead.value && (SceneManager.GetActiveScene().buildIndex != 2))
        {
            SceneManager.LoadScene(2, LoadSceneMode.Single);
            
        }
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        SceneManager.LoadScene(1, LoadSceneMode.Single);
    }

    public void Quit()
    {
        SceneManager.LoadScene(0, LoadSceneMode.Single);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
