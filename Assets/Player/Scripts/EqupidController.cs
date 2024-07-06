using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedController : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }

    private Weapon weapon;
    public GameObject[] prefab;

    private GameObject selectedWeapon;
    private int selectedWeaponIndex;

    private Vector2 equipedScale;
    /*public int selectedWeaponIdx 
    {  
        get {  return selectedWeaponIndex; } 
        set 
        {
            if (selectedWeaponIdx < prefab.Length)
                selectedWeaponIndex = selectedWeaponIdx;
            else
                Debug.Log("Error selectedWeaponIdx >= prefab.Length: " + selectedWeaponIdx);
        } 
    }*/

    void loadWeaponFromPrefab()
    {
        if (selectedWeapon != null)
        {
            DestroySelectedWeapon();
        }

        GameObject selectedPrefab = prefab[selectedWeaponIndex];

        Vector3 position = selectedPrefab.transform.position;
        Quaternion rotation = selectedPrefab.transform.rotation;
        Vector3 scale = selectedPrefab.transform.localScale;
        scale.y *= equipedScale.y;

        selectedWeapon = Instantiate(selectedPrefab, position, rotation);
        selectedWeapon.transform.localScale = scale;
        selectedWeapon.transform.parent = transform;

        weapon = selectedWeapon.GetComponentInChildren<Weapon>();
    }

    void DestroySelectedWeapon()
    {
        if (selectedWeapon != null)
        {
            Destroy(selectedWeapon);
            selectedWeapon = null; 
        }
        weapon = null;
    }

    private void Awake()
    {
        loadWeaponFromPrefab();
    }

    void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        weapon.direction = direction;

        transform.right = direction;

        equipedScale = transform.localScale;
        if (direction.x < 0 ) 
        {
            equipedScale.y = -1;
        }
        else
        if (direction.x > 0 ) 
        {
            equipedScale.y = 1;
        }
        transform.localScale = equipedScale;

        if (Input.GetMouseButtonDown(0))
            weapon.Attack();

        if (Input.GetMouseButtonDown(1))
        {
            selectedWeaponIndex++;
            if (selectedWeaponIndex >= prefab.Length)
                selectedWeaponIndex = 0;

            loadWeaponFromPrefab();
        }
    }
}
