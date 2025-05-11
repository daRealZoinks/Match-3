using UnityEngine;
using UnityEngine.InputSystem;

public class PCInputManager : MonoBehaviour
{
    public new Camera camera;
    public GameObject hoverEffect;
    public GridManager gridManager;

    private Gem hoveredGem;

    public void OnClick(InputAction.CallbackContext context)
    {
        if (context.phase != InputActionPhase.Started) return;

        if (hoveredGem != null)
        {
            gridManager.SelectGem(hoveredGem);
        }
    }

    private void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();

        var hit = Physics2D.Raycast(camera.ScreenToWorldPoint(mousePos), Vector2.zero);

        if (hit.collider != null)
        {
            Gem gem = hit.collider.GetComponent<Gem>();
            if (gem != null && gem != hoveredGem)
            {
                hoveredGem = gem;
                hoverEffect.SetActive(true);
                hoverEffect.transform.position = gem.transform.position;
            }
            else if (gem == null && hoveredGem != null)
            {
                hoveredGem = null;
                hoverEffect.SetActive(false);
            }
        }
        else
        {
            if (hoveredGem != null)
            {
                hoveredGem = null;
                hoverEffect.SetActive(false);
            }
        }
    }
}