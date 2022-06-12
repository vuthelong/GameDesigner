using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private enum LadderPart { complete, lowerLimit, upperLimit };
    [SerializeField] private LadderPart part = LadderPart.complete;

    private void OnTriggerEnter2D(UnityEngine.Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case LadderPart.complete:
                    player.canClimb = true;
                    player.ladderScript = this;
                    break;
                case LadderPart.lowerLimit:
                    player.lowerLimitLadder = true;
                    break;
                case LadderPart.upperLimit:
                    player.upperLimitLadder = true;
                    break;
                default:
                    break;
            }
        }
    }
    private void OnTriggerExit2D(UnityEngine.Collider2D collision)
    {
        if (collision.GetComponent<PlayerController>())
        {
            PlayerController player = collision.GetComponent<PlayerController>();
            switch (part)
            {
                case LadderPart.complete:
                    player.canClimb = false;
                    break;
                case LadderPart.lowerLimit:
                    player.lowerLimitLadder = false;
                    break;
                case LadderPart.upperLimit:
                    player.upperLimitLadder = false;
                    break;
                default:
                    break;
            }
        }
    }
}
