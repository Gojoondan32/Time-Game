using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DamagePopupManager : MonoBehaviour
{
    public static DamagePopupManager i;

    private void Awake()
    {
        i = this;
    }

    public Transform damagePopup;

}
