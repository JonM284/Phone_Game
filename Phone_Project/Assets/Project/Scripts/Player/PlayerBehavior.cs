using Project.Scripts.Managers;
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
            FINISH_TURN,
            DEFEND,
            END
        }

        public PlayerTurnState currentState;

        [SerializeField] private int playerHealth;
        [SerializeField] private int playerStamina;

        [SerializeField] private float swipeDistance;

        [SerializeField] private GameObject defenseObject;
        [SerializeField] private float defenseObjectDist;

        [SerializeField] private float defenseWindowTime;
        [SerializeField] private float defenseCooldownMax;
        [SerializeField] private float currentDefenseCooldownTimer;
        [SerializeField] private bool canDefend = false;

        [SerializeField] private int maxHealth;
        [SerializeField] private int currentHealth;

        private bool fingerDown = false;
        public bool m_playerIsAlive = true;

        private Vector2 initialPosition;

        private TurnManager turnManager;

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

        /// <summary>
        /// Add / subtract from the current amount of health the player has.
        /// </summary>
        /// <param name="_amount">positive = heal, negative = damage</param>
        public void ChangeCurrentHealth(int _amount)
        {
            currentHealth += _amount;
            if (currentHealth <= 0) OnPlayerDeath();
        }

        /// <summary>
        /// Tell manager that the player has died.
        /// </summary>
        private void OnPlayerDeath()
        {
            turnManager.SetPlayerDead(true);
        }


        /// <summary>
        /// Add / subtract from the MAX health of the player. (for upgrade purposes)
        /// </summary>
        /// <param name="_amount">Amount to add or subtract from max health stat</param>
        public void ChangeMaxHealth(int _amount)
        {
            maxHealth += _amount;
        }

        /// <summary>
        /// Set the position of the defending object
        /// </summary>
        /// <param name="_horizontal">Is the player swiping horizontally?</param>
        /// <param name="_offset">Distance from the player</param>
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
