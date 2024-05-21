using UnityEngine;

public class CameraManager : MonoBehaviour {
    [SerializeField] private IntegerVariable gridWidth;
    [SerializeField] private IntegerVariable gridHeight;

    [SerializeField] private FloatVariable singleBlockWidth;
    [SerializeField] private FloatVariable singleBlockHeight;

    [SerializeField] private FloatVariable screenHeight;
    [SerializeField] private FloatVariable topUIHeight;

    public void SetCamera() 
    {
        Camera mainCamera = GetComponent<Camera>();

        // Calculate the orthographic size based on the grid height and a minimum value of 5
        float orthographicSize = Mathf.Max(gridWidth.Value, gridHeight.Value, 5);

        // Calculate the remaining screen height in Unity units
        float topUIOffset = orthographicSize * (topUIHeight.Value / screenHeight.Value);

        // Calculate the offset for the grid based on the grid dimensions and the remaining screen area
        float offsetX = (gridWidth.Value - 1) * singleBlockWidth.Value / 2f;
        float offsetY = (gridHeight.Value - 1) * singleBlockHeight.Value / 2f + topUIOffset;

        // Set the camera position
        mainCamera.transform.position = new Vector3(offsetX, offsetY, -10);

        // Adjust the orthographic size to ensure the grid fits within the screen
        mainCamera.orthographicSize = orthographicSize;
    }
}
