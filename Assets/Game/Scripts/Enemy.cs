﻿using UnityEngine;

namespace FG
{
    public class Enemy : MonoBehaviour
    {
        [Tooltip("The multiplier for which to multiply the enemies initial force added (as they enter the scene)")]
        public float enemySpeedMultiplier;
        [Tooltip("The minimum velocity the enemy should bounce of the walls at")]
        public float minVelocity;
        [Tooltip("The hitpoints of the enemy, 1 will kill it in one machine gun shot")]
        public float hp = 1;

        private Rigidbody2D rb;
        private Vector3 lastVelocity;
        private float speed;
        private Vector3 direction;
        private void Awake()
        {

            Physics2D.IgnoreLayerCollision(10, 10);
            rb = GetComponent<Rigidbody2D>();
            rb.AddForce(transform.up * enemySpeedMultiplier, ForceMode2D.Impulse);
        }
        public void TakeDamage(float damage)
        {
            hp -= damage;
            if (hp <= 0)
            {
                Destroy(gameObject);
            }
        }

        private void Update()
        {
            lastVelocity = rb.velocity;



        }

        private void OnCollisionEnter2D(Collision2D collision)
        {
            if (collision.gameObject.tag == "Player")
            {
                GameObject.Destroy(this.gameObject);
                collision.gameObject.GetComponent<PlayerController>().hp -= 1;
            }
            else if (collision.gameObject.tag == "Wall")
            {
                speed = lastVelocity.magnitude;
                direction = Vector3.Reflect(lastVelocity.normalized, collision.contacts[0].normal);

                rb.velocity = direction * Mathf.Max(speed, minVelocity);
            }

        }



        private void OnTriggerExit2D(Collider2D collision)
        {
            GetComponent<CircleCollider2D>().isTrigger = false;
        }
    }
}
