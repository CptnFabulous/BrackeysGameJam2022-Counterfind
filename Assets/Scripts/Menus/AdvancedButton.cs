using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AdvancedButton : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    [Multiline] public string flavourText;
    public Sprite icon;
    public UnityEvent onHover;
    public UnityEvent onClick;

    


    public void OnPointerEnter(PointerEventData eventData) => onHover.Invoke();
    public void OnPointerClick(PointerEventData eventData) => onClick.Invoke();




    public void PlaySoundEffect(AudioClip effect)
    {
        AudioSource.PlayClipAtPoint(effect, transform.position);
    }
}
