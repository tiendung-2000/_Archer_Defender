using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Quaternion;

public class CircleController : MonoBehaviour {
    public RectTransform pointHolder;
    public RectTransform areaHolder;
    public Image pointImage; // The rotating red point image
    public Image areaImage; // The stationary green area image
    public Button actionButton; // The UI button for performing the action
    public float rotationSpeed = 100f; // Speed at which the red point rotates
    public float areaSize = 0.1f; // Size of the green area in fillAmount (0 to 1)
    public float pointSize = 0.02f; // Size of the red point in fillAmount (0 to 1)

    float _currentRotation; // Current rotation angle of the red point
    [SerializeField] bool _isPointInArea; // Boolean to store if the point is in the area

    void Start() {
        // Set the sizes of the green area and the red point
        pointImage.fillAmount = pointSize;
        pointHolder.localRotation = Euler(0, 0, 360 * pointSize / 2);
        SetAreaLocation();

        // Assign the button click event
        actionButton.onClick.AddListener(OnActionButtonClick);
    }

    void SetAreaLocation() {
        var areaRand = Random.Range(0.05f, 0.15f);
        areaSize = areaRand;
        areaImage.fillAmount = areaSize;
        areaHolder.localRotation = Euler(0, 0, 360 * areaSize / 2);

        // Set the green area to a fixed position (example: 90 degrees on the circle)
        var random = Random.Range(-180f, 180f);
        areaImage.rectTransform.localRotation = Euler(0, 0, random); // Adjust as needed
    }

    void Update() {
        RotatePoint(); // Rotate the red point
        CheckPointInArea(); // Check if the red point overlaps with the green area
    }

    void RotatePoint() {
        // Rotate the current rotation angle of the red point
        _currentRotation += rotationSpeed * Time.deltaTime;
        if (_currentRotation >= 360f) _currentRotation -= 360f; // Wrap around at 360 degrees

        // Apply the rotation to the red point
        pointImage.rectTransform.localRotation = Euler(0, 0, -_currentRotation);
    }

    void CheckPointInArea() {
        // Calculate the angle of the green area (fixed) and the rotating red point
        float pointAngle = pointImage.rectTransform.localRotation.eulerAngles.z;
        float areaAngle = areaImage.rectTransform.localRotation.eulerAngles.z;

        // Convert area and point sizes from fillAmount (0 to 1) to degrees (0 to 360)
        float areaSizeDegrees = areaSize * 360f;
        float pointSizeDegrees = pointSize * 360f;

        // Calculate the angle difference between the center of the green area and the red point
        float angleDifference = Mathf.Abs(Mathf.DeltaAngle(pointAngle, areaAngle));

        // Check if the point's angle is within the bounds of the area size and store result
        _isPointInArea = angleDifference <= (areaSizeDegrees / 2f + pointSizeDegrees / 2f);
    }

    void OnActionButtonClick() {
        // Check if the point is inside the area when the button is clicked
        if (_isPointInArea) {
            SetAreaLocation(); // Success action
        } else {
            Debug.Log("Point is outside the area. Try again!"); // Failure action
        }
    }
}