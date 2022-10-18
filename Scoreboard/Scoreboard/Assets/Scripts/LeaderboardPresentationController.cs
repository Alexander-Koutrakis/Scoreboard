/*summary:
 * This script is repsonsible for presenting the leaderboard scores
 * It contains an Array of 10 game objects that each present the player rank
 * player name and player score 
 * Turn pages by setting the highest and lowest shown ranks
*/

    
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using System.Threading.Tasks;

public class LeaderboardPresentationController : MonoBehaviour
{
    private int highestShownRank=0;// highest Rank shown in board
    private int lowestShownRank;// lowest Rank shown in board
    private int ranksPerPage = 10;
    private int totalRanks = 0;
    private LeaderboardController leaderboardController;
    [SerializeField]private GameObject[] leaderboardMemberGOArray;

    private void Awake()
    {
        leaderboardController = FindObjectOfType<LeaderboardController>();
        SubmitScoreController submitScoreController = FindObjectOfType<SubmitScoreController>();
        submitScoreController.AddOnSubmitAction(RefreshPage);
        leaderboardController.AddOnSessionStartedAction(RefreshPage);
    }

    public void NextPage()
    {
        if (lowestShownRank < totalRanks-1)
        {
            highestShownRank = lowestShownRank + 1;
        }
        else
        {
            highestShownRank = 0;
        }

        RefreshPage();
    }
    public void PreviousPage()
    {
        if (highestShownRank > 0)
        {
            highestShownRank = highestShownRank - ranksPerPage;
        }
        else
        {
            int remainingRanks = totalRanks % ranksPerPage;
            highestShownRank = totalRanks - remainingRanks; ;
        }
        RefreshPage();
    }
    public async void RefreshPage()
    {
        LootLockerLeaderboardMember[] lootLockerLeaderboardMembers = await GetScores(highestShownRank);
        ShowScores(lootLockerLeaderboardMembers);
    }

    /*Get Scores starting on the highestShownRank
     * and find the lowest showingRank
     */
    private async Task<LootLockerLeaderboardMember[]> GetScores(int startingScore)
    {
        LootLockerLeaderboardMember[] lootLockerLeaderboardMembers = await leaderboardController.GetScores(startingScore, ranksPerPage);
        totalRanks = leaderboardController.Size;
        int shownRanks = lootLockerLeaderboardMembers.Length;
        lowestShownRank = startingScore + shownRanks-1;
        lowestShownRank = Mathf.Clamp(lowestShownRank, 0, totalRanks);
        return lootLockerLeaderboardMembers;
    }

    //Fill each leaderboardMemberGO with the leaderboard data
    private void ShowScores(LootLockerLeaderboardMember[] lootLockerLeaderboardMembers)
    {
        for(int i=0;i< leaderboardMemberGOArray.Length; i++)
        {
            if(i< lootLockerLeaderboardMembers.Length)
            {
                ShowMemberScore(lootLockerLeaderboardMembers[i], leaderboardMemberGOArray[i]);
                leaderboardMemberGOArray[i].SetActive(true);
            }
            else
            {
                leaderboardMemberGOArray[i].SetActive(false);
            }
        }
    }
    private void ShowMemberScore(LootLockerLeaderboardMember lootLockerLeaderboardMember,GameObject leaderboardMemberGO)
    {
        TMP_Text rank = leaderboardMemberGO.GetComponentsInChildren<TMP_Text>(true)[0];
        TMP_Text user = leaderboardMemberGO.GetComponentsInChildren<TMP_Text>(true)[1];
        TMP_Text score = leaderboardMemberGO.GetComponentsInChildren<TMP_Text>(true)[2];

        rank.text = lootLockerLeaderboardMember.rank.ToString();
        user.text = lootLockerLeaderboardMember.member_id;
        score.text = lootLockerLeaderboardMember.score.ToString();
    }
}
