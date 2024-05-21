using UnityEngine;


public class InputManager : MonoBehaviour
{
    [SerializeField] BooleanVariable isInputEnabled;

    private void Start() 
    {
        isInputEnabled.Value = true;
    }
    void Update() 
    {
        if (isInputEnabled.Value) 
        {
            HandleClickOnBlock();
        }
    }

    private void HandleClickOnBlock() 
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray.origin, ray.direction);
            if (hit.collider != null) 
            {
                Block block = hit.collider.GetComponent<Block>();
                if (block is ITappable tappableBlock) {
                    tappableBlock.OnTap();
                }
            }
        }
    }

    public void DisableInput() 
    {
        enabled = false;
    }
}
