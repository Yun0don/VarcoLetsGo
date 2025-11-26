using UnityEngine;
using UnityEngine.EventSystems;

/// 화면을 누른 위치에 베이스가 뜨고,
/// 드래그 방향/거리로 -1~1 방향 벡터를 만들어주는 플로팅 조이스틱.
public class DynamicFloatingJoystick : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Refs")]
    [SerializeField] RectTransform baseRect;    // 바닥 원 (Base)
    [SerializeField] RectTransform handleRect;  // 손잡이 (Handle)
    [SerializeField] float radius = 100f;       // 손잡이가 이동할 최대 반경 (px)

    public Vector2 Direction { get; private set; } // -1~1 범위의 방향

    Canvas canvas;
    RectTransform containerRect; // JoystickArea

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        containerRect = transform as RectTransform;

        // 시작은 숨김 상태
        Hide();
    }

    // ------------ Public API ------------

    /// 터치 위치에 조이스틱을 띄운다.
    public void Show(PointerEventData eventData)
    {
        baseRect.gameObject.SetActive(true);
        handleRect.gameObject.SetActive(true);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                containerRect,
                eventData.position,
                GetEventCamera(eventData),
                out Vector2 localPos))
        {
            baseRect.anchoredPosition = localPos;
        }

        handleRect.anchoredPosition = Vector2.zero;
        Direction = Vector2.zero;
    }

    /// 조이스틱을 숨기고 상태를 초기화한다.
    public void Hide()
    {
        baseRect.gameObject.SetActive(false);
        handleRect.gameObject.SetActive(false);
        handleRect.anchoredPosition = Vector2.zero;
        Direction = Vector2.zero;
    }

    // ------------ EventSystem Callbacks ------------

    public void OnPointerDown(PointerEventData eventData)
    {
        // JoystickArea는 항상 켜져 있으므로 언제든지 다운 이벤트를 받는다.
        Show(eventData);
        OnDrag(eventData); // 살짝 움직인 느낌 주고 싶으면 유지
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (!baseRect.gameObject.activeSelf)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                baseRect,
                eventData.position,
                GetEventCamera(eventData),
                out Vector2 localPos))
        {
            if (localPos.magnitude > radius)
                localPos = localPos.normalized * radius;

            handleRect.anchoredPosition = localPos;
            Direction = localPos / radius;  // -1~1
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Hide();
    }

    // ------------ Helper ------------

    Camera GetEventCamera(PointerEventData eventData)
    {
        if (canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            return null;

        return eventData.pressEventCamera ?? canvas.worldCamera;
    }
}
