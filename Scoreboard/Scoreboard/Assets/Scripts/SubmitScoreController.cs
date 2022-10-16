using LootLocker.Requests;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

using System.Linq;
using System;
public class SubmitScoreController : MonoBehaviour
{
    private TMP_InputField playerIDInput;
    private TMP_InputField playerScoreInput;
    [SerializeField]private int ID;
    private Button submitButton;

    
    private void Awake()
    {
        playerIDInput = GetComponentsInChildren<TMP_InputField>()[0];
        playerScoreInput = GetComponentsInChildren<TMP_InputField>()[1];
        submitButton= GetComponentInChildren<Button>();
    }
  
    public void SubmitSocre()
    {
        LootLockerSDKManager.SubmitScore(playerIDInput.text, int.Parse(playerScoreInput.text), ID, (response) => {

            if (response.success)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Failure");
            }

        });
    }

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

    public void GetScores()
    {
        LootLockerSDKManager.GetScoreList(ID,1000,(response) => {

            if (response.success)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Failure");
            }

        });
    }

    public void AddRandomScores(int amount)
    {
        for(int i = 0; i < amount; i++)
        {
            SubmitRandomScore();
        }
    }
    private void SubmitRandomScore()
    {
        string randomUser = RandomString(8);
        int randomScore = RandomInt();

        LootLockerSDKManager.SubmitScore(randomUser, randomScore, ID, (response) => {

            if (response.success)
            {
                Debug.Log("Success");
            }
            else
            {
                Debug.Log("Failure");
            }

        });
    }

    private System.Random random = new System.Random();
    private string RandomString(int length)
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        return new string(Enumerable.Repeat(chars, length)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
     
    private int RandomInt()
    {
        return UnityEngine.Random.Range(1, 9999);
    }

}
