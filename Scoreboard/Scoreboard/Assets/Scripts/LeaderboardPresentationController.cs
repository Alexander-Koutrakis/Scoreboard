    
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using System.Threading.Tasks;

public class LeaderboardPresentationController : MonoBehaviour
{
    private int highestShownRank=0;
    private int lowestShownRank;
    private int shownRanks = 10;
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
    public void ShowTop()
    {
        highestShownRank = -100;
        RefreshPage();
    }      
    public void NextPage()
    {
        highestShownRank = lowestShownRank + 1;
        RefreshPage();
    }
    public void PreviewsPage()
    {
        highestShownRank = highestShownRank - 1 - shownRanks;
        RefreshPage();
    }
    public async void RefreshPage()
    {
        LootLockerLeaderboardMember[] lootLockerLeaderboardMembers = await GetScores();
        ShowScores(lootLockerLeaderboardMembers);
    }
    private async Task<LootLockerLeaderboardMember[]> GetScores()
    {
        LootLockerLeaderboardMember[] lootLockerLeaderboardMembers = await leaderboardController.GetScores(highestShownRank, shownRanks);
        totalRanks = await leaderboardController.TotalRanks();
        lowestShownRank = highestShownRank + lootLockerLeaderboardMembers.Length - 1;
        lowestShownRank = Mathf.Clamp(lowestShownRank, 0, totalRanks);
        return lootLockerLeaderboardMembers;
    }
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
