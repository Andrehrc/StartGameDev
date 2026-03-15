using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnim : MonoBehaviour
{
    private Player player;
    private Animator anim;
    private SpriteRenderer sr;
    private Casting casting;

    private bool _rollTriggered;
    private bool _fishingTriggered;

    private void Awake()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        casting = FindFirstObjectByType<Casting>();
    }

    void Start()
    {
        
    }

    void Update()
    {
        if (player.isRolling)
        {
            if (!_rollTriggered)
            {
                anim.SetTrigger("isRoll");
                _rollTriggered = true;
            }

            return;
        }

        _rollTriggered = false;

        if (player.IsFishing)
        {
            return;
        }

        _fishingTriggered = false;

        if (player.direction.sqrMagnitude > 0)
            anim.SetInteger("transition", player.isRunning ? 2 : 1);
        else
            anim.SetInteger("transition", value: 0);

        if (player.direction.x > 0)
            transform.eulerAngles = new Vector3(0, 0, 0);
        else if (player.direction.x < 0)
            transform.eulerAngles = new Vector3(0, 180, 0);

        if (player.isCutting)
            anim.SetInteger("transition", value: 3);

        if (player.isDigging)
            anim.SetInteger("transition", value: 4);

        if (player.isWatering)
            anim.SetInteger("transition", value: 5);

    }

    public void OnCastingStarted()
    {
        if (!_fishingTriggered)
        {
            player.IsFishing = true;

            if (player.direction.sqrMagnitude > 0)
                player.lockDirection = player.direction.normalized;

            anim.SetTrigger("isCasting");
            _fishingTriggered = true;
        }
    }

    public void OnCastingEnded()
    {
        casting.OnCasting();
    }

    public void OnCastingAnimationEnded()
    {
        player.IsFishing = false;
        player.ResetSpeed();
    }

}
