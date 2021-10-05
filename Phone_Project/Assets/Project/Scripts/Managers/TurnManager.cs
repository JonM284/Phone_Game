using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Project.Scripts.Enemy;
using Project.Scripts.Player;

namespace Project.Scripts.Managers
{
    public class TurnManager : MonoBehaviour
    {
        public enum Turn_Order
        {
            INTRO,
            PLAYER_TURN,
            ENEMY_TURN,
            OTHER,
            END
        }

        public Turn_Order currentTurn = Turn_Order.INTRO;

        [SerializeField] private PlayerBehavior player;
        [SerializeField] private List<EnemyBehavior> enemies;

        private bool enemiesFinishTurn = false;


        private bool playerIsAlive = true;

        // Start is called before the first frame update
        void Start()
        {
            InitilizeGame();
        }

        private void InitilizeGame()
        {
            StartCoroutine(RunIntro());
        }

        public void Update()
        {
            for (int i = 0; i < enemies.Count; i++)
            {
                enemies[i].EnemyUpdate();
            }
        }

        private void DoEnemyAttacks()
        {
            
        }

        public void ChangeTurn()
        {
            switch (currentTurn)
            {
                case Turn_Order.INTRO:
                    StartCoroutine(RunIntro());
                    break;
                case Turn_Order.PLAYER_TURN:
                    StartCoroutine(PlayerTurn());
                    break;
                case Turn_Order.ENEMY_TURN:
                    StartCoroutine(EnemiesTurn());
                    break;
                case Turn_Order.END:
                    StartCoroutine(EndLevel());
                    break;
                case Turn_Order.OTHER:
                    StartCoroutine(DoOtherState());
                    break;
                default:

                    break;
            }
        }

        public void SetPlayerDead(bool _alive)
        {
            playerIsAlive = _alive;
        }

        /// <summary>
        /// Initial text and such for the beginning of each level.
        /// </summary>
        /// <returns></returns>
        private IEnumerator RunIntro()
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(PlayerTurn());
        }

        /// <summary>
        /// If the player is alive and the enemies finish their turn
        /// </summary>
        /// <returns></returns>
        private IEnumerator PlayerTurn()
        {
            yield return new WaitUntil(() => player.currentState != PlayerBehavior.PlayerTurnState.DO_TURN || !player.m_playerIsAlive || enemies.Count <= 0);
            if (playerIsAlive && enemies.Count > 0) StartCoroutine(EnemiesTurn());
            else EndLevel();
        }

        /// <summary>
        /// If the player is alive and finishes their turn and there are enemies
        /// </summary>
        /// <returns></returns>
        private IEnumerator EnemiesTurn()
        {
            yield return new WaitUntil(() => !player.m_playerIsAlive || enemies.Count <= 0);
            if (playerIsAlive && enemies.Count > 0) StartCoroutine(PlayerTurn());
            else EndLevel();
        }

        /// <summary>
        /// If there is another process that needs to run during the course of a round, do this first THEN proceed to the next state.
        /// </summary>
        /// <returns></returns>
        private IEnumerator DoOtherState()
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(PlayerTurn());
        }


        /// <summary>
        /// If the player is alive, end this level and move on to the next level.
        /// If the player is dead, restart from the beginning.
        /// </summary>
        /// <returns></returns>
        private IEnumerator EndLevel()
        {
            yield return new WaitForSeconds(1f);
            StartCoroutine(PlayerTurn());
        }


    }
}

