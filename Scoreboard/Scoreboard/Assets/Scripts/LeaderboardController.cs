using System.Collections;
using LootLocker.Requests;
using UnityEngine;

public class LeaderboardController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(StartSession());
    }

    private IEnumerator StartSession()
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
            }

        });

        yield return new WaitWhile(() => done == false);
    }

}
