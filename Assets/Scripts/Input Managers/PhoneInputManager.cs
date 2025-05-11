using UnityEngine;
using UnityEngine.InputSystem;

public class PhoneInputManager : MonoBehaviour
{
    public GridManager gridManager;

    public void OnTouch(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        Vector2 touchPosition = Touchscreen.current.position.ReadValue();
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(touchPosition);
        RaycastHit2D hit = Physics2D.Raycast(worldPosition, Vector2.zero);

        if (hit.collider != null)
        {
            if (hit.collider.TryGetComponent<Gem>(out var gem))
            {
                gridManager.SelectGem(gem);
            }
        }
    }
}