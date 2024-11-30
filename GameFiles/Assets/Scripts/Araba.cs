using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Araba : MonoBehaviour
{
    public bool ilerle;
    bool DurusNoktasiDurumu=false;
    public GameObject[] TekerIzleri;
    public Transform ArabaHavuzu;
    public GameManager _GameManager;
    public GameObject PatlamaPozisyonu;
    private Vector3 ArabaIlkPozisyon = new Vector3(0, 0, -3.15f);
    private Quaternion ArabaIlkRotasyon = Quaternion.Euler(0, 0, 0);

    void Start()
    {
        
    }

    
    void Update()
    {
        if (!DurusNoktasiDurumu)
        {
            transform.Translate(6f * Time.deltaTime * transform.forward);
            _GameManager.ArabaKonumlandimi = true;
        }

        if (ilerle)
        {
            transform.Translate(14f*Time.deltaTime*transform.forward);
        }

    }
    public void ArabaZamanSonraCagir()
    {
        _GameManager.YeniArabaGetir();
    }

    public void TekerIzleriniKaybet()
    {
        TekerIzleri[0].SetActive(false);
        TekerIzleri[1].SetActive(false);
    }
    public void TekerIzleriniGeriAc()
    {
        TekerIzleri[0].SetActive(true);
        TekerIzleri[1].SetActive(true);
    }

    public void ArabaGecisEfektiVeSesiOynat()
    {
        _GameManager.ArabaGecisEfekti.transform.position = PatlamaPozisyonu.transform.position;
        _GameManager.ArabaGecisEfekti.Play();
        _GameManager.Sesler[4].Play();
    }

    public void ArabaPatlamaEfektiOynat()
    {
        _GameManager.PatlamaEfekti.transform.position = PatlamaPozisyonu.transform.position;
        _GameManager.PatlamaEfekti.Play();
        _GameManager.Sesler[3].Play();
    }

    public void ArabaIlkPozisyonaAl()
    {
        gameObject.transform.localPosition = ArabaIlkPozisyon;
        gameObject.transform.rotation = ArabaIlkRotasyon;
    }
    private void OnCollisionEnter(Collision collision)
    {
        
        if (collision.gameObject.CompareTag("Parking"))
        {
            _GameManager.KacArabaParkEdildi++;
            _GameManager.LevelSayaci++;
            if (_GameManager.LevelSayaci % 10 == 0)
            {
                GameManager.KalanHak++;
                _GameManager.Textler[2].text = GameManager.KalanHak.ToString();
            }
            ilerle = false;
            gameObject.GetComponent<Rigidbody>().isKinematic = true;
            TekerIzleriniKaybet();
            ArabaGecisEfektiVeSesiOynat();
            ArabaIlkPozisyonaAl();
            gameObject.SetActive(false);
            gameObject.GetComponent<Rigidbody>().isKinematic = false;
            DurusNoktasiDurumu = false;
            TekerIzleriniGeriAc();
            Invoke("ArabaZamanSonraCagir", 1.0f);
        }

      
        else if (collision.gameObject.CompareTag("Araba"))
        {

            GameManager.KalanHak--;
            if (GameManager.KalanHak == 0)
            {
                ArabaPatlamaEfektiOynat();
                ilerle = false;
                TekerIzleriniKaybet();
                ArabaIlkPozisyonaAl();
                gameObject.SetActive(false);
                DurusNoktasiDurumu = false;
                TekerIzleriniGeriAc();
                _GameManager.Kaybettin();
            }
            else
            {
                ArabaPatlamaEfektiOynat();
                ilerle = false;
                TekerIzleriniKaybet();
                ArabaIlkPozisyonaAl();
                gameObject.SetActive(false);
                DurusNoktasiDurumu = false;
                TekerIzleriniGeriAc();
                Invoke("ArabaZamanSonraCagir", 1.0f);
            }
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("DurusNoktasi"))
        {
            DurusNoktasiDurumu = true;
            
        }

        else if (other.CompareTag("OrtaGobek"))
        {

            GameManager.KalanHak--;
            if (GameManager.KalanHak == 0)
            {
                ArabaPatlamaEfektiOynat();
                ilerle = false;
                TekerIzleriniKaybet();
                ArabaIlkPozisyonaAl();
                gameObject.SetActive(false);
                DurusNoktasiDurumu = false;
                TekerIzleriniGeriAc();
                _GameManager.Kaybettin();
            }
            else
            {
                ArabaPatlamaEfektiOynat();
                ilerle = false;
                TekerIzleriniKaybet();
                ArabaIlkPozisyonaAl();
                gameObject.SetActive(false);
                DurusNoktasiDurumu = false;
                TekerIzleriniGeriAc();
                Invoke("ArabaZamanSonraCagir", 1.0f);
            }
        }

    }
}
