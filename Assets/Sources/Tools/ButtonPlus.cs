    /***********************
* UI.Button的扩展
* 带有间隔时间,防止操作过快
* 
* 部分Button 源码
*       private void Press()
        {
            if (!IsActive() || !IsInteractable())
                return;

            UISystemProfilerApi.AddMarker("Button.onClick", this);
            m_OnClick.Invoke();
        }

        // Trigger all registered callbacks.
        public virtual void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Press();
        }
**********************/
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[AddComponentMenu("GameObject/UI/ButtonPlus")]
/// <summary>
/// Button 扩展
/// </summary>
public class ButtonPlus : Button
{
    /// <summary>
    /// 点击间隔时间
    /// </summary>
    public float intervelClickTime = 0.2f;
    public bool doScale = true;
    public Vector3 highV3 = new Vector3(1.05f, 1.05f, 1.05f);

    Tweener Twexit, Twdown, Twclick;
    float clickTime = 0;
    public override void OnPointerClick(PointerEventData eventData)
    {
        if (Time.time - clickTime > intervelClickTime)
        {
            base.OnPointerClick(eventData);
            clickTime = Time.time;
        }
    }
 
    protected override void DoStateTransition(SelectionState state, bool instant)
    {
        base.DoStateTransition(state, instant);
        if (!doScale) return;
        if (Twexit != null && Twexit.IsPlaying()) Twdown.Kill();
        if (Twdown != null && Twdown.IsPlaying()) Twdown.Kill();
        if (Twclick != null && Twclick.IsPlaying()) Twclick.Kill();
        switch (state)
        {
            case SelectionState.Normal:
                Twexit = transform.DOScale(Vector3.one, 0.15f);
                break;
            case SelectionState.Highlighted:
                Twdown = transform.DOScale(highV3, 0.25f);
                break;
            case SelectionState.Pressed:
                Twclick = transform.DOPunchScale(new Vector3(-0.1f, -0.1f, 0), 0.4f, 6, 0.5f);
                break;
            case SelectionState.Disabled:
                break;
        }
    }
}
