using UnityEngine;
public enum EBind
{
    Button,
    Image,
    Text,
    RawImage,
    Toggle,
    Slider,
    Scrollbar,
    Dropdown,
    InputField,
    ScrollView,
    Transform,
    TextMeshProUGUI,
}
public class Bind : UnityEngine.MonoBehaviour
{
    public EBind bind = EBind.Button;
    [HideInInspector]
    public string FindPath;
}