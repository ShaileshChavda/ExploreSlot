using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class EventTriggerDelegateExample : MonoBehaviour
{
    public GunManager atc;
    private EventTrigger trigger;
    private EventTrigger.Entry onPress;
    private EventTrigger.Entry onRelese;

    // Start is called before the first frame update
    void Start()
    {
        trigger = GetComponent<EventTrigger>();
        onPress = new EventTrigger.Entry();
        onPress.eventID = EventTriggerType.PointerDown;
        onPress.callback.AddListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        trigger.triggers.Add(onPress);

        onRelese = new EventTrigger.Entry();
        onRelese.eventID = EventTriggerType.PointerUp;
        onRelese.callback.AddListener((data) => { OnPointerUpDelegate((PointerEventData)data); });
        trigger.triggers.Add(onRelese);
    }

    public void OnPointerDownDelegate(PointerEventData data)
    {
        //Debug.Log("OnPointerDownDelegate called.");
        atc._isGunPress = true;
    }

    public void OnPointerUpDelegate(PointerEventData data)
    {
        //Debug.Log("OnPointerUpDelegate called.");
        atc._isGunPress = false;
    }

    private void OnDisable()
    {
        onPress.callback.RemoveListener((data) => { OnPointerDownDelegate((PointerEventData)data); });
        onRelese.callback.RemoveListener((data) => { OnPointerUpDelegate((PointerEventData)data); });
        trigger.triggers.Clear();
    }
}
