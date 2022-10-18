
/*Summary
 * This script is responsible for submitting
 * scores to leaderboard
 */
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using System;

public class SubmitScoreController : MonoBehaviour
{
    private TMP_InputField playerIDInput;
    private TMP_InputField playerScoreInput;
    private Button submitButton;
    private LeaderboardController leaderboardController;
    private Action onScoreSubmit;


    private void Awake()
    {
        playerIDInput = GetComponentsInChildren<TMP_InputField>()[0];
        playerScoreInput = GetComponentsInChildren<TMP_InputField>()[1];
        submitButton= GetComponentInChildren<Button>();
        leaderboardController = FindObjectOfType<LeaderboardController>();
    }
  
    public async void SubmitSocre()
    {
        await leaderboardController.SubmitScore(playerIDInput.text, int.Parse(playerScoreInput.text));
        onScoreSubmit?.Invoke();
    }

    //Disable Sumbit button if the inputs are empty
    public void CheckIfInputEmpty()
    {
        if (playerIDInput.text.Length > 0 && playerScoreInput.text.Length > 0)
        {
            submitButton.interactable = true;
        }
        else
        {
            submitButton.interactable = false;
        }
    }

   public void AddOnSubmitAction(Action action)
    {
        onScoreSubmit += action;
    }

    public void RemoveOnSubmitAction(Action action)
    {
        onScoreSubmit -= action;
    }

}
