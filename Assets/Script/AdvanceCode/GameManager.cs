using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace AdvanceCode
{
    public class GameManager : MonoBehaviour
    {
        [SerializeField] private int maxLife = 3;
        private int currentLife = 0;
        public int CurrentLife => currentLife;
        private int gems = 0;
        public int Gems => gems;
        private int maxGemsAmount = 9;
        private int score = 0;
        [SerializeField] private Text Text_Life, Text_Gems, Text_Score, Text_GameOver_Score;

        [SerializeField] private float ShowScoreTime = 1f;
        [SerializeField] private float ResetTime = 5f;

        private Health_Player playerHealth;
        private Vector3 RevivalPos;

        private static GameManager instance;
        public static GameManager Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<GameManager>();
                    if (instance == null)
                    {
                        GameObject singletonObject = new GameObject("GameManager");
                        instance = singletonObject.AddComponent<GameManager>();
                    }
                }
                return instance;
            }
        }

        public void SetRevivalPos(Vector3 pos)
        {
            RevivalPos = pos;
        }

        void Start()
        {
            playerHealth = GameObject.FindWithTag("Player").GetComponent<Health_Player>();
            playerHealth.HitReceived += PlayerOnHit;
            playerHealth.DeadReceived += PlayerOnDead;
            playerHealth.RevivalReceived += PlayerOnRevival;
            Text_GameOver_Score.transform.parent.gameObject.SetActive(false);

            currentLife = maxLife;
            Text_Life.text = currentLife.ToString();
            Text_Gems.text = Text_Score.text = "0";
        }

        private void PlayerOnHit(object sender, EventArgs e)
        {
            SubGems();
        }

        private void PlayerOnDead(object sender, EventArgs e)
        {
            SubtractLife();
        }

        private void PlayerOnRevival(object sender, EventArgs e)
        {
            playerHealth.gameObject.transform.position = RevivalPos;
        }

        public void SubtractLife()
        {
            currentLife -= 1;
            if (currentLife <= 0)
            {
                currentLife = 0;
                GameOver();
            }
            Text_Life.text = currentLife.ToString();
        }

        public void AddGems()
        {
            SettingGems(1);
        }

        public void SubGems()
        {
            SettingGems(-1);
        }

        private void SettingGems(int amount)
        {
            gems += amount;
            if (gems > maxGemsAmount) gems = maxGemsAmount;
            if (gems < 0) gems = 0;
            Text_Gems.text = gems.ToString();
        }

        public void AddScore(int amount)
        {
            score += (1 + gems) * amount;
            Text_Score.text = score.ToString();
        }

        public void GameOver()
        {
            Text_GameOver_Score.transform.parent.gameObject.SetActive(true);
            Text_GameOver_Score.text = score.ToString();
            Text_Life.transform.parent.gameObject.SetActive(false);
            Text_Gems.transform.parent.gameObject.SetActive(false);
            Text_Score.transform.parent.gameObject.SetActive(false);

            StartCoroutine(AfterReset());
        }

        IEnumerator AfterReset()
        {
            yield return new WaitForSeconds(ShowScoreTime);

            yield return new WaitForSeconds(ResetTime);

            SceneManager.LoadScene(0);
        }
    }
}

