using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI waveText;
    private PauseHandler PH;
    public int score;
    public int level = 1;
    public int IncreaseLevelEveryXSeconds = 10;
    public int rankEveryXPoints = 1000;
    private PlaySoundOneShot playSound;
    int[] rankThresholds = {
    500,    // Rank 1
    5000,   // Rank 2
    10000,  // Rank 3
    50000,  // Rank 4
    75000,  // Rank 5
    100000, // Rank 6
    150000, // Rank 7
    175000, // Rank 8
    250000, // Rank 9
    500000  // Rank 10
};
    private HashSet<int> achievedRanks = new HashSet<int>(); // To keep track of achieved ranks
    // Start is called before the first frame update
    void Start()
    {
        PH = GetComponent<PauseHandler>();
        StartCoroutine(WaitForEventManager());
        Invoke("StartLevelTimer", .5f);
        playSound = GetComponent<PlaySoundOneShot>();
    }

    public void StartLevelTimer()
    {
        StartCoroutine(LevelIncrease());
    }

    IEnumerator LevelIncrease()
    {
        while (true)
        {
            if (!PH.isPaused)
            {
                yield return new WaitForSeconds(IncreaseLevelEveryXSeconds);
                level++;
                EventManager.TriggerEvent("levelUpdate", new Dictionary<string, object> { { "level", level } });
                waveText.text = level.ToString();
                AudioManager.Instance.PlaySFXOneShot(playSound.SoundFXName);
            }
            else
            {
                yield return null;
            }
        }
    }


    IEnumerator WaitForEventManager()
    {
        while (EventManager.Instance == null)
            yield return null;

        EventManager.StartListening("ScoreUpdate", ScoreUpdate);
    }

    public void ScoreUpdate(Dictionary<string, object> obj)
    {
        score += (int)obj["score"] * level;
        //convert score to 00000 format
        scoreText.text = score.ToString("00000");
        // Check if the score crosses any rankEveryXPoints threshold and the rank hasn't been achieved yet
        for (int rank = 1; rank <= 10; rank++) // Assuming there are 10 ranks
        {

            if (score >= rankThresholds[rank - 1] && !achievedRanks.Contains(rank))
            {
                achievedRanks.Add(rank); // Mark the rank as achieved
                EventManager.TriggerEvent("RankUpdate", null);
            }
        }

    }


    private void OnDestroy()
    {
        EventManager.StopListening("ScoreUpdate", ScoreUpdate);
    }

    private void OnDisable()
    {
        EventManager.StopListening("ScoreUpdate", ScoreUpdate);
    }


    // Update is called once per frame
    void Update()
    {

    }
}
