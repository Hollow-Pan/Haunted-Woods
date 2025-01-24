using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveInventory : Singleton<ActiveInventory>{

    private int activeSlotIndexNum = 0;
    private int totalSlots;
    private PlayerControls playerControls;

    protected override void Awake(){
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void Start(){
        totalSlots = this.transform.childCount;

        playerControls.Inventory.Keyboard.performed += ctx => ToggleActiveSlotKeyboard((int)ctx.ReadValue<float>());
        playerControls.Inventory.MouseScroll.performed += ctx => ToggleActiveSlotMouse((int)ctx.ReadValue<Vector2>().normalized.y);
    }

    private void OnEnable(){
        playerControls.Enable();
    }

    public void EquipStartingWeapon(){
        ToggleActiveHighlight(0);
    }

    private void ToggleActiveSlotKeyboard(int slotIndex){
        ToggleActiveHighlight(slotIndex - 1);
    }

    private void ToggleActiveSlotMouse(int scrollValue){
        if (scrollValue < 0){
            activeSlotIndexNum = (activeSlotIndexNum + 1) % totalSlots;
        }
        else if (scrollValue > 0){
            activeSlotIndexNum = (activeSlotIndexNum - 1 + totalSlots) % totalSlots;
        }

        ToggleActiveHighlight(activeSlotIndexNum);
    }

    private void ToggleActiveHighlight(int indexNum){
        activeSlotIndexNum = indexNum;

        foreach (Transform inventorySlot in this.transform){
            inventorySlot.GetChild(0).gameObject.SetActive(false);
        }

        this.transform.GetChild(indexNum).GetChild(0).gameObject.SetActive(true);

        ChangeActiveWeapon();
    }

    private void ChangeActiveWeapon(){
        if (PlayerHealth.Instance.IsDead) {return;}

        if (ActiveWeapon.Instance.CurrentActiveWeapon != null){
            Destroy(ActiveWeapon.Instance.CurrentActiveWeapon.gameObject);
        }

        Transform childTransform = transform.GetChild(activeSlotIndexNum);
        InventorySlot inventorySlot = childTransform.GetComponent<InventorySlot>();
        WeaponInfo weaponInfo = inventorySlot.GetWeaponInfo();

        if (weaponInfo == null){
            ActiveWeapon.Instance.NullWeapon();
            return;
        }

        GameObject weaponToSpawn = weaponInfo.weaponPrefab;
        
        GameObject newWeapon = Instantiate(weaponToSpawn, ActiveWeapon.Instance.transform.position, Quaternion.identity);
        ActiveWeapon.Instance.transform.rotation = Quaternion.Euler(0, 0, 0);
        newWeapon.transform.parent = ActiveWeapon.Instance.transform;

        ActiveWeapon.Instance.NewWeapon(newWeapon.GetComponent<MonoBehaviour>());
    }

}
