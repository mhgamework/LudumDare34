using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndGameController : MonoBehaviour
{

    [SerializeField]
    private HearthController playerHeart;
    [SerializeField]
    private HearthController princessHeart;

    [SerializeField] private PlayerControl player;

    [SerializeField]
    private Transform curtain;
    [SerializeField]
    private float curtainTargetRelativeY;
    [SerializeField]
    private float curtainSpeed;

    private float curtainTargetY;


    [SerializeField]
    private float hardcodedTargetPlayerX = -1;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(doEndGameScript().GetEnumerator());
        curtainTargetY = curtain.position.y + curtainTargetRelativeY;
    }

    private IEnumerable<YieldInstruction> doEndGameScript()
    {
        for (;;)
        {
            while (!isPlayerAtTop()) yield return null;
            disableUserInput();
            while (!isPlayerAtCenter())
            {
                stepMovePlayerToCenter();
                yield return null;
            }
            player.SetAutoMoveDir(0);

            while (!isPrincessAtTop()) yield return null;
            Debug.Log("Hearts");
            playerHeart.ShowHearts();

            yield return new WaitForSeconds(5);

            princessHeart.ShowHearts();

            yield return new WaitForSeconds(5);

            while (stepCurtain()) { yield return null; }

            yield return new WaitForSeconds(4);

            for (;;)
            {
                stepFadeOutMusic();
                if (Input.anyKeyDown) Application.Quit();

                yield return null;

            }
        }
    }

    private void stepFadeOutMusic()
    {
        //throw new System.NotImplementedException();
    }

    private bool stepCurtain()
    {
        var pos = curtain.transform.position;
        var targetY = curtainTargetY;

        if (Mathf.Abs(pos.y - targetY) < 0.001) return false;


        var diff = targetY-pos.y  ;

        pos.y += Mathf.Sign(diff)*Mathf.Min(Mathf.Abs(diff), Time.deltaTime*curtainSpeed);

        curtain.transform.position = pos;
        return true;


    }


    private void disableUserInput()
    {
        player.DisableUserInput();
    }

    private bool isPrincessAtTop()
    {
        return true;
    }

    private bool isPlayerAtCenter()
    {
        return player.transform.position.x < hardcodedTargetPlayerX; // Hardcoded!
    }

    private void stepMovePlayerToCenter()
    {
        player.SetAutoMoveDir(1);
        /*var z = player.transform.position.z;
        player.SetAutoMoveDir((int)Mathf.Sign(z));*/
    }

    private bool isPlayerAtTop()
    {
        return playerAtTop;
    }

    // Update is called once per frame
    void Update()
    {

    }


    public void OnPlayerAtTop()
    {
        playerAtTop = true;
    }
    private bool playerAtTop = false;
    
}
