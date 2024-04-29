using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Movement")]
    public float _moveSpeed;
    public float _groundDrag;

    [Header("Ground Check")]
    public float _playerHeight;
    public LayerMask _identifyGround;
    bool _grounded;

    public Transform _orient;

    float _horiInput;
    float _vertiInput;

    Vector3 _moveDirection;
    Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
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
    }

    private void MovePlayer()
    {
        _moveDirection = _orient.forward * _vertiInput + _orient.right * _horiInput;

        rb.AddForce(_moveDirection.normalized * _moveSpeed * 10f, ForceMode.Force);
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
}
