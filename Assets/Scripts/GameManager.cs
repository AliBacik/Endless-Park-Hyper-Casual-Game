using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;

public class GameManager : MonoBehaviour
{
    [Header("----ARABA AYARLARI")]
    public GameObject[] Arabalar;
    public int AktifAracIndex;
    

    [Header("----CANVAS AYARLARI")]
    public TextMeshProUGUI LevelGostergeSayaci;
    public TextMeshProUGUI[] Textler;
    public GameObject[] Panellerim;
    public GameObject[] TapToButonlar;
 
    [Header("----PLATFORM AYARLARI")]
    public GameObject Platform_1;
    public GameObject Platform_2;
    public float[] DonusHizlari;
    public GameObject[] PlatformKulaklari;
    bool DonusVarmi;

    [Header("----LEVEL AYARLARI")] 
    public ParticleSystem PatlamaEfekti;
    public ParticleSystem ArabaGecisEfekti;
    public AudioSource[] Sesler;
    public int LevelSayaci;
    public int HighScore;
    bool DokunmaKilidi;
    public bool HizArtisiYapildi;
    public bool KulakKucultmeYapildi;
    public bool ArabaKonumlandimi;
    public int KacArabaParkEdildi;


    void Start()
    {
        KacArabaParkEdildi = 0;
        HighScore = PlayerPrefs.GetInt("HighScore");
        HizArtisiYapildi = false;
        KulakKucultmeYapildi = false;
        YeniArabaGetir();
        LevelSayaci = 1;
        AktifAracIndex = 0;
        DokunmaKilidi = true;
        DonusVarmi=true;
        VarsayilanDegerleriKontrolEt();
    }

    public void YeniArabaGetir()
    {
        KacArabaParkEdildi++;

        if (KacArabaParkEdildi % 5 == 0)
        {
            HizArtisiYapildi = false;
        }

        if (KacArabaParkEdildi % 2 == 0)
        {
            KulakKucultmeYapildi = false;
        }

        if (AktifAracIndex < Arabalar.Length)
        {

            Arabalar[AktifAracIndex].SetActive(true);
        }
        else if(AktifAracIndex==Arabalar.Length) 
        {
            AktifAracIndex = 0;
            Arabalar[AktifAracIndex].SetActive(true);


        }

    }

    void Update()
    {
        LevelGostergeSayaci.text = LevelSayaci.ToString();
        //if (Input.touchCount == 1)
        //{
        //    Touch dokunma = Input.GetTouch(0);
        //    if (dokunma.phase == TouchPhase.Began)
        //    {
        //        if (DokunmaKilidi)
        //        {
        //            Panellerim[0].SetActive(false);
        //            //Panellerim[3].SetActive(true);
        //            DokunmaKilidi = false;
        //        }
        //        else
        //        {
        //            Arabalar[AktifAracIndex].GetComponent<Araba>().ilerle = true;
        //            AktifAracIndex++;
        //        }
        //    }
        //}

        if (ArabaKonumlandimi&&Input.GetKeyDown(KeyCode.G))
        {
            ArabaKonumlandimi = false;
            Arabalar[AktifAracIndex].GetComponent<Araba>().ilerle = true;
            AktifAracIndex++;

        }

        else if (Input.GetKeyDown(KeyCode.H)) 
        {
            Panellerim[0].SetActive(false);
            DokunmaKilidi = false;
        }


        if (DonusVarmi)
        {
           Platform_1.transform.Rotate(new Vector3(0, 0, DonusHizlari[0]), Space.Self);
           if (Platform_2 != null)
           {
               Platform_2.transform.Rotate(new Vector3(0, 0, -DonusHizlari[1]), Space.Self);
           }
            
        }

        ///LEVEL ZORLUK AYARLARI///
        ///
        
        if (LevelSayaci % 5 == 0 && !HizArtisiYapildi)
        {
            DonusHizlari[0] = DonusHizlari[0]+0.2f;
            HizArtisiYapildi = true;

        }

        if(LevelSayaci % 2 == 0 && !KulakKucultmeYapildi)
        {
            if (PlatformKulaklari[0].transform.localScale.x > 0.3f)  // platform kulağı min sınırdan büyükse küçültmeye devam et.
            {
                KulakKucultmeYapildi = true;

                for (int i = 0; i < PlatformKulaklari.Length; i++)
                {
                    PlatformKulaklari[i].transform.localScale = new Vector3(PlatformKulaklari[i].transform.localScale.x - 0.01f,
                    PlatformKulaklari[i].transform.localScale.y, PlatformKulaklari[i].transform.localScale.z);

                }
            }

            
        }
        
    }


    public void Kaybettin()
    {
        if (LevelSayaci > HighScore)
        {
            PlayerPrefs.SetInt("HighScore", LevelSayaci);
            
        }
        DonusVarmi = false;      

        Textler[1].text = LevelSayaci.ToString();  
        Sesler[1].Play();
        Sesler[3].Play();
        Panellerim[1].SetActive(true);
        
        Textler[0].text = PlayerPrefs.GetInt("HighScore").ToString();
        Invoke("KaybettinButonuOrtayaCikar", 1.5f);

    }

    void KaybettinButonuOrtayaCikar()
    {
        TapToButonlar[0].SetActive(true);
    } 

    void KazandinButonuOrtayaCikar()
    {
        TapToButonlar[1].SetActive(true);
    }


    // BELLEK YONETIMI

    void VarsayilanDegerleriKontrolEt()
    {
        // PANELDEKI HIGH SCORE SAYISINI OYUN BASLADIGINDA YUKLE
        Textler[0].text = PlayerPrefs.GetInt("HighScore").ToString();
        Textler[1].text = PlayerPrefs.GetInt("Level").ToString();
    }


    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //DokunmaKilidi = true;
    }
}
