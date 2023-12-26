using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySound : MonoBehaviour
{
    public void PlayJumpSound()
    {
        SoundManager.PlayJump();
    }

    public void PlayWalkSound()
    {
        SoundManager.PlayWalkStep();
    }

    public void PlayLandSound()
    {
        SoundManager.PlayLand();
    }

    public void PlayRunSound()
    {
        SoundManager.PlayRunStep();
    }

    public void PlayDashSound()
    {
        SoundManager.PlayDash();
    }

    public void PlaySquatSound()
    {
        SoundManager.PlayWalkStep();
    }

    public void PlayClimbsound()
    {
        SoundManager.PlayClimb();
    }
}
