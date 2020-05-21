using UnityEngine;

public class WeaponHolder : MonoBehaviour {

    public EnemyWeapon weapon;

    public EnemyWeapon spawnedWeapon;

    public float size;

    public bool isBossOgre;

    public bool isBossZombie;

    private void Awake() {
        if (!isBossOgre && !isBossZombie)
            spawnedWeapon = Instantiate(weapon, GetComponentInParent<SlashEnemy>().placePoint.transform.position, Quaternion.identity);
        if(isBossOgre)
            spawnedWeapon = Instantiate(weapon, GetComponentInParent<BossOgre>().placePoint.transform.position, Quaternion.identity);
        if(isBossZombie)
            spawnedWeapon = Instantiate(weapon, GetComponentInParent<BossZombie>().placePoint.transform.position, Quaternion.identity);

        spawnedWeapon.transform.SetParent(transform);
    }

    private void Update() {
        spawnedWeapon.transform.localScale = new Vector3(size, size, size);
    }
}
