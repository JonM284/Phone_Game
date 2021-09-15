using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{

    public enum EnemyState
    {
        IDLE,
        WALKING,
        ATTACKING
    }

    public EnemyState currentState = EnemyState.WALKING;

    [SerializeField]
    private float movementRange = 1.5f;

    [SerializeField]
    private float pauseTimerMax;

    [SerializeField]
    private float moveSpeed;

    private Vector2 m_startPos, m_Walk_Position;

    private Vector3 moveDir;

    private float m_pauseTimerCurrent, m_pauseCurrentMax;

    private bool m_isAlive = true;

    private Rigidbody2D rb;

    /// <summary>
    /// Call this to set up the enemy 
    /// </summary>
    public void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        m_startPos = transform.position;
        m_Walk_Position = NewPosition();
        m_pauseCurrentMax = NewRandomWaitTime();
    }

    /// <summary>
    /// Enemy Upda
    /// </summary>
    void Update()
    {
        if (m_isAlive)
        {
            if (currentState == EnemyState.WALKING)
            {
                if (Vector2.Distance(transform.position, m_Walk_Position) > 0.005)
                {
                    EnemyInputs();
                    MoveEnemy();
                }
                if (Vector2.Distance(transform.position, m_Walk_Position) <= 0.005)
                {
                    SetNewState(EnemyState.IDLE);
                }
            }
            
            if (currentState == EnemyState.IDLE) {
                if (m_pauseTimerCurrent < m_pauseCurrentMax)
                {
                    m_pauseTimerCurrent += Time.deltaTime;
                }
                if (m_pauseTimerCurrent >= m_pauseCurrentMax)
                {
                    ResetPause();
                }
            }
        }
    }

    private void EnemyInputs()
    {
        Debug.Log("Checking Inputs");
        moveDir = new Vector3(m_Walk_Position.x - transform.position.x, m_Walk_Position.y - transform.position.y, 0) * moveSpeed;
    }

    private void MoveEnemy()
    {
        Debug.Log("Moving");
        rb.MovePosition(transform.position + moveDir * Time.deltaTime);
    }

    private void SetNewState(EnemyState _state)
    {
        currentState = _state;
        if (currentState == EnemyState.IDLE)
        {
            m_pauseCurrentMax = NewRandomWaitTime();
        }else if (currentState == EnemyState.WALKING)
        {
            m_Walk_Position = NewPosition();
        }
    }

    private void ResetPause()
    {
        SetNewState(EnemyState.WALKING);
        m_pauseTimerCurrent = 0;
        m_pauseCurrentMax = NewRandomWaitTime();
    }

    private float NewRandomWaitTime()
    {
        return Random.Range(0.05f, pauseTimerMax);
    }

    private Vector2 NewPosition()
    {
        return m_startPos + (Random.insideUnitCircle * movementRange);
    }

    
}
