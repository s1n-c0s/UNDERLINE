using UnityEngine;

public class SamuraiMovement : MonoBehaviour
{
    public float moveSpeed = 5.0f;
    public float stopDuration = 5.0f;

    [SerializeField] private Vector3 startPoint = new Vector3(0.0f, 0.0f, 0.0f);
    [SerializeField] private Vector3 endPoint = new Vector3(0.0f, 0.0f, 0.0f);

    private Vector3 target;
    private bool isMoving = true;

    public bool canOpenKatana = false;
    public GameObject katana;

    private void Start()
    {
        target = endPoint; // เริ่มจากจุดสุดท้าย
    }

    private void Update()
    {
        if (isMoving)
        {
            // ย้ายศัตรูไปที่เป้าหมาย (เคลื่อนที่ในแกน X และ Z)
            Vector3 newPosition = new Vector3(target.x, transform.position.y, target.z); // เปลี่ยนที่ตำแหน่ง Z เป็น target.z
            transform.position = Vector3.MoveTowards(transform.position, newPosition, moveSpeed * Time.deltaTime);

            // เมื่อ Samurai ถึงเป้าหมายแล้วหยุด
            if (transform.position == newPosition)
            {
                // หยุดเคลื่อนที่ไป 2 วินาที
                isMoving = false;
                OpenKatana();
                StartCoroutine(WaitAndRotate());
            }
        }
    }

    private System.Collections.IEnumerator WaitAndRotate()
    {
        // รอเวลาหยุด
        yield return new WaitForSeconds(stopDuration);

        // เปลี่ยนเป้าหมาย
        if (target == startPoint)
            target = endPoint;
        else
            target = startPoint;

        // หมุน 180 องศาเมื่อเดินกลับที่จุดเริ่มต้นหรือจุดสุดท้าย
        transform.Rotate(Vector3.up, 180.0f);

        // รีเซ็ตเพื่อเริ่มเคลื่อนที่ใหม่
        isMoving = true;

        // ปิด Katana เมื่อเริ่มเคลื่อนที่อีกครั้ง
        CloseKatana();
    }

    public void OpenKatana()
    {
        canOpenKatana = true;
        katana.SetActive(canOpenKatana);
    }

    public void CloseKatana()
    {
        canOpenKatana = false;
        katana.SetActive(canOpenKatana);
    }
}
