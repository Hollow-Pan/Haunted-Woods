using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    private enum State {
        Roaming,
        Attacking,
    }

    [SerializeField] private float roamingChangeDirectionTime = 2f;
    [SerializeField] private float attackRange = 0f;
    [SerializeField] private float attackCooldown = 2f;
    [SerializeField] private MonoBehaviour enemyType;
    [SerializeField] private bool stopMotionWhileAttacking = false;


    private State state;
    private EnemyPathfinding enemyPathfinding;
    private Vector2 roamingPosition;
    private float timeRoaming = 0f;
    private bool canAttack = true;


    private void Awake() {
        enemyPathfinding = GetComponent<EnemyPathfinding>();
        state = State.Roaming;
    }

    private void Start() {
        roamingPosition = GetRoamingPosition();
    }

    private void Update() {
        MovementStateControl();
    }

    private void MovementStateControl(){
        switch (state){
            default:

            case State.Roaming:
            Roaming();
            break;

            case State.Attacking:
            Attacking();
            break;
        }
    }

    private void Roaming(){
        timeRoaming += Time.deltaTime;
        enemyPathfinding.MoveTo(roamingPosition);
        
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) < attackRange){
            state = State.Attacking;
        }

        if (timeRoaming > roamingChangeDirectionTime){
            roamingPosition = GetRoamingPosition();
        }
    }

    private void Attacking(){
        if (Vector2.Distance(transform.position, PlayerController.Instance.transform.position) > attackRange){
            state = State.Roaming;
        }

        if (attackRange != 0 && canAttack){
            canAttack = false;
            (enemyType as IEnemy).Attack();

            if (stopMotionWhileAttacking){
                enemyPathfinding.StopMoving();
            }
            else{
                enemyPathfinding.MoveTo(roamingPosition);
            }

            StartCoroutine(AttackCooldownRoutine());
        }
    }

    private IEnumerator AttackCooldownRoutine(){
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;

    }

    private Vector2 GetRoamingPosition() {
        timeRoaming = 0f;
        return new Vector2(Random.Range(-1f, 1f), Random.Range(-1f, 1f)).normalized;
    }
}
