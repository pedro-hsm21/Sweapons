using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    int playerNumber = -1;

    [SerializeField] GameObject bulletPrefab;
    [SerializeField] Transform spawnPosition;
    [SerializeField] SpriteRenderer graphics;
    
    [SerializeField] float fireRate = 0.3f;
    float currentFireTime = 0;

    Color myColor;

    public void SetPlayerNumber (int _number, Color _color)
    {
        myColor = _color;
        graphics.color = myColor;
        playerNumber = _number;
    }

    void Update()
    {
        if (currentFireTime <= 0)
        {
            if (InputManager.Instance.FireKeyIsPressed(playerNumber))
            {
                Shoot();
                currentFireTime = fireRate;
            }
        }
        else
        {
            currentFireTime -= Time.deltaTime;
        }
    }

    public virtual void Shoot()
    {
        Transform bullet = Instantiate(bulletPrefab, spawnPosition.position, Quaternion.identity).transform;
        bullet.localScale = new Vector3(Mathf.Sign(this.transform.lossyScale.x), 1, 1);

        bullet.GetComponent<Bullet>().SetPlayerNumber(playerNumber, myColor);
    }
}
