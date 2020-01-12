using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BasicMovement))]
public class Player : MonoBehaviour
{
    BasicMovement _movementController;

    GameObject _currentWeapon;
    [SerializeField] Transform _weaponHolderTransform;

    [SerializeField] Transform _healthBar;
    [SerializeField] float totalLifeTime = 3.0f;
    float currentLifeTime = 0;
    [SerializeField] int lifes = 3;
    public int Lifes => lifes;

    public int PlayerNumber => playerNumber;
    int playerNumber = -1;
    Color _color;
    [SerializeField] SpriteRenderer _graphics;
    [SerializeField] ParticleSystem _deathParticle;

    public bool IsAlive => isAlive;
    bool isAlive;
    public bool CanMove => canMove;
    bool canMove;
    Vector2 velocity;

    [SerializeField] Animator _animator;

    private void Awake()
    {
        _movementController = transform.GetComponent<BasicMovement>();
        _animator = GetComponent<Animator>();
    }

    private void Start()
    {
        isAlive = true;
        currentLifeTime = totalLifeTime;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F7)) totalLifeTime = 1000;
        
        if (!isAlive || !canMove) return;

        MovementInput();
        UpdateLife();
    }

    void MovementInput()
    {
        velocity.x = 0;

        if (InputManager.Instance.LeftInputIsPressed(playerNumber)) velocity.x -= 1;
        else if (InputManager.Instance.RightInputIsPressed(playerNumber)) velocity.x += 1;

        _movementController.Move(velocity);

        if (InputManager.Instance.JumpInputIsPressed(playerNumber))
            _movementController.Jump();
        
        _animator.SetBool("IsWalking", Mathf.Abs(_movementController.CurrentHorizontalVelocity) > 0.1f);
        _animator.SetBool("IsGrounded", _movementController.IsGrounded);
    }

    void UpdateLife()
    {
        currentLifeTime -= Time.deltaTime;
        if (InputManager.Instance.FireKeyIsPressed(playerNumber))
            currentLifeTime = totalLifeTime;

        _healthBar.localScale = new Vector3(currentLifeTime / totalLifeTime, 1, 1);

        if (currentLifeTime <= 0) Die();
    }

    public void HitDamage(int _hitPlayerNumber = -1)
    {
        if (playerNumber == _hitPlayerNumber || _hitPlayerNumber == -1) Die();
    }

    public void SetPlayerNumber (int number, Color color)
    {
        PlayerHUDManager.Instance.InitializePlayerHUD(number, color);
        _color = color;
        var mainModule = _deathParticle.main;
        mainModule.startColor = _color;
        _graphics.color = _color;
        playerNumber = number;
    }

    public void SetWeapon(GameObject newWeapon)
    {
        if (_currentWeapon != null) Destroy(_currentWeapon);

        Transform _weaponTransform = newWeapon.transform;
        _weaponTransform.position = this._weaponHolderTransform.position;
        _weaponTransform.localScale = this._weaponHolderTransform.lossyScale;
        _weaponTransform.parent = this._weaponHolderTransform;

        _currentWeapon = newWeapon;
    }

    void Die()
    {
        if (!isAlive) return;

        StartCoroutine(DieCoroutine());
    }

    IEnumerator DieCoroutine ()
    {
        ExplosionSFXManager.Instance.PlayShotSFX();
        PlayerHUDManager.Instance.SetPlayerDeath(playerNumber);
        _movementController.StopPhysics();
        isAlive = false;
        MainCamera.Instance.Shake(0.25f, 0.25f);
        lifes--;
        currentLifeTime = totalLifeTime;
        _currentWeapon.SetActive(false);
        _graphics.enabled = false;

        _deathParticle.Emit(25);

        if (lifes == 0) yield break;
        yield return new WaitForSeconds(_deathParticle.main.startLifetime.constant);

        PlayerManager.Instance.SetSpawnPosition(this.transform);
        isAlive = true;
        _graphics.enabled = true;
        _currentWeapon.SetActive(true);
        _movementController.StartPhysics();
    }

    public float PlayerLifes()
    {
        return lifes;
    }

    public void SetForce(Vector2 force)
    {
        _movementController.SetForce(force);
    }

    public void StopPlayerMovement()
    {
        canMove = false;   
    }

    public void StartPlayerMovement()
    {
        canMove = true;
    }
}
