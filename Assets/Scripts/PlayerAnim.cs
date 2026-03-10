using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAnim : MonoBehaviour
{
    private Player player;
    private Animator anim;
    private SpriteRenderer sr;

    private bool _rollTriggered;

    void Start()
    {
        player = GetComponent<Player>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
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

    }

}
