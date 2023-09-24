﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody rb;
    private Vector3 startPosition;
    private Vector3 targetPosition;
    private bool isRunning = false; // เปลี่ยน jump เป็น run หรือพุ่งตัวในภาษาอังกฤษ
    private Collider playerCollider;

    public int runsRemaining = 3; // แก้ชื่อตัวแปรเป็น runsRemaining
    public float maxPower = 20f;
    public float powerMultiplier = 5f;
    public float stopThreshold = 0.1f;
    public float runningAngularDamping = 10f; // เปลี่ยน jumpAngularDamping เป็น runningAngularDamping
    public bool IsMoving { get; private set; }

    public LineRenderer _lineRenderer;
    public Transform launchPoint;
    public float launchSpeed = 10f;
    public int linePoints = 20;
    public float timeIntervalinPoints = 0.1f;
    public float maxDistance = 170f;
    private EnemyDetector enemyDetector;
    //private ScoreManager scoreManager;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
        startPosition = transform.position;
        targetPosition = startPosition;
        playerCollider = GetComponent<Collider>();
        enemyDetector = GetComponent<EnemyDetector>();
        //scoreManager = GetComponent<ScoreManager>();
    }

    void Update()
    {
        if (runsRemaining > 0 && !IsMoving)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (playerCollider.Raycast(ray, out hit, Mathf.Infinity))
                {
                    isRunning = true; // เปลี่ยน jump เป็น run หรือพุ่งตัวในภาษาอังกฤษ
                    startPosition = transform.position;
                }
            }

            if (isRunning)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                Plane tablePlane = new Plane(Vector3.up, startPosition);

                float hitDistance;
                if (tablePlane.Raycast(ray, out hitDistance))
                {
                    targetPosition = ray.GetPoint(hitDistance);
                }

                Debug.DrawLine(targetPosition, targetPosition + Vector3.up, Color.red);

                Vector3 lookDirection = startPosition - targetPosition;
                lookDirection.y = 0f;
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = targetRotation;

                rb.angularDrag = runningAngularDamping; // เปลี่ยน jumpAngularDamping เป็น runningAngularDamping

                DrawTrajectory();
                _lineRenderer.enabled = true;
            }

            if (Input.GetMouseButtonUp(0) && isRunning)
            {
                Vector3 runDirection = startPosition - targetPosition; // เปลี่ยน jumpDirection เป็น runDirection
                runDirection.y = 0;

                float power = Mathf.Clamp(runDirection.magnitude * powerMultiplier, 0f, maxPower);

                rb.AddForce(runDirection.normalized * power, ForceMode.Impulse);

                isRunning = false; // เปลี่ยน jump เป็น run หรือพุ่งตัวในภาษาอังกฤษ
                IsMoving = true;
                runsRemaining--;

                if (runsRemaining <= 0)
                {
                    isRunning = false; // เปลี่ยน jump เป็น run หรือพุ่งตัวในภาษาอังกฤษ
                }

                rb.angularDrag = 0f;
            }
        }
        else
        {
            if (rb.velocity.magnitude < stopThreshold)
            {
                IsMoving = false;
                rb.freezeRotation = true;
            }

            _lineRenderer.enabled = false;
        }
    }
    void DrawTrajectory()
    {
        Vector3 origin = launchPoint.position;
        Vector3 startVelocity1 = launchSpeed * launchPoint.forward;
        Vector3 startVelocity2 = -launchSpeed * launchPoint.forward * 6 / 2; // ปรับให้แกนปลายของ Line Renderer พุ่งออกไปข้างหน้า

        _lineRenderer.positionCount = linePoints * 2;

        float timeInterval = timeIntervalinPoints / linePoints;
        float time = 0;

        for (int i = 0; i < linePoints; i++)
        {
            var x1 = origin.x + startVelocity1.x * time;
            var y1 = origin.y + startVelocity1.y * time - 0.5f * Mathf.Abs(Physics.gravity.y) * time * time;
            var z1 = origin.z + startVelocity1.z * time;

            var x2 = origin.x + startVelocity2.x * time;
            var y2 = origin.y + startVelocity2.y * time - 0.5f * Mathf.Abs(Physics.gravity.y) * time * time;
            var z2 = origin.z + startVelocity2.z * time;

            Vector3 point1 = new Vector3(x1, y1, z1);
            Vector3 point2 = new Vector3(x2, y2, z2);

            // คำนวณระยะทางจากตัวละครไปยังจุดปลายทางที่ผู้เล่นลากเม้าส์
            float distanceToTarget = Vector3.Distance(origin, targetPosition);
            float distanceToPoint1 = Vector3.Distance(origin, point1);
            float distanceToPoint2 = Vector3.Distance(origin, point2);

            /*if (distanceToPoint1 > distanceToTarget)
            {
                // ตัดส่วนที่เกินระยะทางที่ผู้เล่นลากเม้าส์ออก
                point1 = origin + (point1 - origin).normalized * distanceToTarget;
            }*/

            if (distanceToPoint2 > distanceToTarget)
            {
                // ตัดส่วนที่เกินระยะทางที่ผู้เล่นลากเม้าส์ออก
                point2 = origin + (point2 - origin).normalized * distanceToTarget;

            }

            _lineRenderer.SetPosition(i * 2, point1);
            _lineRenderer.SetPosition(i * 2 + 1, point2);

            time += timeInterval;
        }
    }
}