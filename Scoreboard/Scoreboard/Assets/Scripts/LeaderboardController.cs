
using LootLocker.Requests;
using UnityEngine;
using System;
using System.Linq;
using System.Threading.Tasks;
public class LeaderboardController : MonoBehaviour
{
    [SerializeField] private int ID;
    private Action onSessionStarted;
    void Start()
    {
        StartSession();
    }

    private async void StartSession()
    {
        bool done = false;
        LootLockerSDKManager.StartGuestSession((response) =>
        {
            if (response.success)
            {
                Debug.Log("Success");
                done = true;

            }
            else
            {
                Debug.Log("Couldnt start session");
                done = true;
                StartSession();
            }

        });

        while (!done)
        {
            await Task.Yield();
        }

        onSessionStarted?.Invoke();
    }

    public async Task SubmitScore(string username,int score)
    {
        bool done = false;
        LootLockerSDKManager.SubmitScore(username, score, ID, (response) => {

            if (response.success)
            {
                Debug.Log("Success");
                done = true;
            }
            else
            {
                Debug.Log("Failure");
                done = true;
            }

        });

        while (!done)
        {
            await Task.Yield();
        }
    }
    public async Task<LootLockerLeaderboardMember[]> GetScores(int startRank,int amount)
    {
        bool done = false;
        LootLockerLeaderboardMember[] lootLockerLeaderboardMembers=new LootLockerLeaderboardMember[0];
        LootLockerSDKManager.GetScoreList(ID,amount,startRank, (response) =>
        {
            if (response.success)
            {                
                Debug.Log("Successful");
                lootLockerLeaderboardMembers = response.items;
                done = true;
            }
            else
            {
                Debug.Log("failed: " + response.Error);
                done = true;
            }
        });

        while (!done)
        {
            await Task.Yield();
        }

        return lootLockerLeaderboardMembers;
    }  
     public async Task<int> Test()
    {
        bool done = false;
        int totalRanks = 0;
        LootLockerSDKManager.GetScoreList(ID,0, -1, (response) =>
          {
              if (response.success)
              {
                  Debug.Log("Successful");
                  totalRanks = response.pagination.total;
                  done = true;
              }
              else
              {
                  Debug.Log("failed: " + response.Error);
                  done = true;
              };
          });

        while (!done)
        {
            await Task.Yield();
        }

        return totalRanks;
    }
   
    private async Task SubmitRandomScore()
    {
        string randomUser = RandomString(8);
        int randomScore = RandomInt();
        bool done = false;
        LootLockerSDKManager.SubmitScore(randomUser, randomScore, ID, (response) => {

            if (response.success)
            {
                Debug.Log("Success");
                done = true;
            }
            else
            {
                Debug.Log("Failure "+response.Error);
                done = true;

            }

        });

        while (!done)
        {
            await Task.Yield();
        }
    }

    public void AddOnSessionStartedAction(Action action)
    {
        onSessionStarted += action;
    }

    public void RemoveOnSessionStartedAction(Action action)
    {
        onSessionStarted -= action;
    }

    //Used for testing
    public async void AddRandomScores(int amount)
    {
        for (int i = 0; i < amount; i++)
        {
             await SubmitRandomScore();
        }
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
