using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour {

    public List<GameObject> collections;

    public int price;

    public int nItems;

    public float offset;

    public void ReleaseTreasure() {
        if(collections.Count <= 1) {
            Instantiate(collections[0], transform.position, Quaternion.identity);
        } else {
            for(int i = 0; i < collections.Count; i++) {
                while(nItems > 0) {
                    float x = Random.Range(transform.position.x - offset, transform.position.x + offset);
                    float y = Random.Range(transform.position.y - offset, transform.position.y + offset );
                    Vector2 dropPoint = new Vector2(x, y);
                    Instantiate(collections[Random.Range(0, collections.Count)], dropPoint, Quaternion.identity);
                    nItems--;
                }
                
            }
        }
    }

}
