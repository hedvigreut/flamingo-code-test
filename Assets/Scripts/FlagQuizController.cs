using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FlagQuizController : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI flagQuestion;
    [SerializeField]
    private Image[] flags;

    [SerializeField] private TextAsset flagData;
}
