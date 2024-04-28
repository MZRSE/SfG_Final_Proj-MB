using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.InputSystem;

public class SwitchWeapons : MonoBehaviour
{
    public Vector2 _switchWeapon = Input.mouseScrollDelta;
    [SerializeField] public int activeWeapon = 0;

    void Start()
    {
        SelectWeapon();
    }

    // Update is called once per frame
    void Update()
    {
        int previousSelectedWeapon = activeWeapon;
        var scrollAxis = _switchWeapon.y;

        if (scrollAxis > 0f)
        {
            if (activeWeapon >= transform.childCount - 1)
            {
                activeWeapon = 0;
            }
            else
            {
                activeWeapon++;
            }
        }
        if (scrollAxis < 0f)
        {
            if (activeWeapon <= 0)
            {
                activeWeapon = transform.childCount - 1;
            }
            else
            {
                activeWeapon--;
            }
        }

        if (previousSelectedWeapon != activeWeapon)
        {
            SelectWeapon();
        }
    }

    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform _weapon in transform)
        {
            if (i == activeWeapon)
            {
                _weapon.gameObject.SetActive(true);
            } else {
                _weapon.gameObject.SetActive(false);
            }
            i++;
        }
    }

}
