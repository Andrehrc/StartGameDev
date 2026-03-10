using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float timeMove;

    private float timeCount;
    private float direction = 1f;

    public void SetDirection(float value)
    {
        direction = value;
    }

    void Update()
    {
        timeCount += Time.deltaTime;

        if (timeCount < timeMove)
            transform.Translate(Vector2.right * direction * speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<PlayerBag>().totalWood++;
            Destroy(gameObject);
        }
    }
}
