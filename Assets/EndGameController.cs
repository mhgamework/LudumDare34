using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndGameController : MonoBehaviour
{

    [SerializeField]
    private HearthController playerHeart;
    [SerializeField]
    private HearthController princessHeart;

    [SerializeField]
    private PlayerControl player;
    [SerializeField]
    private RobotGirlController princess;

    [SerializeField]
    private Transform curtain;
    [SerializeField]
    private float curtainTargetRelativeY;
    [SerializeField]
    private float curtainSpeed;

    [SerializeField]
    private GameObject QuitHelpText = null;

    [SerializeField]
    private Transform curtainTargetTransform;


    [SerializeField]
    private float hardcodedTargetPlayerX = -1;

    [SerializeField]
    private float princessTopY = 31.8f;
    [SerializeField]
    private Transform walkToTransform;

    [SerializeField]
    private RobotAnimator robotAnimator;


    public bool DebugTest = false;


    private Vector3 PrincessTargetPos = new Vector3();
    // Use this for initialization
    void Start()
    {
        StartCoroutine(doEndGameScript().GetEnumerator());
    }

    private IEnumerable<YieldInstruction> doEndGameScript()
    {
        yield return new WaitForSeconds(1f);
        curtain.gameObject.SetActive(false);

        for (; ; )
        {
            while (!isPlayerAtTop()) yield return null;
            disableUserInput();
            while (!isPlayerAtCenter())
            {
                stepMovePlayerToCenter();
                yield return null;
            }
            player.SetMoveDir(0);
            player.SetAutoMoveDir(0);

            if (!DebugTest)
            {
                while (!isPrincessAtTop())
                {
                    yield return null;
                }
            }


            Debug.Log("Hearts");

            QuitHelpText.SetActive(false);

            yield return new WaitForSeconds(0.5f);

            robotAnimator.PlayWalk();
            yield return StartCoroutine(WalkToPrincess());
            robotAnimator.PlayIdle();

            playerHeart.ShowHearts();

            yield return new WaitForSeconds(2);

            princessHeart.ShowHearts();

            yield return new WaitForSeconds(5);

            
            curtain.gameObject.SetActive(true);
            yield return StartCoroutine(stepCurtain());

            yield return new WaitForSeconds(4);

            StartCoroutine(UpdateQuitHelpText());

            for (; ; )
            {
                stepFadeOutMusic();
                if (Input.anyKeyDown) Application.Quit();

                yield return null;

            }
        }
    }

    private IEnumerator WalkToPrincess()
    {
        player.enabled = false;
        player.GetComponent<Rigidbody>().useGravity = false;

        var start = player.transform.position;
        var end = walkToTransform.position;

        var start_r = player.transform.rotation;
        var end_r = walkToTransform.transform.rotation;

        var elapsed = 0f;
        var total_time = 5f;

        while (elapsed < total_time)
        {
            player.transform.position = EasingFunctions.Ease(EasingFunctions.TYPE.InOut, elapsed / total_time, start, end);

            if (elapsed < total_time * 0.1f)
                player.transform.rotation = EasingFunctions.Ease(EasingFunctions.TYPE.InOut, elapsed / (total_time * 0.1f), start_r, end_r);

            elapsed += Time.deltaTime;
            yield return null;
        }
    }


    private IEnumerator UpdateQuitHelpText()
    {
        yield return new WaitForSeconds(2f);
        QuitHelpText.SetActive(true);
    }

    private void stepFadeOutMusic()
    {
        //throw new System.NotImplementedException();
    }

    private IEnumerator stepCurtain()
    {
        var start = curtain.transform.position.y;
        var end = curtainTargetTransform.position.y;

        var elapsed = 0f;
        var total_time = 2f;
        while (elapsed < total_time)
        {
            var new_y = EasingFunctions.Ease(EasingFunctions.TYPE.OutBounce, elapsed / total_time, start, end);
            curtain.transform.position = new Vector3(curtain.transform.position.x, new_y, curtain.transform.position.z);
            elapsed += Time.deltaTime;
            yield return null;
        }
    }


    private void disableUserInput()
    {
        player.DisableUserInput();
    }

    private bool isPrincessAtTop()
    {
        return princess.transform.position.y > princessTopY;
    }



    private void stepMovePlayerToCenter()
    {
        player.SetAutoMoveDir(1);
    }


    private bool isPlayerAtCenter()
    {
        return playerAtCenter;
    }
    public void OnPlayerAtCenter()
    {
        playerAtCenter = true;
    }
    private bool playerAtCenter = false;

    private bool isPlayerAtTop()
    {
        return playerAtTop;
    }
    public void OnPlayerAtTop()
    {
        playerAtTop = true;
    }
    private bool playerAtTop = false;

}
