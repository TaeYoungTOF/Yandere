using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Demo_Project.SceneManager;

    public class BossPattern2_Projectile : MonoBehaviour
    {
        [Header("이펙트 프리팹")]
        public GameObject impactObject = null;
        public GameObject muzzleFlashObject = null;

        [Header("설정")]
        public bool rotateSprite = true;
        public bool muzzleFlash = true;
        public bool explodeAtScreenEdge = true;

        [Header("이동 관련")]
        public float moveAngle = 0; // 라디안
        public float spriteAngle = 0; // 라디안
        public float moveSpeed = 5f;

        [Header("데미지 설정")]
        public float damage = 10f;
        public string targetTag = "Player";
        
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
                transform.rotation = Quaternion.Euler(0, 0, spriteAngle * Mathf.Rad2Deg);
            }

            if (muzzleFlash && muzzleFlashObject != null)
            {
                GameObject flash = Instantiate(muzzleFlashObject, transform.position, Quaternion.identity);

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
            CheckIfOffScreen();
        }

        void Move()
        {
            float tempMoveSpeed = (moveSpeed * timeSinceLastFrame) / 100f;
            Vector3 direction = new Vector3(Mathf.Cos(moveAngle), Mathf.Sin(moveAngle), 0);
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

        void CheckIfOffScreen()
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
            if (screenPos.y < 0 || screenPos.x < 0 || screenPos.x > Screen.width || screenPos.y > Screen.height)
            {
                if (explodeAtScreenEdge)
                {
                    Impact();
                }
                else
                {
                    Destroy(gameObject);
                }
            }
        }

        void OnTriggerEnter2D(Collider2D col)
        {
            if (!col.CompareTag(targetTag)) return;

            var player = col.GetComponent<Player>();
            if (player != null)
            {
                StageManager.Instance.Player.TakeDamage(damage); // 직접 호출로 확인
            }

            Impact();
        }

        void Impact()
        {
            if (impactObject != null)
            {
                GameObject impact = Instantiate(impactObject, transform.position, transform.rotation);
                Destroy(impact, 1f); // 🔥 impact 이펙트 제거
            }

            Destroy(gameObject);
        }
    }