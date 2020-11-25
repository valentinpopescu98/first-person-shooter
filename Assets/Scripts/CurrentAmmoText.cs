using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CurrentAmmoText : MonoBehaviour
{
    public Text currentAmmoText;
    public GameObject gun;
    private int currentAmmo;

    // Update is called once per frame
    void Update()
    {
        currentAmmo = gun.GetComponent<Gun>().currentAmmo;
        currentAmmoText.text = currentAmmo.ToString();
    }
}
