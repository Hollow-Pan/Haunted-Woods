using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWeapon : Singleton<ActiveWeapon>{

    public MonoBehaviour CurrentActiveWeapon {get; private set;}
    private PlayerControls playerControls;
    private bool attackButtonDown, isAttacking = false;
    private float weaponCD;


    protected override void Awake(){
        base.Awake();

        playerControls = new PlayerControls();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void Start()
    {
        playerControls.Combat.Attack.started += _ => StartAttacking();
        playerControls.Combat.Attack.canceled += _ => StopAttacking();

        AttackCD();
    }

    private void Update(){
        Attack();
    }

    public void NewWeapon(MonoBehaviour newWeapon){
        CurrentActiveWeapon = newWeapon;
        AttackCD();
        weaponCD = (CurrentActiveWeapon as IWeapon).GetWeaponInfo().weaponCooldown;
    }

    public void NullWeapon(){
        CurrentActiveWeapon = null;
    }

    private void AttackCD(){
        isAttacking = true;
        StopAllCoroutines();
        StartCoroutine(WeaponCDCoroutine());
    }

    private IEnumerator WeaponCDCoroutine(){
        yield return new WaitForSeconds(weaponCD);
        isAttacking = false;
    }

    private void StartAttacking() {
        attackButtonDown = true;
    }

    private void StopAttacking() {
        attackButtonDown = false;
    }

    private void Attack(){
        if (attackButtonDown && !isAttacking && CurrentActiveWeapon){
            AttackCD();

            (CurrentActiveWeapon as IWeapon).Attack();
        }        
    }

}
