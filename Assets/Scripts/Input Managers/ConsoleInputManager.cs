using UnityEngine;
using UnityEngine.InputSystem;

public class ConsoleInputManager : MonoBehaviour
{
    public GridManager gridManager;
    public GameObject hoverEffect;

    private Gem hoveredGem;
    private Vector2Int currentGridPosition;
    private Vector2 moveInput;
    private float moveCooldown = 0.1f;
    private float moveTimer = 0f;

    private void Start()
    {
        currentGridPosition = new Vector2Int(gridManager.width / 2, gridManager.height / 2);
        UpdateHoveredGem();
    }

    private void Update()
    {
        moveTimer += Time.deltaTime;

        if (moveInput != Vector2.zero && moveTimer >= moveCooldown)
        {
            Vector2Int direction = new Vector2Int(Mathf.RoundToInt(moveInput.x), Mathf.RoundToInt(moveInput.y));
            currentGridPosition += direction;
            currentGridPosition.x = Mathf.Clamp(currentGridPosition.x, 0, gridManager.width - 1);
            currentGridPosition.y = Mathf.Clamp(currentGridPosition.y, 0, gridManager.height - 1);
            UpdateHoveredGem();
            moveTimer = 0f;
        }
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveInput = context.phase switch
        {
            InputActionPhase.Started => context.ReadValue<Vector2>(),
            InputActionPhase.Performed => context.ReadValue<Vector2>(),
            _ => Vector2.zero
        };
    }

    public void OnSelect(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        if (hoveredGem != null)
        {
            gridManager.SelectGem(hoveredGem);
        }
    }

    private void UpdateHoveredGem()
    {
        if (currentGridPosition.x >= 0 && currentGridPosition.x < gridManager.width &&
            currentGridPosition.y >= 0 && currentGridPosition.y < gridManager.height)
        {
            hoveredGem = gridManager.GetGemAt(currentGridPosition.x, currentGridPosition.y);

            if (hoveredGem != null)
            {
                hoverEffect.SetActive(true);
                hoverEffect.transform.position = hoveredGem.transform.position;
            }
        }
        else
        {
            hoveredGem = null;
            hoverEffect.SetActive(false);
        }
    }
}