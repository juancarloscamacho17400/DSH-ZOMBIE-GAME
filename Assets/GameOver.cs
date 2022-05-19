using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;


public class GameOver : MonoBehaviour
{
    public int rounds;
    public Text roundsText;

    public void Setup()
    {
        gameObject.SetActive(true);
        roundsText.text = "Has sobrevivido " + rounds.ToString() + " rondas";
    }

    public void RestartButton()
    {
        SceneManager.LoadScene(1);  // Again mansion map
    }
    public void QuitButton()
    {
        SceneManager.LoadScene(0);  // Main menu
    }
}
