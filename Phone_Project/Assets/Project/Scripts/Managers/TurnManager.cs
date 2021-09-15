using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

        // Start is called before the first frame update
        void Start()
        {
            InitilizeGame();
        }

        private void InitilizeGame()
        {
            StartCoroutine(RunIntro());
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

        private IEnumerator RunIntro()
        {
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator PlayerTurn()
        {
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator EnemiesTurn()
        {
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator DoOtherState()
        {
            yield return new WaitForSeconds(1f);
        }

        private IEnumerator EndLevel()
        {
            yield return new WaitForSeconds(1f);
        }


    }
}

