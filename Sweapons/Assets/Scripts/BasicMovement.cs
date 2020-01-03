using System;
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
    [SerializeField] LayerMask horizontalCollisionMask;

    [SerializeField] float smoothLandGravityMultiplier = 0.95f;
    [SerializeField] float gravityFallMultiplier = 2.0f;
    [SerializeField] float jumpForce = 6.0f;
    bool isGrounded;

    float currentLookingX = 0;
    [SerializeField] float xSmooth = 0.1f;
    [SerializeField] float xSpeed = 5.0f;
    float currentHorizontalVelocity;

    public float CurrentHorizontalVelocity => currentHorizontalVelocity;
    public bool IsGrounded => isGrounded;

    private void Awake()
    {
        _transform = transform;
        _rigidbody = _transform.GetComponent<Rigidbody2D>() as Rigidbody2D;
        _collider = _transform.GetComponent<BoxCollider2D>() as BoxCollider2D;

        //gravityFallMultiplier = (2 * gravityCompensation) / (gravityCompensation + Physics2D.gravity.y);
    }

    private void Start()
    {
        currentLookingX = _transform.localScale.x;
    }

    private void FixedUpdate()
    {
        CheckVerticalCollision();


        float additionalGravityForce = 0;
        if (_rigidbody.velocity.y < 0 && !isGrounded)
        {
            additionalGravityForce = -Mathf.Abs(Physics2D.gravity.y) * (gravityFallMultiplier - 1) * Time.fixedDeltaTime;
        }

        _rigidbody.velocity = new Vector2(currentHorizontalVelocity, _rigidbody.velocity.y + additionalGravityForce);

        if (MainCamera.Instance.HorizontalBounds.IsOutOfBounds(_transform.position))
            _transform.position = MainCamera.Instance.HorizontalBounds.GetPositionInBounds(_transform.position);
    }

    private void CheckVerticalCollision()
    {
        isGrounded = false;
        if (_rigidbody.velocity.y > 0.1f) return;
        
        float skinWidth = 0.001f;
        for (int i = 0; i < 3; i++)
        {
            float xPosition = (_collider.bounds.min.x - skinWidth) + ((_collider.bounds.size.x - skinWidth * 2) / 2 * i);
            Vector2 origin = new Vector2(xPosition, _collider.bounds.min.y);

            RaycastHit2D hitInfo = Physics2D.Raycast(origin, Vector2.down, 0.1f + Mathf.Abs(_rigidbody.velocity.y) / 30, grounMask);
            if (hitInfo)
            {
                Debug.DrawRay(origin, Vector2.down * (0.1f + Mathf.Abs(_rigidbody.velocity.y) / 30), Color.green);
                isGrounded = true;
                _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _rigidbody.velocity.y * smoothLandGravityMultiplier);
                return;
            }
            else
            {
                Debug.DrawRay(origin, Vector2.down * (0.1f + Mathf.Abs(_rigidbody.velocity.y) / 30), Color.red);
            }
        }
    }

    public void Move(Vector2 _direction)
    {
        currentHorizontalVelocity = Mathf.Lerp(currentHorizontalVelocity, _direction.x * xSpeed, xSmooth);

        float skinWidth = 0.001f;
        for (int i = 0; i < 3; i++)
        {
            Vector2 origin = _transform.position;
            origin.y = (_collider.bounds.min.y + skinWidth) + (_collider.bounds.size.y - skinWidth * 2) / 2 * i;

            RaycastHit2D hit = Physics2D.Raycast(origin, Vector2.right * currentLookingX, 0.1f, horizontalCollisionMask);
            
            if (hit)
            {
                currentHorizontalVelocity = 0;
                Debug.DrawRay(origin, Vector2.right * 0.1f * currentLookingX, Color.green);
            }
            else
            {
                Debug.DrawRay(origin, Vector2.right * 0.1f * currentLookingX, Color.red);
            }
        }

        if (_direction.x != 0 && Mathf.Sign(_direction.x) != Mathf.Sign(currentLookingX)) Flip();
    }

    public void Flip()
    {
        currentLookingX = _transform.localScale.x * -1;
        _transform.localScale = new Vector3(currentLookingX, _transform.localScale.y, _transform.localScale.z);
    }

    public void Jump ()
    {
        if (!isGrounded) return;

        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, jumpForce);
    }

    public void StopPhysics ()
    {
        _rigidbody.velocity = new Vector2();
        currentHorizontalVelocity = 0;
        _rigidbody.isKinematic = true;
        _collider.enabled = false;
    }

    public void StartPhysics()
    {
        _rigidbody.isKinematic = false;
        _collider.enabled = true;
    }

    public void SetForce(Vector2 force)
    {
        Debug.Log(force);
        force.x = force.x * 3;
        force.y = _rigidbody.velocity.y + force.y / 2;
        _rigidbody.velocity = force;
        currentHorizontalVelocity = force.x;
    }
}
