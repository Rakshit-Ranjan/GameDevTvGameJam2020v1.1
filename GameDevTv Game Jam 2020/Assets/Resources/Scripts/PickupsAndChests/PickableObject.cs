using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickableObject : MonoBehaviour {

    public Sprite sprite;

    public bool smallFlask;

    public bool bigFlask;

    public float increment;

    private void Start() {
        sprite = GetComponent<SpriteRenderer>().sprite;
    }

    public void Use() {
        if(bigFlask) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAttack>().currentWeapon.startTimeBtwAttack -= increment;
        } 
        if(smallFlask) {
            GameObject.FindGameObjectWithTag("Player").GetComponent<Health>().health += increment;
        }
    }
}
