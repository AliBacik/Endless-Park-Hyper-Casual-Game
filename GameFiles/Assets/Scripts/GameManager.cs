using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using System.Security.Cryptography;
using GoogleMobileAds.Api;
using EndlessPark;
using Unity.VisualScripting;
using System;

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
    public static int KalanHak;

    [Header("----REKLAM AYARLARI")]
    ReklamYonetimi _ReklamYonetimi=new ReklamYonetimi();
    public bool ReklamGosterildimi;

    private int dokunmaSayisi = 0;
    public static bool ReklamAtkifMi;
    void Start()
    {
        ReklamGosterildimi = false;
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
        KalanHak = 1;
        _ReklamYonetimi.RequestRewardedAd();
        

        //_ReklamYonetimi.RequestInterstitial();
        //_ReklamYonetimi.GecisReklamiGoster();
    }

    public void YeniArabaGetir()
    {

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
        Textler[2].text = KalanHak.ToString();
        LevelGostergeSayaci.text = LevelSayaci.ToString();

        if (Input.touchCount == 1&& ReklamAtkifMi==false)
        {
            Touch dokunma = Input.GetTouch(0);

            if (dokunma.phase == TouchPhase.Began)
            {
                dokunmaSayisi++;
             
                if (dokunmaSayisi == 1 && DokunmaKilidi)
                {
                    Panellerim[0].SetActive(false);
                    DokunmaKilidi = false;
                }
                else if (ArabaKonumlandimi&&DokunmaKilidi==false&&dokunmaSayisi!=1)
                {
                    ArabaKonumlandimi = false;
                    Arabalar[AktifAracIndex].GetComponent<Araba>().ilerle = true;
                    AktifAracIndex++;
                }
            }
        }

        //if (ArabaKonumlandimi&&Input.GetKeyDown(KeyCode.G))
        //{
        //    ArabaKonumlandimi = false;
        //    Arabalar[AktifAracIndex].GetComponent<Araba>().ilerle = true;
        //    AktifAracIndex++;

        //}

        //else if (Input.GetKeyDown(KeyCode.H)) 
        //{
        //    Panellerim[0].SetActive(false);
        //    DokunmaKilidi = false;
        //}


        if (DonusVarmi)
        {
            
            Platform_1.transform.Rotate(new Vector3(0, 0, DonusHizlari[0] * Time.deltaTime), Space.Self);

            if (Platform_2 != null)
            {
                
                Platform_2.transform.Rotate(new Vector3(0, 0, -DonusHizlari[1] * Time.deltaTime), Space.Self);
            }
        }

        ///LEVEL ZORLUK AYARLARI///
        ///

        if (LevelSayaci % 5 == 0 && !HizArtisiYapildi)
        {
            DonusHizlari[0] = DonusHizlari[0]+7.5f;
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

        ///LEVEL REKLAM AYARLARI///
        ///
        

        if (LevelSayaci % 15 == 0 && ReklamGosterildimi==false)
        {
            _ReklamYonetimi.RequestInterstitial();
            _ReklamYonetimi.GecisReklamiGoster();
            ReklamGosterildimi = true;
            return;

        }

        if (LevelSayaci % 15 != 0)
        {
            ReklamGosterildimi = false; 
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

    // ODULLU REKLAM GOSTER BUTONU
    public void OdulluReklamGosterButonu()
    {
        _ReklamYonetimi.OdulluReklamGoster();
        YeniArabaGetir();
        Panellerim[1].SetActive(false);
        DonusVarmi = true;
    }

  
}

namespace EndlessPark
{
    public class ReklamYonetimi
    {
        private InterstitialAd interstitial;
        private RewardedAd _RewardedAd;

        public void RequestInterstitial()
        {
            string AdUnitId;
#if UNITY_ANDROID  
            AdUnitId = "ca-app-pub-8912958133819768/4809156976";
            //AdUnitId = "ca-app-pub-3940256099942544/1033173712"; test
#elif UNITY_IPHONE
            AdUnitId = "ca-app-pub-3940256099942544/4411468910";
#else
            AdUnitId = "unexpected_platform";F
#endif
            InterstitialAd.Load(AdUnitId, new AdRequest(), (ad, _) => interstitial = ad);

            interstitial.OnAdFullScreenContentClosed += GecisReklamiKapatildi;
        }

        void GecisReklamiKapatildi()
        {
            interstitial.Destroy();
            RequestInterstitial();
            GameManager.ReklamAtkifMi = false;
        }

        public void GecisReklamiGoster()
        {
            GameManager.ReklamAtkifMi = true;

            if (interstitial != null && interstitial.CanShowAd())
                {
                    PlayerPrefs.SetInt("GecisReklamSayisi", 0);
                    interstitial.Show();
                }
            else
               {
                   interstitial.Destroy();
                    RequestInterstitial();
               }




                //if (PlayerPrefs.GetInt("GecisReklamSayisi") == 2)
                //{
                //    if (interstitial != null && interstitial.CanShowAd())
                //    {
                //        PlayerPrefs.SetInt("GecisReklamSayisi", 0);
                //        interstitial.Show();
                //    }
                //    else
                //    {
                //        interstitial.Destroy();
                //        RequestInterstitial();
                //    }
                //}
                //else
                //{
                //    PlayerPrefs.SetInt("GecisReklamSayisi", PlayerPrefs.GetInt("GecisReklamSayisi") + 1);
                //}


        }

        public void RequestRewardedAd()
        {
            string AdUnitId;
#if UNITY_ANDROID 
            AdUnitId = "ca-app-pub-8912958133819768/7569954206";
            //AdUnitId = "ca-app-pub-3940256099942544/5224354917"; test
#elif UNITY_IPHONE
            AdUnitId = "ca-app-pub-3940256099942544/1712485313";
#else
            AdUnitId = "unexpected_platform";F
#endif

            RewardedAd.Load(AdUnitId, new AdRequest(), (ad, _) => _RewardedAd = ad);
            _RewardedAd.OnAdFullScreenContentClosed += OdulluReklamKapatildi;
            
        }

        private void OdulluReklamKapatildi()
        {
            GameManager.ReklamAtkifMi = false;
            RequestRewardedAd();
        }

        public void OdulluReklamGoster()
        {
            GameManager.ReklamAtkifMi = true;

            if (_RewardedAd != null && _RewardedAd.CanShowAd())
            {

                _RewardedAd.Show(OnUserEarnedReward);
            }
            
        }
        private void OnUserEarnedReward(Reward reward)
        {
            GameManager.KalanHak++;    
        }
    }
}
