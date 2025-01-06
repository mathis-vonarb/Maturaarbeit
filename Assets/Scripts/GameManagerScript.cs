using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManagerScript : MonoBehaviour
{
    public GameObject Player;
    public BoolVariable playerCanAttack;
    
    public float playerAttackCooldown;
    bool resettingAttack;

    void Start()
    {
        playerCanAttack.value = true;
        resettingAttack = false;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().buildIndex == 1)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        if (!playerCanAttack.value && !resettingAttack)
        {
            resettingAttack = true;
            Invoke("ResetPlayerAttack", playerAttackCooldown);
        }
    }

    private void ResetPlayerAttack()
    {
        playerCanAttack.value = true;
        resettingAttack = false;
    }
}
