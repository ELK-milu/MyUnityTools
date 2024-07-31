using System;

[Flags]
public enum AnimationType
{
	None = 0b_0000_0000,
	Fade = 0b_0000_0001,
	Scale = 0b_0000_0010,
	Move = 0b_0000_0100,
	Rotate = 0b_0000_1000,
}

[Flags]
public enum FadeType
{
	None = 0b_0000_0000,
	RawImage = 0b_0000_0001,
	Image = 0b_0000_0010,
	CanvasGroup = 0b_0000_0100,
	TextMeshProUGUI = 0b_0000_1000,
	Text = 0b_0001_0000,
	TextBlend = Text | TextMeshProUGUI,
}
