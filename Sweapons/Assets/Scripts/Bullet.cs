using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] SpriteRenderer graphics;
    int playerNumber;
    float lifeTime = 10.0f;

    [SerializeField] float xVelocity = 8.0f;
    
    void FixedUpdate()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, transform.right * transform.localScale.x, 0.3f);
        if (hit)
        {
            Debug.DrawRay(transform.position, transform.right * 0.3f, Color.red, 1.5f);
            Player _player = hit.transform.GetComponent<Player>() as Player;

            if (_player != null)
            {
                _player.BulletCollision(playerNumber);
            }

            Destroy(this.gameObject);
        }
        else
        {
            transform.Translate(transform.right * transform.localScale.x * xVelocity * Time.deltaTime);

            Debug.DrawRay(transform.position, transform.right * 0.3f, Color.cyan);
            lifeTime -= Time.deltaTime;
            if (lifeTime <= 0) Destroy(this.gameObject);
        }
    }

    public void SetPlayerNumber(int _number, Color _color)
    {
        graphics.color = _color;
        playerNumber = _number;
    }
}
