using UnityEngine;
using UnityEngine.UI;

public class FishingMiniGame : MonoBehaviour {
    [SerializeField] Transform _topPivot;
    [SerializeField] Transform _bottomPivot;

    [Header("Fish Settings")]
    [SerializeField] Transform _fish;
    float _fishPosition;
    float _fishDestination;
    float _fishTimer;
    float _fishSpeed;
    [Tooltip("Time Between Fish Moving")]
    [SerializeField] float _timeMultiplicator = 3f;
    [Tooltip("Smoothing Speed When Fish Moving")]
    [SerializeField] float _smoothMotion = 1f;

    [Header("Hook Settings")]
    [SerializeField] Image _hookSpriteRenderer;
    [SerializeField] Transform _progressBarContainer;
    [SerializeField] Transform _hook;
    float _hookPosition;
    float _hookProgress;
    float _hookPullVelocity;
    [Tooltip("Size Of Hook")]
    [SerializeField] float _hookSize = 0.1f;
    [Tooltip("Hook Power To Increase Progress")]
    [SerializeField] float _hookPower = 0.5f;
    [Tooltip("Hook Value When Tap Or Hold")]
    [SerializeField] float _hookPullPower = 0.01f;
    [Tooltip("Hook Value When Fall Down")]
    [SerializeField] float _hookGravityPower = 0.005f;
    [Tooltip("Hook Value To Decrease Progress")]
    [SerializeField] float _hookProgressDegradationPower = 1f;

    [Header("Controller")]
    [SerializeField] bool _paused;
    [SerializeField] float _failTimer = 10f;

    // Start is called before the first frame update
    void Start() {
        Resize();
    }

    void Resize() {
        RectTransform rectTransform = _hookSpriteRenderer.rectTransform;
        float ySize = rectTransform.rect.height;
        float distance = Vector3.Distance(_topPivot.position, _bottomPivot.position);
        Vector3 ls = _hook.localScale;
        ls.y = (distance / ySize) * _hookSize;
        _hook.localScale = ls;
    }

    // Update is called once per frame
    void Update() {
        if (_paused) {
            return;
        }

        Fish();
        Hook();
        ProgressCheck();
    }

    void ProgressCheck() {
        Vector3 ls = _progressBarContainer.localScale;
        ls.y = _hookProgress;
        _progressBarContainer.localScale = ls;
        float min = _hookPosition - _hookSize * 0.5f;
        float max = _hookPosition + _hookSize * 0.5f;

        if (min < _fishPosition && _fishPosition < max) {
            _hookProgress += _hookPower * Time.deltaTime;
        } else {
            _hookProgress -= _hookProgressDegradationPower * Time.deltaTime;
            _failTimer -= Time.deltaTime;
            if (_failTimer < 0) {
                Lose();
            }
        }

        if (_hookProgress >= 1) {
            Win();
        }

        _hookProgress = Mathf.Clamp(_hookProgress, 0f, 1f);
    }

    void Lose() {
        _paused = true;
        Debug.Log("Lose");
    }

    void Win() {
        _paused = true;
        Debug.Log("Caught the fish!");
    }

    void Hook() {
        if (Input.GetMouseButton(0)) {
            _hookPullVelocity += _hookPullPower * Time.deltaTime;
        } else {
            _hookPullVelocity -= _hookGravityPower * Time.deltaTime;
        }

        if (_hookPosition - _hookSize * 0.5f <= 0f && _hookPullVelocity < 0f) {
            _hookPullVelocity = 0f;
        }

        if (_hookPosition + _hookSize * 0.5f >= 0.99f && _hookPullVelocity > 0f) {
            _hookPullVelocity = 0f;
        }

        _hookPosition += _hookPullVelocity;
        _hookPosition = Mathf.Clamp(_hookPosition, _hookSize * 0.5f, 1 - (_hookSize * 0.5f));
        _hook.position = Vector3.Lerp(_bottomPivot.position, _topPivot.position, _hookPosition);
    }

    void Fish() {
        _fishTimer -= Time.deltaTime;
        if (_fishTimer < 0f) {
            _fishTimer = Random.value * _timeMultiplicator;
            _fishDestination = Random.value;
        }

        _fishPosition = Mathf.SmoothDamp(_fishPosition, _fishDestination, ref _fishSpeed, _smoothMotion);
        _fish.position = Vector3.Lerp(_bottomPivot.position, _topPivot.position, _fishPosition);
    }
}