using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float _moveSpeed;
    public float _groundDrag;

    public float _jumpForce;
    public float _jumpCooldown;
    public float _airMult;
    public bool _jumpReady;

    [Header("Keybinds")]
    public KeyCode jumpKey = KeyCode.Space;

    [Header("Ground Check")]
    public float _playerHeight;
    public LayerMask _identifyGround;
    public bool _grounded;

    public Transform _orient;

    float _horiInput;
    float _vertiInput;

    Vector3 _moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        _jumpReady = true;
    }

    private void Update()
    {
        _grounded = Physics.Raycast(transform.position, Vector3.down, _playerHeight * 0.5f + 0.2f, _identifyGround);
        MoveInput();
        SpeedControl();

        if (_grounded)
        {
            rb.drag = _groundDrag;
        } else {
            rb.drag = 0;
        }
    }

    private void FixedUpdate()
    {
        MovePlayer();
    }

    private void MoveInput()
    {
        _horiInput = Input.GetAxisRaw("Horizontal");
        _vertiInput = Input.GetAxisRaw("Vertical");

        if(Input.GetKey(jumpKey) && _jumpReady && _grounded)
        {
            _jumpReady = false;
            Jump();
            Invoke(nameof(ResetJump), _jumpCooldown);
        }
    }

    private void MovePlayer()
    {
        _moveDirection = _orient.forward * _vertiInput + _orient.right * _horiInput;

        if (_grounded)
            rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
        else if (!_grounded)
            rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f * _airMult, ForceMode.Force);
    }

    private void SpeedControl()
    {
        Vector3 _flatVelocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);

        if(_flatVelocity.magnitude > _moveSpeed)
        {
            Vector3 _limitVelocity = _flatVelocity.normalized * _moveSpeed;
            rb.velocity = new Vector3(_limitVelocity.x, rb.velocity.y, _limitVelocity.z);
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector3(rb.velocity.x, 0f, rb.velocity.z);
        rb.AddForce(transform.up * _jumpForce, ForceMode.Impulse);
    }

    private void ResetJump()
    {
        _jumpReady = true;
    }
}
