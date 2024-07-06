using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EquipedController : MonoBehaviour
{
    public Vector2 PointerPosition { get; set; }

    private Weapon currentWeapon;
    public GameObject[] prefabWeapons;

    private GameObject selectedWeapon;
    private int currentWeaponIndex;

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

    void LoadWeaponFromPrefab()
    {
        if (selectedWeapon != null)
        {
            DestroySelectedWeapon();
        }

        GameObject selectedPrefab = prefabWeapons[currentWeaponIndex];

        Vector3 position = selectedPrefab.transform.position;
        Quaternion rotation = selectedPrefab.transform.rotation;
        Vector3 scale = selectedPrefab.transform.localScale;
        scale.y *= equipedScale.y;

        selectedWeapon = Instantiate(selectedPrefab, position, rotation);
        selectedWeapon.transform.localScale = scale;
        selectedWeapon.transform.parent = transform;

        currentWeapon = selectedWeapon.GetComponentInChildren<Weapon>();
    }

    void DestroySelectedWeapon()
    {
        if (selectedWeapon != null)
        {
            Destroy(selectedWeapon);
            selectedWeapon = null;
        }
        currentWeapon = null;
    }

    private void Awake()
    {
        LoadWeaponFromPrefab();
    }

    void Update()
    {
        Vector2 direction = (PointerPosition - (Vector2)transform.position).normalized;
        currentWeapon.direction = direction;

        transform.right = direction;

        equipedScale = transform.localScale;
        if (direction.x < 0)
        {
            equipedScale.y = -1;
        }
        else
        if (direction.x > 0)
        {
            equipedScale.y = 1;
        }
        transform.localScale = equipedScale;

        HandleInput();
    }
    private void HandleInput()
    {
        // Атака при нажатии левой кнопки мыши
        if (Input.GetMouseButtonDown(0) && currentWeapon != null)
        {
            currentWeapon.Attack();
        }

        // Смена оружия при нажатии правой кнопки мыши
        if (Input.GetMouseButtonDown(1))
        {
            currentWeaponIndex = (currentWeaponIndex + 1) % prefabWeapons.Length;
            LoadWeaponFromPrefab();
        }
    }
}