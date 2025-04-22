using UnityEngine;
using UnityEngine.EventSystems;

public class FirstPersonLook : MonoBehaviour
{
    [SerializeField] Transform character;
    public float sensitivity = 2;
    public float smoothing = 1.5f;

    Vector2 velocity;
    Vector2 frameVelocity;

    private int lookFingerId = -1;
    private Vector2 lastTouchPosition;

    private void Start()
    {
        // Получаем начальные углы поворота
        Vector3 characterRotation = character.localEulerAngles;
        Vector3 cameraRotation = transform.localEulerAngles;

        velocity.x = characterRotation.y;
        velocity.y = -cameraRotation.x;
    }

    void Update()
    {
#if UNITY_WEBGL
        if (Input.touchCount > 0)
            HandleTouchLook();
        else
            HandleMouseLook();
#else
        if (Input.touchSupported && Application.isMobilePlatform)
            HandleTouchLook();
        else
            HandleMouseLook();
#endif
    }

    void HandleMouseLook()
    {
        Vector2 mouseDelta = new Vector2(Input.GetAxisRaw("Mouse X"), Input.GetAxisRaw("Mouse Y"));
        Vector2 rawFrameVelocity = Vector2.Scale(mouseDelta, Vector2.one * sensitivity);
        frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
        velocity += frameVelocity;
        velocity.y = Mathf.Clamp(velocity.y, -90, 90);

        ApplyRotation();
    }

    void HandleTouchLook()
    {
        if (Input.touchCount == 0)
        {
            lookFingerId = -1;
            return;
        }

        for (int i = 0; i < Input.touchCount; i++)
        {
            Touch touch = Input.GetTouch(i);

            if (lookFingerId == -1 && touch.phase == TouchPhase.Began)
            {
                lookFingerId = touch.fingerId;
                lastTouchPosition = touch.position;
                frameVelocity = Vector2.zero;
                return;
            }

            if (touch.fingerId == lookFingerId)
            {
                if (touch.phase == TouchPhase.Moved || touch.phase == TouchPhase.Stationary)
                {
                    Vector2 touchDelta = (touch.position - lastTouchPosition) * (sensitivity / 10f);
                    lastTouchPosition = touch.position;

                    Vector2 rawFrameVelocity = new Vector2(touchDelta.x, touchDelta.y);
                    frameVelocity = Vector2.Lerp(frameVelocity, rawFrameVelocity, 1 / smoothing);
                    velocity += frameVelocity;
                    velocity.y = Mathf.Clamp(velocity.y, -90, 90);

                    ApplyRotation();
                }

                if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
                {
                    lookFingerId = -1;
                    frameVelocity = Vector2.zero;
                }
            }
        }
    }

    void ApplyRotation()
    {
        transform.localRotation = Quaternion.AngleAxis(-velocity.y, Vector3.right);
        character.localRotation = Quaternion.AngleAxis(velocity.x, Vector3.up);
    }
}
