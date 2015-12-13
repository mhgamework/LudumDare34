using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EndGameController : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
	    StartCoroutine(doEndGameScript().GetEnumerator());
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

            while (!isPrincessAtTop()) yield return null;

            playerHeart.Play();

            yield return new WaitForSeconds(1);

            princessHeart.Play();

            yield return new WaitForSeconds(3);

            while (stepCurtain()) { yield return null;}

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
        throw new System.NotImplementedException();
    }

    private bool stepCurtain()
    {
        throw new System.NotImplementedException();
    }


    private void disableUserInput()
    {
        throw new System.NotImplementedException();
    }

    private bool isPrincessAtTop()
    {
        throw new System.NotImplementedException();
    }

    private bool isPlayerAtCenter()
    {
        throw new System.NotImplementedException();
    }

    private void stepMovePlayerToCenter()
    {
        throw new System.NotImplementedException();
    }

    private bool isPlayerAtTop()
    {
        throw new System.NotImplementedException();
    }

    // Update is called once per frame
	void Update () {
	
	}
}
