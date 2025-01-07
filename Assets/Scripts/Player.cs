using DG.Tweening;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private ParticleSystem particleSystem;
    [SerializeField] 
    private TMP_Text travelPointText;
    public void PlayCoinEffect()
    {
        particleSystem.Play();
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