using DG.Tweening;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailPopupManager : MonoBehaviour
{
    [SerializeField] private GameObject failPopupContainer;

    public void ActivateFailPopup() 
    {
        failPopupContainer.SetActive(true);
        failPopupContainer.transform.Find("Popup").GetComponent<RectTransform>().DOAnchorPosY(0, 0.5f).SetEase(Ease.OutExpo);
    }

    public void ReloadScene() 
    {
        SceneManager.LoadScene(1);
    }

    public void LoadMainScene() 
    {
        SceneManager.LoadScene(0);
    }
}
