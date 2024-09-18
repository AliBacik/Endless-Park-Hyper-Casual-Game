using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnParking : MonoBehaviour
{
    public GameObject Parking;

    public void ParkingAktiflestir()
    {
        Parking.SetActive(true);
    }
}
