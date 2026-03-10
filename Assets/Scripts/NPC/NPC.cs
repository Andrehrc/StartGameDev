using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    public float speed;

    private float initialSpeed;
    private int index;
    private SpriteRenderer[] renderers;
    private Animator anim;

    public List<Transform> paths = new List<Transform>();

    void Start()
    {
        renderers = GetComponentsInChildren<SpriteRenderer>();
        anim = GetComponent<Animator>();
        initialSpeed = speed;
    }

    void Update()
    {
        if (paths.Count == 0) return;

        if (DialogContol.instance.IsShowing)
        {
            speed = 0f;
            anim.SetBool("isWalking", false);
            return;
        }

        speed = initialSpeed;
        anim.SetBool("isWalking", true);

        Vector2 direction = paths[index].position - transform.position;

        if (direction.x > 0)
        {
            foreach (var sr in renderers)
                sr.flipX = false;
        }

        if (direction.x < 0)
        {
            foreach (var sr in renderers)
                sr.flipX = true;
        }

        transform.position = Vector2.MoveTowards(transform.position, paths[index].position, speed * Time.deltaTime);

        if (Vector2.Distance(transform.position, paths[index].position) < 0.1f)
        {
            if (index < paths.Count - 1)
            {
                index++;
                //Para deixar random:
                //index = Random.Range(0, paths.Count - 1);
            }
            else
            {
                index = 0;
            }
        }
    }

    public void LookAtPlayer(Transform player)
    {
        if (player == null) return;

        float directionX = player.position.x - transform.position.x;

        if (directionX > 0)
        {
            foreach (var sr in renderers)
                sr.flipX = false;
        }
        else if (directionX < 0)
        {
            foreach (var sr in renderers)
                sr.flipX = true;
        }
    }
}
