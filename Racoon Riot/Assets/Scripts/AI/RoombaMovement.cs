using UnityEngine;
using System.Collections;

public class RoombaMovement : MonoBehaviour
{

    [Header("Movement Settings")]
    [SerializeField] private float _movementSpeed = 3.0f;
    [SerializeField] private float _rotationSpeed = 120.0f;
    [SerializeField] private float _backupSpeed = 1.5f;

    [Header("Behaviour Settings")]
    [SerializeField] private float _backupTime = 0.5f;
    [SerializeField] private float _minTurnAngle = 90.0f;
    [SerializeField] private float _maxTurnAngle = 180.0f;

    // State Variables
    private enum State {  MovingForward, MovingBackward, Turning }
    private State _currentState = State.MovingForward;

    private Quaternion _targetRotation;
    private Rigidbody _rb;

    private void Awake()
    {
        _rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        // State Machine
        switch (_currentState)
        {
            //Forward
            case State.MovingForward:
                MoveForward();
                break;
            //Backward
            case State.MovingBackward:
                MoveBackward();
                break;
            //Turning
            case State.Turning:
                Turn();
                break;
        }
    }

    private void MoveForward()
    {
        _rb.linearVelocity = transform.forward * _movementSpeed;
    }

    private void MoveBackward()
    {
        _rb.linearVelocity = -transform.forward * _backupSpeed;
    }

    // Calculate where to turn
    private void StartTurn()
    {
        float turnAngle = Random.Range(_minTurnAngle, _maxTurnAngle);

        // 50/50 on turning left or right
        if (Random.value > 0.5f)
        {
            turnAngle = -turnAngle;
        }

        _targetRotation = Quaternion.Euler(0, turnAngle, 0) * transform.rotation;
        _currentState = State.Turning;
    }

    private void Turn()
    {
        transform.rotation = Quaternion.RotateTowards(transform.rotation, _targetRotation, _rotationSpeed * Time.deltaTime);

        if (Quaternion.Angle(transform.rotation, _targetRotation) < 1.0f)
        {
            transform.rotation = _targetRotation;
            _currentState = State.MovingForward;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        // If hit something stop for a bit, then backup, then turn
        if (_currentState == State.MovingForward)
        {
            _currentState = State.MovingBackward;

            // Stop for half a second before backing up
            Invoke("StartBackup", 0.5f);
        }
    }

    private void StartBackup()
    {
        _currentState = State.MovingBackward;
        Invoke("StartTurn", _backupTime);
    }

}
