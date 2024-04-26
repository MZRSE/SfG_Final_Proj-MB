using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class UseShoot : MonoBehaviour
{
    [SerializeField] private PaintballGun _weapon1;
    //[SerializeField] private ColorStreamGun _weapon2;
    //[SerializeField] private SprayCan _weapon3;

    //private void Awake()
    //{
        //PaintballGun shootableObject = _weapon1.GetComponent<PaintballGun>();
    //}

    public void OnShoot(InputValue value)
    {
        if (value.isPressed)
        {
            //if paintball gun is active
            //_weapon1.PaintballShoot();
            //else if water gun is active
            //shoot water guh
            //else if spray can is active
            //use spray can


        }
    }

}