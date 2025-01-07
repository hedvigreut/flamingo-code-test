using DG.Tweening;
using Lofelt.NiceVibrations;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem coinParticleSystem;
    [SerializeField] 
    private TMP_Text travelPointText;
    [SerializeField]
    private HapticClip coinClip;
    public void PlayCoinEffect()
    {
        coinParticleSystem.Play();
        HapticController.Play(coinClip);
    }
    public void PlayTravelPointsEffect(int coinValue)
    {
        travelPointText.text = $"+{coinValue}";
        travelPointText.gameObject.SetActive(true);
        travelPointText.DOFade(1, 1f)
            .OnKill(() => travelPointText.DOFade(0, 1f)
                .OnKill(() => travelPointText.gameObject.SetActive(false)));
    }
}