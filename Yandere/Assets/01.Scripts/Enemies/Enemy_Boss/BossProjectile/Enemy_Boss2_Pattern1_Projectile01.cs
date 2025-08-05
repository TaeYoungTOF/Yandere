using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Demo_Project.SceneManager;

    public class Enemy_Boss2_Pattern1_Projectile01 : MonoBehaviour
    {
        [Header("이펙트 프리팹")]
        public GameObject impactObject = null;
        public GameObject muzzleFlashEffect = null;

        
        [Header("총알 관련 설정")]
        [SerializeField] private float bulletSpeed = 5f;
        [SerializeField] private float bulletDamage = 10f;
        [SerializeField] private float impactEffectLifeTime = 0.5f;
        
        [Header("회전 관련")]
        [SerializeField] private float moveAngleRad = 0; // 라디안
        [SerializeField] private float spriteAngleRad = 0; // 라디안
        
        [Header("설정")]
        public string targetTag = "Player";
        public bool rotateSprite = true;
        public bool muzzleFlash = true;
        public float rotationSpeed = 0;
        public float rotationRange = 0;

        private bool rotateClockwise = false;
        private float timeSinceLastFrame = 0;
        
        private bool isFacingLeft = false;
        
        public void SetFacingDirection(bool facingLeft)
        {
            isFacingLeft = facingLeft;
        }

        void Start()
        {
            
            if (rotateSprite)
            {
                transform.rotation = Quaternion.Euler(0, 0, spriteAngleRad * Mathf.Rad2Deg);
            }

            if (muzzleFlash && muzzleFlashEffect != null)
            {
                GameObject flash = Instantiate(muzzleFlashEffect, transform.position, Quaternion.identity);

                // Z축 회전으로 파티클 방향 조정
                float rotZ = isFacingLeft ? 180f : 0f;
                flash.transform.rotation = Quaternion.Euler(0, 0, rotZ);

                Destroy(flash, 0.5f);
            }
        }

        void Update()
        {
            timeSinceLastFrame = Time.deltaTime / 0.001666f;
            Move();
            RotateProjectile();
        }

        public void Init(float damage, float speed, float angle)
        {
            bulletDamage = damage;
            bulletSpeed = speed;
            moveAngleRad = angle;
            spriteAngleRad = angle;
        }

        void Move()
        {
            float tempMoveSpeed = (bulletSpeed * timeSinceLastFrame) / 100f;
            Vector3 direction = new Vector3(Mathf.Cos(moveAngleRad), Mathf.Sin(moveAngleRad), 0);
            transform.Translate(direction * tempMoveSpeed, Space.World);
        }

        void RotateProjectile()
        {
            if (rotationRange > 0 && rotationSpeed > 0)
            {
                float zRot = rotationSpeed * timeSinceLastFrame;
                if (!rotateClockwise)
                {
                    transform.Rotate(0, 0, zRot);
                    if (transform.rotation.z * Mathf.Rad2Deg >= rotationRange)
                        rotateClockwise = true;
                }
                else
                {
                    transform.Rotate(0, 0, -zRot);
                    if (transform.rotation.z * Mathf.Rad2Deg <= -rotationRange)
                        rotateClockwise = false;
                }
            }
        }

        
        void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag(targetTag)) return;

            Player player = col.GetComponent<Player>();
            if (player != null)
            {
                player.TakeDamage(bulletDamage);
            }

            Impact();
        }

        void Impact()
        {
            if (impactObject != null)
            {
                GameObject impact = Instantiate(impactObject, transform.position, Quaternion.identity);
                Destroy(impact, impactEffectLifeTime); // 🔥 impact 이펙트 제거
            }

            Destroy(gameObject);
        }
    }