using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public float speed = 5;
    public float runSpeed = 9;
    public float rollSpeed = 12f;

    float initialSpeed;
    bool _isRunning;
    bool _isRolling;
    bool _isCutting;

    Rigidbody2D rig;
    Vector2 _direction;
    Vector2 rollDirection;

    public Vector2 direction
    {
        get { return _direction; }
        set { _direction = value; }
    }

    public bool isRunning
    {
        get { return _isRunning; }
        set { _isRunning = value; }
    }

    public bool isRolling
    {
        get { return _isRolling; }
        set { _isRolling = value; }
    }

    public bool isCutting
    {
        get { return _isCutting; }
        set { _isCutting = value; }
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        initialSpeed = speed;
    }

    private void Update()
    {
        OnInput();
        OnRun();
        OnRolling();
        OnCutting();
    }

    private void FixedUpdate()
    {
        OnMove();
    }


    #region Movement

    void OnCutting()
    {
        _isCutting = Keyboard.current.qKey.isPressed;

        if (_isCutting)
        {
            speed = 0;
            return;
        }

        if (_isRolling)
        {
            return;
        }

        if (_isRunning)
        {
            speed = runSpeed;
            return;
        }

        speed = initialSpeed;
    }

    void OnInput()
    {
        _direction = new Vector2(
                Keyboard.current.aKey.isPressed ? -1 :
                Keyboard.current.dKey.isPressed ? 1 : 0,

                Keyboard.current.sKey.isPressed ? -1 :
                Keyboard.current.wKey.isPressed ? 1 : 0
            );
    }

    void OnMove()
    {
        var moveDir = _isRolling ? rollDirection : _direction.normalized;
        rig.MovePosition(rig.position + moveDir * speed * Time.fixedDeltaTime);
    }

    void OnRun()
    {
        if (_isRolling) return;

        speed = Keyboard.current.leftShiftKey.isPressed ? runSpeed : initialSpeed;
        _isRunning = Keyboard.current.leftShiftKey.isPressed;
    }


    void OnRolling()
    {
        if (_isRolling) return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (_direction.sqrMagnitude == 0) return;

            _isRolling = true;
            rollDirection = _direction.normalized;
            speed = rollSpeed;
        }
    }

    public void EndRoll()
    {
        _isRolling = false;
        speed = initialSpeed;
    }

    #endregion Movement


}
