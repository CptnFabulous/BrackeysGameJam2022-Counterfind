using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AdvancedSelectable : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Multiline] public string flavourText;
    public Sprite icon;
    public UnityEvent onHover;
    public UnityEvent onClick;

    MenuHandler hierarchyContaining;

    void Awake()
    {
        hierarchyContaining = GetComponentInParent<MenuHandler>();
    }

    public void OnPointerEnter(PointerEventData eventData) => onHover.Invoke();
    public void OnPointerClick(PointerEventData eventData) => onClick.Invoke();



    /// <summary>
    /// Plays a sound effect through the current menu hierarchy's AudioSource.
    /// </summary>
    /// <param name="effect"></param>
    public void PlaySoundEffect(AudioClip effect) => hierarchyContaining?.soundEffectPlayer?.PlayOneShot(effect);
}