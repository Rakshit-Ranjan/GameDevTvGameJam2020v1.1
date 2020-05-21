using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerPickup : MonoBehaviour {

    public Inventory inventory;

    public bool isTutorial;

    public StorageContainer storage;

    private Rigidbody2D rb;

    public GameObject InventoryCanvas;

    public GameObject StorageContainerCanvas;

    public GameObject MinimapCanvas;

    public bool minimapState = false;

    public float radius;

    private int delIndex = 0;

    private int addIndex = 0;

    public int startNJewel;

    public GameObject[] jewels;

    public int totalDeath;

    public int deaths;

    public int nJewel;

    int times = 1;

    private void Start() {
        rb = this.GetComponent<Rigidbody2D>();
        jewels = GameObject.FindGameObjectsWithTag("Jewel");
        startNJewel = jewels.Length;
    }

    private void Update() {

        ToggleInventory();

        ToggleMinimap();

        storage = GameObject.FindGameObjectWithTag("Storage").GetComponent<StorageContainer>();

        if(addIndex == 3) {
            addIndex = 0;
        }
        if (delIndex == 3) {
            delIndex = 0;
        }

        if (Input.GetKeyDown(KeyCode.E)) {
            CheckForItems();
        }

        if (Input.GetKeyDown(KeyCode.R)) {
            DeleteItem(inventory.slots[delIndex]);
        }

        if (Input.GetKeyDown(KeyCode.T)) {
            TransferItem(inventory.slots[addIndex]);
            addIndex++;
        }

        if (!isTutorial) {
            if (nJewel == startNJewel) {
                if (times > 0) {
                    UnityEngine.SceneManagement.SceneManager.LoadScene("WinScene");
                    times--;
                }
            }
        }
        
    }

    void CheckForItems() {
        Collider2D[] objects = Physics2D.OverlapCircleAll(rb.position, radius);
        foreach(Collider2D O in objects) {
            PickableObject pickable = O.GetComponent<PickableObject>();
            if(pickable != null) {
                print("Found a pickable object");
                for(int i = 0; i < inventory.slots.Count; i++) {
                    if(inventory.slots[i].slotSprite == null) {
                        inventory.slots[i].slotSprite = pickable.sprite;
                        inventory.slots[i].parentObject = pickable;
                        pickable.gameObject.SetActive(false);
                        FindObjectOfType<AudioManager>().Play("Pickup");
                        break;
                    }
                } 
            }
        }
    }

    public void DeleteItem(Slot slot) {
        if (slot.slotSprite != null) {
            slot.slotSprite = null;
            slot.parentObject = null;
            delIndex++;
            FindObjectOfType<AudioManager>().Play("Remove");
        } else {
            delIndex = delIndex;
        }
    }

    public void TransferItem(Slot slot) {
        if(slot.slotSprite != null) {
            storage.AddSlots(slot);
            slot.slotSprite = null;
            slot.parentObject = null;
            FindObjectOfType<AudioManager>().Play("Pickup");
        }
    }

    public void ToggleMinimap() {
        if(Input.GetKeyDown(KeyCode.M)) {
            MinimapCanvas.SetActive(minimapState);
            minimapState = !minimapState;
        }
    }

    public void ToggleInventory() {
        if(Input.GetKeyDown(KeyCode.I)) {
            InventoryCanvas.SetActive(true);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Chest chest = collision.gameObject.GetComponent<Chest>();
        if(chest != null) {
            chest.ReleaseTreasure();
            Destroy(chest.gameObject);
        }

        Jewel jewel = collision.gameObject.GetComponent<Jewel>();
        if(jewel != null) {
            Destroy(jewel.gameObject);
            nJewel++;
        }
    }

}
