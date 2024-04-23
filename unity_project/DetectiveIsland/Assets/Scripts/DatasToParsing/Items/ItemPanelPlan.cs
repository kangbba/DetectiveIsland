using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ItemPanelPlan
{
    public bool ShowEnterBtn { get; private set; }
    public bool ShowExitBtn { get; private set; }
    public bool ShowSubmitBtn { get; private set; }

    public ItemPanelPlan(bool showEnterBtn, bool showExitBtn, bool showSubmitBtn)
    {
        ShowEnterBtn = showEnterBtn;
        ShowExitBtn = showExitBtn;
        ShowSubmitBtn = showSubmitBtn;
    }
}
