using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem;
    public void PlayCoinEffect()
    {
        particleSystem.Play();
    }

    public void PlayTravelPointsEffect(int coinValue)
    {
        Debug.Log("Play travel points");
    }
}
