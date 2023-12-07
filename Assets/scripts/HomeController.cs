using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeController : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Projectile"))
        {
            Debug.Log("Home is under attack!");
            // Thêm logic xử lý khi home bị tấn công ở đây.
        }
    }
}

