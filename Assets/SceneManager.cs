using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SceneManager : MonoBehaviour
{
    public Text GameOverText;
    public Text GameClearText;
    // Start is called before the first frame update
    void Start()
    {
        GameOverText.gameObject.SetActive(false);
        GameClearText.gameObject.SetActive(false);
    }

    public void ShowGameOver()
    {
        GameOverText.gameObject.SetActive(true);
    }

    public void ShowGameClear()
    {
        GameClearText.gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
