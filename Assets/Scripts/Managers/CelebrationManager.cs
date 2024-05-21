using UnityEngine;

public class CelebrationManager : MonoBehaviour
{
    [SerializeField] private GameObject celebrationContainer;

    public void ActivateCelebrationContainer() 
    {
        if(IsCelebrationContainerPresent()) 
        {
            celebrationContainer.SetActive(true);
        }
    }

    public void DeactivateCelebrationContainer() 
    {
        if (IsCelebrationContainerPresent()) 
        {
            celebrationContainer.SetActive(false);
        }
            
    }

    private bool IsCelebrationContainerPresent() 
    {
        if (celebrationContainer == null) 
        {
            Debug.LogWarning("CelebrationManager: The celebration canvas is not assigned. Make sure to assign it in the inspector.");
            return false;
        }
        return true;
    }
}
