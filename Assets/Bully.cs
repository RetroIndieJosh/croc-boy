using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bully : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 6f;
    private Rigidbody2D m_body = null;

    private void Awake() {
        m_body = GetComponent<Rigidbody2D>();
    }

    void Start() {
        m_body.velocity = Vector2.right * m_moveSpeed;
    }
}
