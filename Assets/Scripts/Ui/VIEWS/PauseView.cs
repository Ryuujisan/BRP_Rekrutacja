using UiInput;

public class PauseView : UiView
{
    protected override void ActiveView_OnClick(UiView viewToActive)
    {
        base.ActiveView_OnClick(viewToActive);
        StartCoroutine(UISelectHelper.GiveFocus(BackButon));
    }
}
