using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Menu_ChangeOnSelected : MonoBehaviour
{
    public Image button;

    public Sprite selected;
    public Sprite notSelected;

    public GameObject obj1Check;
    public GameObject obj2Check;


    void Update()
    {
        GameObject go = EventSystem.current.currentSelectedGameObject;
        bool select = (go == obj1Check) || (go == obj2Check);
        button.sprite = select ? selected : notSelected;
    }
}
