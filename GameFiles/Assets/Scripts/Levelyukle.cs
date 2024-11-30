using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Levelyukle : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PlayerPrefs.HasKey("HighScore"))
        {
            PlayerPrefs.SetInt("HighScore", 0);
            PlayerPrefs.SetInt("Level", 1);
            PlayerPrefs.SetInt("GecisReklamSayisi", 0);
        }

        StartCoroutine(SahneyiZamanSonraYukle(2.0f));
    }
    IEnumerator SahneyiZamanSonraYukle(float delay)
    {
        
        yield return new WaitForSeconds(delay);

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }


}
