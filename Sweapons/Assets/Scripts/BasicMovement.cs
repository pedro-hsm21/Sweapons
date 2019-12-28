using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(Rigidbody2D))]
public class BasicMovement : MonoBehaviour
{
    Rigidbody2D _rigidbody;
    BoxCollider2D _collider;
    Transform _transform;
    [SerializeField] LayerMask grounMask;

    [SerializeField] float jumpForce = 6.0f;
    bool canJump;

    float currentLookingX = 0;
    [SerializeField] float xSmooth = 0.1f;
    [SerializeField] float xSpeed = 5.0f;
    float currentHorizontalVelocity;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = _transform.GetComponent<Rigidbody2D>() as Rigidbody2D;
        _collider = _transform.GetComponent<BoxCollider2D>() as BoxCollider2D;
    }

    private void Start()
    {
        currentLookingX = _transform.localScale.x;
    }

    private void FixedUpdate()
    {
        RaycastHit2D hitInfo = Physics2D.Raycast(_transform.position, Vector2.down, 0.6f, grounMask);
        Debug.DrawRay(_transform.position, Vector2.down * 0.6f, Color.red);

        canJump = (hitInfo);

        _rigidbody.velocity = new Vector2(currentHorizontalVelocity, _rigidbody.velocity.y);
    }

    public void Move(Vector2 _direction)
    {
        currentHorizontalVelocity = Mathf.Lerp(currentHorizontalVelocity, _direction.x * xSpeed, xSmooth);

        if (_direction.x != 0 && Mathf.Sign(_direction.x) != Mathf.Sign(currentLookingX)) Flip();
    }

    public void Flip()
    {
        currentLookingX = _transform.localScale.x * -1;
        _transform.localScale = new Vector3(currentLookingX, _transform.localScale.y, _transform.localScale.z);
    }

    public void Jump ()
    {
        if (!canJump) return;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
    }
}
