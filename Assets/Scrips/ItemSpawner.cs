using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawner : MonoBehaviour
{
    [SerializeField]
    private float dropInterval = 3.0f;
    [SerializeField]
    private GameObject itemPrefab_Health;
    [SerializeField]
    private GameObject itemPrefab_Ammo;
    [SerializeField]
    private GameObject itemPrefab_Shield;
    [SerializeField]
    private GameObject itemPrefab_PowerUp;

    private List<GameObject> itemPool;
    [SerializeField]
    private int maxItemsOnScreen = 10;
    private int activeItemCount;

    float screenWidth;

    void Start()
    {
        screenWidth = Camera.main.orthographicSize * Camera.main.aspect * 2;

        itemPool = new List<GameObject>();

        // Khởi tạo pool với 10 items cho mỗi loại và thêm Rigidbody2D
        for (int i = 0; i < maxItemsOnScreen; i++)
        {
            GameObject healthItem = Instantiate(itemPrefab_Health);
            SetupItem(healthItem);
            itemPool.Add(healthItem);

            GameObject ammoItem = Instantiate(itemPrefab_Ammo);
            SetupItem(ammoItem);
            itemPool.Add(ammoItem);

            GameObject shieldItem = Instantiate(itemPrefab_Shield);
            SetupItem(shieldItem);
            itemPool.Add(shieldItem);

            GameObject powerUpItem = Instantiate(itemPrefab_PowerUp);
            SetupItem(powerUpItem);
            itemPool.Add(powerUpItem);
        }

        StartCoroutine(DropItemRoutine());
    }

    // Hàm setup cho mỗi item
    private void SetupItem(GameObject item)
    {
        // Thêm và cấu hình Rigidbody2D nếu chưa có
        Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            rb = item.AddComponent<Rigidbody2D>();
        }
        rb.gravityScale = 0; // Tắt gravity
        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Ngăn item xoay
        item.SetActive(false);
    }

    void Update()
    {
        activeItemCount = 0;
        foreach (var item in itemPool)
        {
            if (item.activeInHierarchy) activeItemCount++;
        }
    }

    IEnumerator DropItemRoutine()
    {
        while (true)
        {
            if (activeItemCount < maxItemsOnScreen)
            {
                DropItem();
            }
            yield return new WaitForSeconds(dropInterval);
        }
    }

    void DropItem()
    {
        List<GameObject> inactiveItems = itemPool.FindAll(item => !item.activeInHierarchy);

        if (inactiveItems.Count > 0)
        {
            GameObject itemToDrop = null;

            float dropChance = Random.Range(0f, 1f);

            if (dropChance <= 0.25f)
            {
                itemToDrop = inactiveItems.Find(item => item.CompareTag("HalfHealthItem"));
            }
            //else if (dropChance <= 0.5f)
            //{
            //    itemToDrop = inactiveItems.Find(item => item.CompareTag("HealthItem"));
            //}
            //else if (dropChance <= 0.75f)
            //{
            //    itemToDrop = inactiveItems.Find(item => item.CompareTag("PoisonItem"));
            //}
            //else
            //{
            //    itemToDrop = inactiveItems.Find(item => item.CompareTag("EnergyItem"));
            //}

            if (itemToDrop != null)
            {
                float randomX = Random.Range(-screenWidth / 2, screenWidth / 2);
                float fixedY = 4.5f;

                itemToDrop.transform.position = new Vector3(randomX, fixedY, 0);
                itemToDrop.SetActive(true);

                // Thêm chuyển động rơi sử dụng Rigidbody2D
                Rigidbody2D rb = itemToDrop.GetComponent<Rigidbody2D>();
                float dropSpeed = Random.Range(2f, 5f);
                rb.velocity = new Vector2(0, -dropSpeed); // Set vận tốc rơi xuống

                StartCoroutine(DeactivateItemAfterTime(itemToDrop, 10f));
            }
        }
    }

    IEnumerator DeactivateItemAfterTime(GameObject item, float time)
    {
        yield return new WaitForSeconds(time);
        if (item != null && item.activeInHierarchy)
        {
            Rigidbody2D rb = item.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero; // Reset vận tốc
            item.SetActive(false);
        }
    }
}