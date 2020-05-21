using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StorageContainer : MonoBehaviour {

    public List<Slot> slots;

    public GameObject canvas;

    public Transform playerTransform;

    private void Start() {
        playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update() {
        if (Vector2.Distance(transform.position, playerTransform.position) <= 3) {
            if (Input.GetKeyDown(KeyCode.O)) {
                canvas.gameObject.SetActive(true);
            }
        } 
    }

    public void AddSlots(Slot slot) {
        for(int i = 0; i < slots.Count; i++) {
            if(slots[i].slotSprite == null) {
                slots[i].slotSprite = slot.slotSprite;
                slots[i].parentObject = slot.parentObject;
                break;
            }
        }
    }

}
