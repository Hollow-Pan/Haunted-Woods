using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shooter : MonoBehaviour, IEnemy {

    [SerializeField] private GameObject bulletPrefab;
    [SerializeField] private float bulletMoveSpeed = 8f;
    [SerializeField] private int burstCount = 5;
    [SerializeField] private int projectilesPerBurst = 7;
    [SerializeField][Range(0, 359)] private float angleSpread;
    [SerializeField] private float startingDistance = 0.1f;
    [SerializeField] private float timeBetweenBursts = 0.3f;
    [SerializeField] private float restTime = 1f;
    [SerializeField] private bool oscillate;
    [SerializeField] private bool stagger;

    private bool isShooting = false;

    private void OnValidate() {
        if (oscillate) stagger = true;
        else stagger = false;

        if (projectilesPerBurst < 1) projectilesPerBurst = 1;
        if (burstCount < 1) burstCount = 1;
        if (timeBetweenBursts < 0.1f) timeBetweenBursts = 0.1f;
        if (restTime < 0.1f) restTime = 0.1f;
        if (startingDistance < 0.1f) startingDistance = 0.1f;
        if (angleSpread == 0) projectilesPerBurst = 1;
        if (bulletMoveSpeed <= 0) bulletMoveSpeed = 0.1f;
    }

    public void Attack(){
        if (!isShooting){
            StartCoroutine(ShootBurstsRoutine());
        }
    }

    private IEnumerator ShootBurstsRoutine()
    {
        isShooting = true;

        float startAngle, endAngle, currentAngle, angleStep;
        float timeBetweenProjectiles = 0f;

        TargetConeOfInfluence(out startAngle, out endAngle, out currentAngle, out angleStep);

        if (stagger) timeBetweenProjectiles = timeBetweenBursts / projectilesPerBurst;

        for (int i = 0; i < burstCount; i++)
        {

            if (!oscillate){
                TargetConeOfInfluence(out startAngle, out endAngle, out currentAngle, out angleStep);
            }
            else if (oscillate && i % 2 == 0){
                TargetConeOfInfluence(out startAngle, out endAngle, out currentAngle, out angleStep);
            }
            else if (oscillate){
                currentAngle = endAngle;
                endAngle = startAngle;
                startAngle = currentAngle;
                angleStep *= -1;
            }
        
            for (int j = 0; j < projectilesPerBurst; j++)
            {

                Vector2 pos = BulletSpawnPosition(currentAngle);

                GameObject newBullet = Instantiate(bulletPrefab, pos, Quaternion.identity);
                newBullet.transform.right = newBullet.transform.position - this.transform.position;

                if (newBullet.TryGetComponent(out Projectile projectile))
                {
                    projectile.UpdateMoveSpeed(bulletMoveSpeed);
                }

                currentAngle += angleStep;
                if (stagger) yield return new WaitForSeconds(timeBetweenProjectiles);

            }

            currentAngle = startAngle;
            if (!stagger) yield return new WaitForSeconds(timeBetweenBursts);


        }

        yield return new WaitForSeconds(restTime);
        isShooting = false;
    }

    private void TargetConeOfInfluence(out float startAngle, out float endAngle, out float currentAngle, out float angleStep)
    {
        Vector2 targetDirection = PlayerController.Instance.transform.position - transform.position;
        float targetAngle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;
        startAngle = targetAngle;
        endAngle = targetAngle;
        currentAngle = targetAngle;
        float halfAngleSpread = 0f;
        angleStep = 0f;

        if (angleSpread != 0)
        {
            angleStep = angleSpread / (projectilesPerBurst - 1);
            halfAngleSpread = angleSpread / 2f;
            startAngle = targetAngle - halfAngleSpread;
            endAngle = targetAngle + halfAngleSpread;
            currentAngle = startAngle;
        }
    }

    private Vector2 BulletSpawnPosition(float currentAngle){
        float x = transform.position.x + startingDistance * Mathf.Cos(currentAngle * Mathf.Deg2Rad);
        float y = transform.position.y + startingDistance * Mathf.Sin(currentAngle * Mathf.Deg2Rad);

        Vector2 pos = new Vector2(x, y);
        return pos;
    }

}
