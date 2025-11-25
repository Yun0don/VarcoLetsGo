using UnityEngine;
using UnityEngine.EventSystems;

/// 화면을 누른 위치에 베이스가 뜨고,
/// 드래그 방향/거리로 -1~1 방향 벡터를 만들어주는 플로팅 조이스틱.
public class DynamicFloatingJoystick : MonoBehaviour,
    IPointerDownHandler, IPointerUpHandler, IDragHandler
{
    [Header("Refs")]
    [SerializeField] RectTransform baseRect;    // 바닥 원 (투명/반투명)
    [SerializeField] RectTransform handleRect;  // 손잡이
    [SerializeField] float radius = 100f;       // 손잡이가 이동할 최대 반경 (px)

    public Vector2 Direction { get; private set; } // -1~1 범위의 방향

    Canvas canvas;
    Camera uiCam;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        uiCam = canvas.worldCamera;

        Hide();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // 터치한 지점에 베이스 위치 시키기 (캔버스 기준 좌표로 변환)
        RectTransform canvasRect = canvas.transform as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, eventData.position, uiCam, out Vector2 localPos))
        {
            baseRect.anchoredPosition = localPos;
        }

        baseRect.gameObject.SetActive(true);
        handleRect.anchoredPosition = Vector2.zero;
        Direction = Vector2.zero;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // 베이스 기준 로컬 좌표로 변환
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            baseRect, eventData.position, uiCam, out Vector2 localPos))
        {
            // 반경 제한
            if (localPos.magnitude > radius)
                localPos = localPos.normalized * radius;

            handleRect.anchoredPosition = localPos;

            // -1 ~ 1 구간으로 정규화된 입력 벡터
            Direction = localPos / radius;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        Direction = Vector2.zero;
        Hide();
    }

    void Hide()
    {
        baseRect.gameObject.SetActive(false);
        handleRect.anchoredPosition = Vector2.zero;
    }
}
