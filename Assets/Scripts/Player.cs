using UnityEngine;
using UnityEngine.InputSystem;

public class Player : MonoBehaviour
{
    public bool isPaused;

    public float speed = 5;
    public float runSpeed = 9;
    public float rollSpeed = 12f;

    [HideInInspector] public Tools handlingObj;
    [HideInInspector] public Vector2 lockDirection;
    [HideInInspector]
    public Vector2 LookDirection
    {
        get
        {
            if (_isRolling || _isFishing)
                return lockDirection;

            return _direction.normalized;
        }
    }

    float initialSpeed;
    bool _isRunning;
    bool _isRolling;
    bool _isCutting;
    bool _isDigging;
    bool _isWatering;
    bool _isFishing;
    bool _isAttacking;

    PlayerBag bag;
    Rigidbody2D rig;
    Vector2 _direction;

    HudController hud;

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

    public bool isDigging
    {
        get { return _isDigging; }
        set { _isDigging = value; }
    }

    public bool isWatering
    {
        get { return _isWatering; }
        set { _isWatering = value; }
    }

    public bool IsFishing { get => _isFishing; set => _isFishing = value; }
    public bool IsAttacking { get => _isAttacking; set => _isAttacking = value; }

    private void Awake()
    {
        rig = GetComponent<Rigidbody2D>();
        bag = GetComponent<PlayerBag>();
        hud = FindFirstObjectByType<HudController>();
    }

    private void Start()
    {
        rig = GetComponent<Rigidbody2D>();
        bag = GetComponent<PlayerBag>();
        initialSpeed = speed;

        hud.UpdateToolUi((int)handlingObj);
    }

    private void Update()
    {
        if (!isPaused)
        {
            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                handlingObj = Tools.axe;
                hud.UpdateToolUi((int)handlingObj);
            }

            if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                handlingObj = Tools.shovel;
                hud.UpdateToolUi((int)handlingObj);
            }

            if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                handlingObj = Tools.wateringCan;
                hud.UpdateToolUi((int)handlingObj);
            }

            if (Keyboard.current.digit4Key.wasPressedThisFrame)
            {
                handlingObj = Tools.sword;
                hud.UpdateToolUi((int)handlingObj);
            }

            OnInput();
            OnRun();
            OnRolling();
            OnCutting();
            OnDigging();
            OnWatering();
            OnAttacking();
            SetCharSpeed();
        }
    }

    private void FixedUpdate()
    {
        OnMove();
    }


    #region Movement

    void OnCutting()
    {
        if (handlingObj != Tools.axe)
        {
            _isCutting = false;
            return;
        }

        _isCutting = Keyboard.current.qKey.isPressed;
    }

    void OnDigging()
    {
        if (handlingObj != Tools.shovel)
        {
            _isDigging = false;
            return;
        }

        _isDigging = Keyboard.current.qKey.isPressed;
    }

    void OnWatering()
    {
        if (handlingObj != Tools.wateringCan || bag.currentWater <= 0)
        {
            _isWatering = false;
            return;
        }

        _isWatering = Keyboard.current.qKey.isPressed;

        if (_isWatering)
        {
            speed = 0;
            bag.currentWater -= 0.01f;
            if (bag.currentWater <= 0)
            {
                bag.currentWater = 0;
                _isWatering = false;
            }
            return;
        }
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
        var moveDir = _direction.normalized;

        if (_isRolling || _isFishing)
            moveDir = lockDirection;

        rig.MovePosition(rig.position + moveDir * speed * Time.fixedDeltaTime);
    }

    void OnRun()
    {
        _isRunning = Keyboard.current.leftShiftKey.isPressed;
    }

    void OnRolling()
    {
        if (_isRolling) return;

        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            if (_direction.sqrMagnitude == 0) return;

            _isRolling = true;
            lockDirection = _direction.normalized;
            speed = rollSpeed;
        }
    }

    public void EndRoll()
    {
        _isRolling = false;
        speed = initialSpeed;
    }

    public void ResetSpeed()
    {
        speed = initialSpeed;
        lockDirection = _direction.normalized;
    }

    public void SetCharSpeed()
    {
        if (_isRolling)
        {
            speed = rollSpeed;
            return;
        }

        if (_isFishing || _isDigging || _isCutting || _isWatering || _isAttacking)
        {
            speed = 0;
            return;
        }

        speed = _isRunning ? runSpeed : initialSpeed;
    }

    #endregion Movement

    void OnAttacking()
    {
        if (handlingObj != Tools.sword)
        {
            _isAttacking = false;
            return;
        }

        _isAttacking = Keyboard.current.qKey.isPressed;
    }

    public enum Tools
    {
        axe,
        shovel,
        wateringCan,
        sword,
    }
}
