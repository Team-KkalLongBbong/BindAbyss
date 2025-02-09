using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Button : UIPopUp
{
    enum Texts
    {
        ButtonText,
        PanelText,
        TestText
    }

    enum Buttons
    {
        Button
    }

    enum GameObjects
    {

    }

    enum Images
    {
        Mika
    }
    public override void Init()
    {
        //부모 Init 호출식
        base.Init();

        Bind<Button>(typeof(Buttons));
        Bind<Image>(typeof(Images));
        Bind<Text>(typeof(Texts));
        Bind<GameObject>(typeof(GameObjects));

        GameObject go = GetImage((int)Images.Mika).gameObject;
        BindEvent(go, (PointerEventData data) => { go.transform.position = data.position; }, Define.UIEvent.Drag);

        GetText((int)Texts.TestText).text = "Go";
    }

}