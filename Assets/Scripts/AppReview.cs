using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Google.Play.Review;

public class AppReview : MonoBehaviour
{
    public static AppReview instance;

    ReviewManager reviewManager;
    PlayReviewInfo reviewInfo;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    public void RequestReview()
    {
        if (Application.platform == RuntimePlatform.Android)
        {
            reviewManager = new ReviewManager();

            if (!GameManager.instance.GetReviewChecked() && GameManager.instance.GetTotalScore() > 1000)
            {
                StartCoroutine(ReviewOperation());
            }
        }
    }

    IEnumerator ReviewOperation()
    {
        yield return new WaitForSeconds(1f);

        var requestFlowOperation = reviewManager.RequestReviewFlow();
        yield return requestFlowOperation;

        if (requestFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError(requestFlowOperation.Error.ToString());
            yield break;
        }

        reviewInfo = requestFlowOperation.GetResult();
        var launchFlowOperation = reviewManager.LaunchReviewFlow(reviewInfo);
        yield return launchFlowOperation;
        reviewInfo = null;

        if (launchFlowOperation.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError(requestFlowOperation.Error.ToString());
            yield break;
        }

        GameManager.instance.SetReviewChecked(true);
        DBManager.instance.data.reviewChecked = true;
        DBManager.instance.SaveCurrentData();
    }
}
