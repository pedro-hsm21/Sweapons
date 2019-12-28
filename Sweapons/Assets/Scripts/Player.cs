using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicMovement))]
public class Player : MonoBehaviour
{
    BasicMovement movementController;

    [SerializeField] GameObject currentWeapon;
    [SerializeField] Transform weaponTransform;

    [SerializeField] Transform HealthBar;
    float totalLifeTime = 7.0f;
    float currentLifeTime = 0;
    [SerializeField] int lifes = 3;

    int playerNumber = -1;
    [SerializeField] SpriteRenderer graphics;

    bool isAlive;
    Vector2 velocity;

    private void Awake()
    {
        movementController = transform.GetComponent<BasicMovement>();
    }

    private void Start()
    {
        isAlive = true;
        currentLifeTime = totalLifeTime;
    }

    private void Update()
    {
        if (!isAlive) return;

        MovementInput();
        UpdateLife();
    }

    void MovementInput()
    {
        velocity.x = 0;

        if (InputManager.Instance.LeftInputIsPressed(playerNumber)) velocity.x -= 1;
        if (InputManager.Instance.RightInputIsPressed(playerNumber)) velocity.x += 1;

        movementController.Move(velocity);

        if (InputManager.Instance.JumpInputIsPressed(playerNumber))
            movementController.Jump();
    }

    void UpdateLife()
    {
        currentLifeTime -= Time.deltaTime;
        if (InputManager.Instance.FireKeyIsPressed(playerNumber))
            currentLifeTime = totalLifeTime;

        HealthBar.localScale = new Vector3(currentLifeTime / totalLifeTime, 1, 1);

        if (currentLifeTime <= 0) Die();
    }

    public void BulletCollision(int _bulletPlayerNumber)
    {
        if (playerNumber == _bulletPlayerNumber) Die();
    }

    public void SetPlayerNumber (int _number, Color _color)
    {
        graphics.color = _color;
        playerNumber = _number;
    }

    public void SetWeapon(GameObject newWeapon)
    {
        if (currentWeapon != null) Destroy(currentWeapon);

        Transform _weaponTransform = newWeapon.transform;
        _weaponTransform.position = weaponTransform.position;
        _weaponTransform.parent = weaponTransform;
        _weaponTransform.localScale = weaponTransform.lossyScale;

        currentWeapon = newWeapon;
    }

    void Die()
    {
        if (!isAlive) return;

        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine ()
    {
        lifes--;
        if (lifes == 0) Destroy(this);

        graphics.enabled = false;
        yield return new WaitForSeconds(0.2f);

        PlayerManager.Instance.SetSpawnPosition(this.transform);
        isAlive = true;
        graphics.enabled = true;
    }
}
