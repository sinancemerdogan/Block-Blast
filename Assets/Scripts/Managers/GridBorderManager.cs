using UnityEngine;

public class GridBorderManager : MonoBehaviour
{
    [SerializeField] private IntegerVariable gridWidth;
    [SerializeField] private IntegerVariable gridHeight;

    [SerializeField] private FloatVariable singleBlockWidth;
    [SerializeField] private FloatVariable singleBlockHeight;

    [SerializeField] private FloatVariable borderWidthOffset;
    [SerializeField] private FloatVariable borderHeightOffset;

    public void SetBorder() 
    {
        float offsetX = (gridWidth.Value - singleBlockWidth.Value) / 2f;
        float offsetY = (gridHeight.Value - singleBlockHeight.Value) / 2f;
        transform.position = new Vector3(offsetX, offsetY, 0);
        GetComponent<SpriteRenderer>().size = new Vector2(gridWidth.Value + borderWidthOffset.Value, gridHeight.Value + borderHeightOffset.Value);
    }
}
