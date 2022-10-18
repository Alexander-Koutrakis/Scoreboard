    
using UnityEngine;
using LootLocker.Requests;
using TMPro;
using System.Threading.Tasks;

public class LeaderboardPresentationController : MonoBehaviour
{
    private int highestShownRank=0;
    private int lowestShownRank;
    private int ranksPerPage = 10;
    private int shownRanks = 0;
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
        highestShownRank = 0;
        RefreshPage();
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
    public void PreviewsPage()
    {
        if (highestShownRank > 0)
        {
            highestShownRank = highestShownRank - ranksPerPage;
        }
        else
        {
            int remainningRanks = totalRanks % ranksPerPage;
            highestShownRank = totalRanks - remainningRanks; ;
            Debug.Log("Highest rank "+highestShownRank+" remainning ranks"+remainningRanks);
        }
        RefreshPage();
    }
    public async void RefreshPage()
    {
        LootLockerLeaderboardMember[] lootLockerLeaderboardMembers = await GetScores();
        ShowScores(lootLockerLeaderboardMembers);
    }
    private async Task<LootLockerLeaderboardMember[]> GetScores()
    {
        LootLockerLeaderboardMember[] lootLockerLeaderboardMembers = await leaderboardController.GetScores(highestShownRank, ranksPerPage);
        totalRanks = leaderboardController.Size;
        shownRanks = lootLockerLeaderboardMembers.Length;
        lowestShownRank = highestShownRank + shownRanks-1;
        lowestShownRank = Mathf.Clamp(lowestShownRank, 0, totalRanks);

        Debug.Log("Highest Rank " + highestShownRank + " Lowest Rank " + lowestShownRank);


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
