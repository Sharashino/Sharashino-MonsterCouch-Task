using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine;

namespace _MonsterCouch.UI
{
    public class UINavigationController : MonoBehaviour
    {
        [Header("Navigation Settings"), SerializeField] 
        private Selectable firstSelected;
        [SerializeField] 
        private bool selectOnEnable = true;
        [SerializeField] 
        private bool autoReselect = true;
        [SerializeField] 
        private bool clearSelectionOnDisable;

        private GameObject lastSelected;

        private void Start()
        {
            if (selectOnEnable)
                SelectFirstElement();
        }

        private void OnEnable()
        {
            if (selectOnEnable)
                SelectFirstElement();
        }

        private void OnDisable()
        {
            if (clearSelectionOnDisable && EventSystem.current != null)
                EventSystem.current.SetSelectedGameObject(null);
        }

        private void LateUpdate()
        {
            if (!autoReselect || EventSystem.current == null)
                return;

            GameObject currentSelected = EventSystem.current.currentSelectedGameObject;

            // Store the last selected object if it's valid
            if (currentSelected != null && currentSelected.activeInHierarchy)
                lastSelected = currentSelected;

            // If nothing is selected, try to reselect
            if (currentSelected == null)
            {
                // First try to reselect the last selected object if it's still valid
                if (lastSelected != null && lastSelected.activeInHierarchy)
                {
                    Selectable selectable = lastSelected.GetComponent<Selectable>();
                    if (selectable != null && selectable.IsInteractable())
                    {
                        EventSystem.current.SetSelectedGameObject(lastSelected);
                        return;
                    }
                }

                // Otherwise, select the first element
                SelectFirstElement();
            }
            // If the currently selected object is not interactable, find the next one
            else if (currentSelected.TryGetComponent<Selectable>(out var currentSelectable))
                if (!currentSelectable.IsInteractable())
                    SelectFirstElement();
        }

        private void SelectFirstElement()
        {
            if (firstSelected != null && EventSystem.current != null)
            {
                // Make sure the element is interactable before selecting
                if (firstSelected.IsInteractable())
                {
                    EventSystem.current.SetSelectedGameObject(firstSelected.gameObject);
                    lastSelected = firstSelected.gameObject;
                }
                else
                {
                    // If first selected is not interactable, try to find the next one
                    Selectable next = firstSelected.FindSelectableOnDown()
                                      ?? firstSelected.FindSelectableOnRight()
                                      ?? firstSelected.FindSelectableOnUp()
                                      ?? firstSelected.FindSelectableOnLeft();

                    if (next != null && next.IsInteractable())
                    {
                        EventSystem.current.SetSelectedGameObject(next.gameObject);
                        lastSelected = next.gameObject;
                    }
                }
            }
        }
    }
}
