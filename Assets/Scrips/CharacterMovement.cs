using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    public enum CharacterType { Character1, Character2 }
    public CharacterType characterType; // Set this in the inspector for each character

    [SerializeField]
	float moveSpeed = 5f;
	[SerializeField]
	float jumpForce = 5f;

	

	Rigidbody2D Rigidbody2D;
	Animator animator;
	bool isAttacking;
	bool isGrounded; // To prevent infinite jumping
	bool isDashing = false;


	public Animator object1Animator; // Animator của object 1
	public Animator object2Animator; // Animator của object 2
	public Animator object3Animator; // Animator của object 3
	public Animator object4Animator; // Animator của object 3

	[SerializeField]
	GameObject[] objects; // Mảng chứa 5 đối tượng
	[SerializeField]
	GameObject[] objects2; // Danh sách thứ hai
	[SerializeField]
	float delayBetweenObjects = 1f; // Khoảng thời gian giữa mỗi đối tượng

	[SerializeField]
	GameObject bulletPrefab; // Prefab của viên đạn
	[SerializeField]
	Transform firePoint; // Điểm xuất phát của viên đạn
	[SerializeField]
	float bulletSpeed = 10f; // Tốc độ của viên đạn
	public Animator bulletAnimator; // Animator của viên đạn

	private GameObject currentBullet; // Để giữ viên đạn hiện tại

	float doubleTapTime;
	KeyCode lastKeyCode;

	public float dashSpeed;
	private float dashCount;
	public float startDashCount;
	private int side;

    bool isTakingDamage = false; // Biến theo dõi khi nhân vật bị đánh

    // Các trạng thái của việc nhận đòn
    bool tookNormalHit = false; // Biến theo dõi nếu bị đánh bởi đòn thường
    bool tookSpecialHit = false; // Biến theo dõi nếu bị đánh bởi đòn khác

    public HealthAndMana healthAndMana; // Reference to the HealthAndMana script

    [SerializeField]
    float normalHitDamage = 10f; // Damage for normal hits
    [SerializeField]
    float specialHitDamage = 25f; // Damage for special hits
    [SerializeField] float manaCostAttack7 = 10f;
    [SerializeField] float manaCostAttack2 = 15f;
    [SerializeField] float manaCostAttack3 = 20f;
    [SerializeField] float manaCostAttack4 = 25f;
    [SerializeField] float manaCostAttack5 = 30f;
    [SerializeField] float manaCostAttack6 = 35f;

    [SerializeField] private GifACE gifPlayer;
    public Canvas attackCanvas; // Reference to your canvas

    public GameObject ultimatePrefab;
    public Transform ultimatePoint;
    public Animator ultimateAnimator;
    private GameObject currentUltimate;



    // Start is called before the first frame update
    void Start()
	{
		Rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
		animator = gameObject.GetComponent<Animator>();
        healthAndMana = GameObject.FindObjectOfType<HealthAndMana>();
        dashCount = startDashCount;
        //attackCanvas.gameObject.SetActive(false); // Hide the canvas initially

    }

    // Update is called once per frame
    void Update()
	{
		HandleMovement();
		HandleAttack();
		HandleDash(); // Xử lý logic dash
        HandleDamage(); // Xử lý logic nhận đòn


    }

    // Hàm xử lý khi nhân vật nhận đòn
    void HandleDamage()
    {
        if (isTakingDamage)
        {
            // Normal hit
            if (tookNormalHit)
            {
                animator.SetTrigger("NormalHit");
                healthAndMana.TakeDamage(normalHitDamage); // Call to reduce health
                isTakingDamage = false;
            }
            // Special hit
            else if (tookSpecialHit)
            {
                animator.SetTrigger("SpecialHit");
                healthAndMana.TakeDamage(specialHitDamage); // Call to reduce health
                isTakingDamage = false;
            }
        }
    }

    // Method to handle movement with 'a', 'w', 'd' keys
    void HandleMovement()
	{
		if (!isAttacking)
		{
            if (characterType == CharacterType.Character1)
            {
			// Move left
			    if (Input.GetKey(KeyCode.A))
			    {
				    Rigidbody2D.velocity = new Vector2(-moveSpeed, Rigidbody2D.velocity.y);
				    animator.SetInteger("StateIndex", 6); // Optional: Set movement animation

				    // Flip character to face left
				    transform.localScale = new Vector3((float)-4.531328, (float)4.677096, 1); // Flip the character's X-axis
			    }
			    // Move right
			    else if (Input.GetKey(KeyCode.D))
			    {
				    Rigidbody2D.velocity = new Vector2(moveSpeed, Rigidbody2D.velocity.y);
				    animator.SetInteger("StateIndex", 6); // Optional: Set movement animation

				    // Flip character to face right
				    transform.localScale = new Vector3((float)4.531328, (float)4.677096, 1); // Set to default scale
			    }
			    else
			    {
				    // Stop moving
				    Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
				    animator.SetInteger("StateIndex", 0); // Idle state when not moving
			    }

			    // Jump (ensure the character is grounded to prevent double-jumping)
			    if (Input.GetKey(KeyCode.K) && isGrounded)
			    {
				    Rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
				    isGrounded = false; // Character is now in the air
				    animator.SetInteger("StateIndex",13); // Use a trigger for jump animation
			    }
            }
            else if (characterType == CharacterType.Character2)
            {
                // Move left
                if (Input.GetKey(KeyCode.LeftArrow))
                {
                    Rigidbody2D.velocity = new Vector2(-moveSpeed, Rigidbody2D.velocity.y);
                    animator.SetInteger("StateIndex", 6); // Optional: Set movement animation

                    // Flip character to face left
                    transform.localScale = new Vector3((float)-4.531328, (float)4.677096, 1); // Flip the character's X-axis
                }
                // Move right
                else if (Input.GetKey(KeyCode.RightArrow))
                {
                    Rigidbody2D.velocity = new Vector2(moveSpeed, Rigidbody2D.velocity.y);
                    animator.SetInteger("StateIndex", 6); // Optional: Set movement animation

                    // Flip character to face right
                    transform.localScale = new Vector3((float)4.531328, (float)4.677096, 1); // Set to default scale
                }
                else
                {
                    // Stop moving
                    Rigidbody2D.velocity = new Vector2(0, Rigidbody2D.velocity.y);
                    animator.SetInteger("StateIndex", 0); // Idle state when not moving
                }

                // Jump (ensure the character is grounded to prevent double-jumping)
                if (Input.GetKey(KeyCode.Alpha6) && isGrounded)
                {
                    Rigidbody2D.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
                    isGrounded = false; // Character is now in the air
                    animator.SetInteger("StateIndex", 13); // Use a trigger for jump animation
                }
            }
        }

	}

	void HandleAttack()
	{
        if (!isAttacking)
        {
            if (characterType== CharacterType.Character1)
            {
                // Attack 4 - requires S + U
                if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.U) && healthAndMana.HasEnoughMana(manaCostAttack4))
                {
                    animator.SetInteger("StateIndex", 4);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack4);
                }

                // Attack 6 - requires W + U
                else if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.U) && healthAndMana.HasEnoughMana(manaCostAttack6))
                {
                    animator.SetInteger("StateIndex", 7);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack6);
                }

                // Attack 2
                else if (Input.GetKey(KeyCode.W) && Input.GetKeyDown(KeyCode.J) && healthAndMana.HasEnoughMana(manaCostAttack2))
                {
                    animator.SetInteger("StateIndex", 2);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack2);
                }

                // Attack 3
                else if (Input.GetKeyDown(KeyCode.I) && healthAndMana.HasEnoughMana(manaCostAttack3))
                {
                    StartCoroutine(PlayGifThenAttackI());
                    //animator.SetInteger("StateIndex", 3);
                    //isAttacking = true;
                    //healthAndMana.UseMana(manaCostAttack2);


                }

                // Attack 5 - just U key
                else if (Input.GetKeyDown(KeyCode.U) && healthAndMana.HasEnoughMana(manaCostAttack5))
                {
                    animator.SetInteger("StateIndex", 5);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack5);
                }

                // Attack 7
                else if (Input.GetKey(KeyCode.S) && Input.GetKeyDown(KeyCode.J) && healthAndMana.HasEnoughMana(manaCostAttack7))
                {
                    animator.SetInteger("StateIndex", 8);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack7);
                }

                // Attack 1 - No mana cost for this attack
                else if (Input.GetKeyDown(KeyCode.J))
                {
                    animator.SetInteger("StateIndex", 1);
                    isAttacking = true;
                }
            }
            else if(characterType == CharacterType.Character2)
            {
                // Attack 4 - requires S + U
                if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Alpha4) && healthAndMana.HasEnoughMana(manaCostAttack4))
                {
                    animator.SetInteger("StateIndex", 4);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack4);
                }

                // Attack 6 - requires W + U
                else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.Alpha4) && healthAndMana.HasEnoughMana(manaCostAttack6))
                {
                    animator.SetInteger("StateIndex", 7);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack6);
                }

                // Attack 2
                else if (Input.GetKey(KeyCode.UpArrow) && Input.GetKeyDown(KeyCode.Alpha1) && healthAndMana.HasEnoughMana(manaCostAttack2))
                {
                    animator.SetInteger("StateIndex", 2);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack2);
                }

                // Attack 3
                else if (Input.GetKeyDown(KeyCode.Alpha5) && healthAndMana.HasEnoughMana(manaCostAttack3))
                {
                    StartCoroutine(PlayGifThenAttackI());
                    //animator.SetInteger("StateIndex", 3);
                    //isAttacking = true;
                    //healthAndMana.UseMana(manaCostAttack2);


                }

                // Attack 5 - just U key
                else if (Input.GetKeyDown(KeyCode.Alpha4) && healthAndMana.HasEnoughMana(manaCostAttack5))
                {
                    animator.SetInteger("StateIndex", 5);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack5);
                }

                // Attack 7
                else if (Input.GetKey(KeyCode.DownArrow) && Input.GetKeyDown(KeyCode.Alpha1) && healthAndMana.HasEnoughMana(manaCostAttack7))
                {
                    animator.SetInteger("StateIndex", 8);
                    isAttacking = true;
                    healthAndMana.UseMana(manaCostAttack7);
                }

                // Attack 1 - No mana cost for this attack
                else if (Input.GetKeyDown(KeyCode.Alpha1))
                {
                    animator.SetInteger("StateIndex", 1);
                    isAttacking = true;
                }
            }
            
        }


        // Reset StateIndex after the animation is done
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);
		if (isAttacking && stateInfo.normalizedTime >= 1f && stateInfo.IsTag("Attack"))
		{
			animator.SetInteger("StateIndex", 0); // Reset to idle state
			isAttacking = false; // Allow new inputs
		}
	}
    private IEnumerator PlayGifThenAttackI()
    {
        attackCanvas.gameObject.SetActive(true);
        // Phát GIF
        yield return StartCoroutine(gifPlayer.PlayGIF());
        animator.SetInteger("StateIndex", 3);
        isAttacking = true;
        healthAndMana.UseMana(manaCostAttack3);
    }

    void HandleDash()
    {
        // Kiểm tra nếu đang tấn công thì không dash
        if (isAttacking)
        {
            return; 
        }

        if (side == 0)
        {
            if (Input.GetKeyDown(KeyCode.A))
            {
                if (doubleTapTime > Time.time && lastKeyCode == KeyCode.A)
                {
                    side = 1;
                }
                else
                {
                    doubleTapTime = Time.time + 0.5f;
                }
                lastKeyCode = KeyCode.A;
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                if (doubleTapTime > Time.time && lastKeyCode == KeyCode.D)
                {
                    side = 2;
                }
                else
                {
                    doubleTapTime = Time.time + 0.5f;
                }
                lastKeyCode = KeyCode.D;
            }
        }
        else
        {
            if (dashCount <= 0)
            {
                side = 0;
                dashCount = startDashCount;
                Rigidbody2D.velocity = Vector2.zero;
            }
            else
            {
                dashCount -= Time.deltaTime;
                if (side == 1)
                {
                    Rigidbody2D.velocity = Vector2.left * dashSpeed;
                }
                else if (side == 2)
                {
                    Rigidbody2D.velocity = Vector2.right * dashSpeed;
                }
            }
        }
    }





    // To detect if the player is grounded (you need a ground detection mechanism)
    private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.gameObject.CompareTag("Ground"))
		{
			isGrounded = true;
			Debug.Log("Cham dat");
		}
        //if (collision.gameObject.CompareTag("Player2"))
        //{
        //    Debug.Log("Player1 hit Player2!");
        //}
    }

	void PlayAnimationForObject(Animator objectAnimator, string triggerName, float duration)
	{
		objectAnimator.gameObject.SetActive(true); // Ensure the GameObject is active
		objectAnimator.enabled = true; // Ensure the Animator is enabled

		objectAnimator.SetTrigger(triggerName);

		StartCoroutine(StopAnimationAfterTime(objectAnimator, duration));
	}

	// Coroutine to stop the animation after a given duration
	IEnumerator StopAnimationAfterTime(Animator objectAnimator, float duration)
	{
		yield return new WaitForSeconds(duration);

		objectAnimator.enabled = false;
		objectAnimator.gameObject.SetActive(false);
	}


	IEnumerator SpawnObjectsInOrder(GameObject[] objList)
	{
		for (int i = 0; i < objList.Length; i++)
		{
			objList[i].SetActive(true); // Kích hoạt đối tượng
			Animator anim = objList[i].GetComponent<Animator>(); // Lấy Animator của đối tượng

			// Khởi động coroutine để đợi animation kết thúc và tắt đối tượng
			StartCoroutine(WaitAndDisable(anim, objList[i]));

			// Chờ một khoảng thời gian trước khi kích hoạt đối tượng tiếp theo
			yield return new WaitForSeconds(delayBetweenObjects);
		}
	}

	IEnumerator WaitAndDisable(Animator animator, GameObject obj)
	{
		// Đợi cho đến khi animation hiện tại kết thúc
		while (animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
		{
			yield return null; // Chờ frame tiếp theo
		}

		obj.SetActive(false); // Tắt đối tượng sau khi animation kết thúc
	}




	void PlayObject1Animation()
	{
		PlayAnimationForObject(object1Animator, "FireEffect", 1f); // 3 giây
	}

	void PlayObject2Animation()
	{
		PlayAnimationForObject(object2Animator, "PillarOfFire", 1f); // 3 giây
	}

	void PlayObject3Animation()
	{
		PlayAnimationForObject(object3Animator, "PillarEffect", 1.5f); // 3 giây
	}
	void PlayObject4Animation()
	{
		PlayAnimationForObject(object4Animator, "Hiken", 1.5f); // 3 giây
	}
	void PlayListObject1Animation()
	{
		StartCoroutine(SpawnObjectsInOrder(objects));
	}
	void PlayListObject2Animation()
	{
		StartCoroutine(SpawnObjectsInOrder(objects2));
	}

	void CreateFireBullet()
	{
		// Tạo viên đạn tại vị trí firePoint
		currentBullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);

		// Lấy Rigidbody2D của viên đạn để đặt vận tốc
		Rigidbody2D bulletRb = currentBullet.GetComponent<Rigidbody2D>();

		// Đặt vận tốc cho viên đạn theo hướng nhân vật đang nhìn
		float direction = transform.localScale.x > 0 ? 1 : -1; // Xác định hướng dựa trên hướng nhân vật
		bulletRb.velocity = new Vector2(direction * bulletSpeed, 0);

		// Gán Animator cho viên đạn
		bulletAnimator = currentBullet.GetComponent<Animator>();

		// Kích hoạt trạng thái "create" cho Animator của viên đạn
		bulletAnimator.SetTrigger("Create");

		StartCoroutine(DelayBeforeHold());

	}

	// Hàm giữ viên đạn (trạng thái bay)
	void HoldFireBullet()
	{
		if (bulletAnimator != null)
		{
			// Kích hoạt trigger "hold" để viên đạn giữ trạng thái bay
			bulletAnimator.SetTrigger("Hold");
		}
	}
	IEnumerator DelayBeforeHold()
	{
		yield return new WaitForSeconds(0.5f); // Chờ khoảng thời gian được đặt

		HoldFireBullet(); // Chuyển sang trạng thái hold sau khi chờ
	}

    public void TakeDamage(bool isSpecialAttack)
    {
        // Kiểm tra loại đòn tấn công
        if (isSpecialAttack)
        {
            tookSpecialHit = true;  // Đòn tấn công đặc biệt
            tookNormalHit = false;  // Không phải đòn tấn công thường
        }
        else
        {
            tookNormalHit = true;   // Đòn tấn công thường
            tookSpecialHit = false; // Không phải đòn tấn công đặc biệt
        }

        isTakingDamage = true; // Nhân vật đang bị đánh
    }
    void CreateUltimateEffect()
    {
        currentUltimate = Instantiate(ultimatePrefab, ultimatePoint.position, ultimatePoint.rotation);

        float direction = transform.localScale.x > 0 ? 1 : -1;

        // Gán scale một lần với kích thước mong muốn
        //currentultimate.transform.localscale = new vector3(direction * 6.085109f, 1.571724f, 1);

        Animator effectAnimator = currentUltimate.GetComponent<Animator>();
        effectAnimator.SetTrigger("Ultimate");

        // Hủy object sau 3 giây
        Destroy(currentUltimate, 3f);
    }

}