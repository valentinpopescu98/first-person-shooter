using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaxAmmoText : MonoBehaviour
{
    public Text maxAmmoText;
    public GameObject gun;
    private int maxAmmo;

    // Update is called once per frame
    void Update()
    {
        maxAmmo = gun.GetComponent<Gun>().maxAmmo;
        maxAmmoText.text = maxAmmo.ToString();
    }
}
