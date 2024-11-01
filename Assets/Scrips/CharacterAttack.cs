using UnityEngine;

public class CharacterAttack : MonoBehaviour
{
    public Animator characterAnimator; // Animator của nhân vật
    public Animator fireballAnimator; // Animator của quả cầu lửa
    public GameObject fireballPrefab; // Prefab quả cầu lửa
    public Transform firePoint; // Vị trí tạo chiêu
    public float fireballSpeed = 10f; // Tốc độ của chiêu
    public GameObject fireEffectPrefab; // Prefab của ngọn lửa


    private GameObject currentFireball; // Quả cầu lửa đang tạo ra
    private GameObject currentFireEffect; // Ngọn lửa đang kích hoạt


    void Update()
    {
        // Khi người chơi nhấn phím để tạo chiêu (ví dụ: phím Q)
        if (Input.GetKeyDown(KeyCode.Q))
        {
            // Kích hoạt animation tạo chiêu cho nhân vật
            characterAnimator.SetTrigger("CreateAttack");
        }
    }

    // Hàm này được gọi khi Animation Event trong animation "CreateAttack"
    void CreateFireball()
    {
        // Tạo quả cầu lửa tại vị trí của firePoint
        currentFireball = Instantiate(fireballPrefab, firePoint.position, Quaternion.identity);
        //currentFireball.transform.localScale = new Vector3(2f, 2f, 2f); // Thay đổi kích thước theo ý muốn

        currentFireball.SetActive(true); // Hiển thị quả cầu lửa ngay lập tức

        // Gán Rigidbody2D cho quả cầu lửa và tắt trọng lực khi tạo ra
        Rigidbody2D rb = currentFireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0; // Tắt trọng lực để quả cầu lửa không rơi ngay sau khi tạo ra
            rb.bodyType = RigidbodyType2D.Kinematic; // Chuyển sang Kinematic để không bị ảnh hưởng bởi va chạm
        }

        // Kích hoạt animation cho quả cầu lửa
        Animator fireballAnimator = currentFireball.GetComponent<Animator>();
        if (fireballAnimator != null)
        {
            fireballAnimator.SetTrigger("CreateFireball"); // Kích hoạt animation "CreateFireball"
        }

        // Gọi hàm để giữ chiêu (nếu cần)
        HoldFireball(); // Không cần Invoke nữa
    }

    void FireEffect()
    {
        //// Tạo ngọn lửa tại vị trí ở chân nhân vật
        //Vector3 firePosition = currentFireball.transform.position + new Vector3(0, -0.5f, 0); // Điều chỉnh khoảng cách tùy thuộc vào yêu cầu của bạn

        //// Tạo ngọn lửa nếu chưa có
        //if (currentFireEffect == null)
        //{
        //    // Tạo ngọn lửa
        //    currentFireEffect = Instantiate(fireEffectPrefab, firePosition, Quaternion.identity);
        //    currentFireEffect.transform.SetParent(currentFireball.transform); // Nếu muốn ngọn lửa là con của quả cầu lửa

        //    Animator fireAnimator = currentFireEffect.GetComponent<Animator>();
        //    if (fireAnimator != null)
        //    {
        //        fireAnimator.SetTrigger("StartFire"); // Kích hoạt animation ngọn lửa
        //    }
        //}
    }


    void HoldFireball()
    {
        // Kích hoạt animation giữ chiêu cho nhân vật và quả cầu lửa
        characterAnimator.SetTrigger("HoldAttack");
        Animator fireballAnimator = currentFireball.GetComponent<Animator>();
        if (fireballAnimator != null)
        {
            fireballAnimator.SetTrigger("HoldFireball");
        }

        // Sau một khoảng thời gian, chuyển sang giai đoạn ném chiêu
        Invoke("ThrowFireball", 2.0f); // Điều chỉnh thời gian tùy thuộc vào animation
    }

    void ThrowFireball()
    {
        // Kích hoạt animation ném chiêu cho nhân vật
        characterAnimator.SetTrigger("ThrowAttack");

        // Kích hoạt animation ném cho quả cầu lửa
        Animator fireballAnimator = currentFireball.GetComponent<Animator>();

        if (fireballAnimator != null)
        {
            fireballAnimator.SetTrigger("ThrowFireball");
        }

        // Chuyển Rigidbody2D của quả cầu lửa từ Kinematic về Dynamic và gán vận tốc cho nó
        Rigidbody2D rb = currentFireball.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.bodyType = RigidbodyType2D.Dynamic; // Chuyển sang Dynamic để chịu tác động của trọng lực
            rb.gravityScale = 1; // Bật trọng lực để quả cầu lửa bị kéo xuốngrb.drag = 10; // Điều chỉnh giá trị tùy thuộc vào tốc độ mà bạn muốn

            // Xác định hướng nhân vật đang đối mặt (phải hoặc trái)
            Vector2 direction = transform.localScale.x > 0 ? Vector2.right : Vector2.left;

            // Gán vận tốc cho quả cầu lửa để nó bay ra xa
            rb.velocity = new Vector2(direction.x * fireballSpeed, rb.velocity.y); // Tốc độ theo phương x
        }
    }





}
