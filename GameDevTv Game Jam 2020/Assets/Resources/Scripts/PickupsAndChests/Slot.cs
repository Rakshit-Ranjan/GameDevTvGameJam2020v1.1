using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Slot : MonoBehaviour {

    Image image;

    public PickableObject parentObject;

    public Sprite slotSprite;

    private void Start() {
        image = this.GetComponent<Image>();
    }

    private void Update() {
        if (slotSprite == null) {
            image.color = Color.clear;
        } else {
            image.sprite = slotSprite;
            image.color = Color.white;
        }
    }

    public void Use() {
        if(parentObject != null) {
            parentObject.Use();
            FindObjectOfType<AudioManager>().Play("MouseClick");
            parentObject = null;
            slotSprite = null;
        }
    }

}