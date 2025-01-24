using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{

    protected override bool ShouldDestroyInScene(string sceneName)
    {
        return sceneName == "MainMenuScene";
    }

    public bool FacingLeft { get { return facingLeft; } }

    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float dashSpeed = 4f;
    [SerializeField] private float dashTime = 0.2f;
    [SerializeField] private float dashCD = 0.25f;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform weaponCollider;
    [SerializeField] private Transform slashAnimSpawnPoint;

    private PlayerControls playerControls;
    private Vector2 movement;
    private Rigidbody2D rb;
    private Animator myAnimator;
    private SpriteRenderer mySpriteRender;
    private Knockback knockback;

    private bool facingLeft = false;
    private bool isDashing = false;
    private float startingMoveSpeed;

    protected override void Awake() {
        base.Awake();

        playerControls = new PlayerControls();
        rb = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        mySpriteRender = GetComponent<SpriteRenderer>();
        knockback = GetComponent<Knockback>();
    }

    private void Start(){
        playerControls.Combat.Dash.performed += _ => Dash();
        
        startingMoveSpeed = moveSpeed;

        ActiveInventory.Instance.EquipStartingWeapon();
    }

    private void OnEnable() {
        playerControls.Enable();
    }

    private void OnDisable(){
        playerControls.Disable();
    }

    protected override void OnDestroy() {
        base.OnDestroy(); // Unsubscribe from scene events in Singleton<T>
        if (playerControls != null) {
            playerControls.Dispose();
        }
    }

    private void Update() {
        PlayerInput();
    }

    private void FixedUpdate() {
        AdjustPlayerFacingDirection();
        Move();
    }

    public Transform GetWeaponCollider(){
        return weaponCollider;
    }

    public Transform GetSlashAnimSpawnPoint(){
        return slashAnimSpawnPoint;
    }

    private void PlayerInput() {
        movement = playerControls.Movement.Move.ReadValue<Vector2>();

        myAnimator.SetFloat("moveX", movement.x);
        myAnimator.SetFloat("moveY", movement.y);
    }

    private void Move() {
        if(knockback.GettingKnockedBack || PlayerHealth.Instance.IsDead) {return;}
        rb.MovePosition(rb.position + movement * (moveSpeed * Time.fixedDeltaTime));
    }

    private void AdjustPlayerFacingDirection() {
        Vector3 mousePos = Input.mousePosition;
        Vector3 playerScreenPoint = Camera.main.WorldToScreenPoint(transform.position);

        if (mousePos.x < playerScreenPoint.x) {
            mySpriteRender.flipX = true;
            facingLeft = true;
        } else {
            mySpriteRender.flipX = false;
            facingLeft = false;
        }
    }

    private void Dash(){
        if (!isDashing && Stamina.Instance.CurrentStamina > 0){
            Stamina.Instance.UseStamina();
            isDashing = true;
            moveSpeed *= dashSpeed;
            tr.emitting = true;
            StartCoroutine(EndDashRoutine());
        }       
    }

    private IEnumerator EndDashRoutine(){
        yield return new WaitForSeconds(dashTime);
        moveSpeed = startingMoveSpeed;
        tr.emitting = false;
        yield return new WaitForSeconds(dashCD);
        isDashing = false;
    }

}
