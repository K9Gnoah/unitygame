using UnityEngine;

public class Fireball : MonoBehaviour
{
    public Animator fireballAnimator; // Animator của quả cầu lửa
    private Rigidbody2D rb; // Rigidbody của quả cầu lửa

    void Start()
    {
        // Lấy Animator và Rigidbody2D của quả cầu lửa nếu chưa được gán
        if (fireballAnimator == null)
        {
            fireballAnimator = GetComponent<Animator>();
        }

        rb = GetComponent<Rigidbody2D>();
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        // Khi va chạm với mặt đất hoặc một đối tượng có tag là "Ground"
        if (collision.gameObject.CompareTag("Ground"))
        {
            // Kích hoạt animation nổ
            Explode();
        }
    }

    void Explode()
    {
        // Kích hoạt trigger "Explode" trong Animator để phát animation nổ
        if (fireballAnimator != null)
        {
            fireballAnimator.SetTrigger("Explode");
        }

        // Dừng mọi chuyển động của quả cầu lửa
        rb.velocity = Vector2.zero; // Dừng chuyển động
        rb.isKinematic = true; // Chuyển về Kinematic để không bị ảnh hưởng bởi va chạm
        Destroy(gameObject, 0.5f); // Có thể điều chỉnh thời gian sau khi phát nổ

        // Vô hiệu hóa Collider để không còn va chạm sau khi phát nổ
        //Collider2D collider = GetComponent<Collider2D>();
        //if (collider != null)
        //{
        //    collider.enabled = false; // Vô hiệu hóa va chạm
        //}
    }
}
