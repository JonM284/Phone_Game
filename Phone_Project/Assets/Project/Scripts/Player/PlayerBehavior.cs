using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Project.Scripts.Player
{
    public class PlayerBehavior : MonoBehaviour
    {

        public enum PlayerTurnState
        {
            IDLE,
            DO_TURN,
            DEFEND,
            END
        }

        [SerializeField] private PlayerTurnState currentState;

        [SerializeField] private int playerHealth;
        [SerializeField] private int playerStamina;

        [SerializeField] private float swipeDistance;

        [SerializeField] private GameObject defenseObject;
        [SerializeField] private float defenseObjectDist;

        [SerializeField] private float defenseWindowTime;
        [SerializeField] private float defenseCooldownMax;
        [SerializeField] private float currentDefenseCooldownTimer;
        [SerializeField] private bool canDefend = false;

        private bool fingerDown = false;
        private bool m_playerIsAlive = true;

        private Vector2 initialPosition;

        public void InitializePlayerTurn()
        {
            currentState = PlayerTurnState.DO_TURN;
        }

        private void Update()
        {
            if (m_playerIsAlive && currentState == PlayerTurnState.DO_TURN)
            {
                CheckPlayerInput();
            }
        }

        private void CheckPlayerInput()
        {
            if (canDefend) {
                if (!fingerDown && Input.GetMouseButtonDown(0))
                {
                    fingerDown = true;
                    initialPosition = Input.mousePosition;
                }

                if (fingerDown)
                {
                    if (Input.mousePosition.y >= initialPosition.y + swipeDistance)
                    {
                        SetDefenseObject(false, defenseObjectDist);
                        Debug.Log("Swiped up");
                    }
                    else if (Input.mousePosition.y <= initialPosition.y - swipeDistance)
                    {
                        SetDefenseObject(false, -defenseObjectDist);
                        Debug.Log("Swiped down");
                    }
                    else if (Input.mousePosition.x >= initialPosition.x + swipeDistance)
                    {
                        SetDefenseObject(true, defenseObjectDist);
                        Debug.Log("Swiped right");
                    }
                    else if (Input.mousePosition.x <= initialPosition.x - swipeDistance)
                    {
                        SetDefenseObject(true, -defenseObjectDist);
                        Debug.Log("Swiped left");
                    }
                }

                if (fingerDown && Input.GetMouseButtonUp(0))
                {
                    fingerDown = false;
                }
            }else if (!canDefend && currentDefenseCooldownTimer < defenseCooldownMax)
            {
                currentDefenseCooldownTimer += Time.deltaTime;
            }


            if (!canDefend && currentDefenseCooldownTimer >= defenseCooldownMax)
            {
                canDefend = true;
                currentDefenseCooldownTimer = 0;
            }
        }


        private void SetDefenseObject(bool _horizontal, float _offset)
        {
            fingerDown = false;
            canDefend = false;
            defenseObject.SetActive(true);
            defenseObject.transform.position = _horizontal ? new Vector2(transform.position.x + _offset, transform.position.y) 
                : new Vector2(transform.position.x, transform.position.y + _offset);
            StartCoroutine(DefendWindowTiming(defenseWindowTime));
        }

        private IEnumerator DefendWindowTiming(float _time)
        {
            yield return new WaitForSeconds(_time);
            defenseObject.SetActive(false);
        }

        private void EndPlayerTurn()
        {

        }

    }
}
