using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AdvancedSelectable : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Multiline] public string flavourText;
    public Sprite flavourGraphic;
    public UnityEvent onHover;
    public UnityEvent onClick;

    MenuHandler hierarchyContaining;
    Selectable attachedSelectable;

    [SerializeField] bool autoSetVisibleName = true;
    Text visibleName;

    private void OnValidate()
    {
        if (autoSetVisibleName)
        {
            if (visibleName == null) visibleName = GetComponentInChildren<Text>();
            if (visibleName != null) visibleName.text = name;
        }
    }

    void Awake()
    {
        hierarchyContaining = GetComponentInParent<MenuHandler>();
        attachedSelectable = GetComponent<Selectable>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (attachedSelectable.interactable == false) return;
        // Update icon
        // Update flavour text
        onHover.Invoke();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (attachedSelectable.interactable == false) return;
        onClick.Invoke();
    }



    /// <summary>
    /// Plays a sound effect through the current menu hierarchy's AudioSource.
    /// </summary>
    /// <param name="effect"></param>
    public void PlaySoundEffect(AudioClip effect) => hierarchyContaining?.soundEffectPlayer?.PlayOneShot(effect);
}