using System;
using Pixelplacement;
using Pixelplacement.TweenSystem;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000162 RID: 354
public class StorySheepStatusDisplay : MonoBehaviour
{
	// Token: 0x0600087F RID: 2175 RVA: 0x00007F66 File Offset: 0x00006166
	private void Awake()
	{
		this.rootRectTransform = base.GetComponent<RectTransform>();
	}

	// Token: 0x06000880 RID: 2176 RVA: 0x00007F74 File Offset: 0x00006174
	private void OnEnable()
	{
		this.AnimateOnScreenComplete();
	}

	// Token: 0x06000881 RID: 2177 RVA: 0x00038810 File Offset: 0x00036A10
	private void AnimateOffScreen()
	{
		this.currentDisplayState = StorySheepStatusDisplay.DisplayState.AnimatingOut;
		this.movementTween = Tween.AnchoredPosition(this.rootRectTransform, new Vector3(0f, 280f, 0f), 0.5f, 0f, Tween.EaseIn, Tween.LoopType.None, null, new Action(this.AnimateOffScreenComplete), true);
	}

	// Token: 0x06000882 RID: 2178 RVA: 0x00007F7C File Offset: 0x0000617C
	private void AnimateOffScreenComplete()
	{
		this.currentDisplayState = StorySheepStatusDisplay.DisplayState.OffScreen;
	}

	// Token: 0x06000883 RID: 2179 RVA: 0x0003886C File Offset: 0x00036A6C
	private void AnimateOnScreen()
	{
		this.currentDisplayState = StorySheepStatusDisplay.DisplayState.AnimatingIn;
		if (this.movementTween != null)
		{
			this.movementTween.Stop();
		}
		this.movementTween = Tween.AnchoredPosition(this.rootRectTransform, new Vector3(0f, 0f, 0f), 0.5f, 0f, Tween.EaseOut, Tween.LoopType.None, null, new Action(this.AnimateOnScreenComplete), true);
	}

	// Token: 0x06000884 RID: 2180 RVA: 0x00007F85 File Offset: 0x00006185
	private void AnimateOnScreenComplete()
	{
		this.currentDisplayState = StorySheepStatusDisplay.DisplayState.OnScreen;
		this.rootRectTransform.anchoredPosition = Vector3.zero;
		this.onScreenTimer = 2f;
	}

	// Token: 0x06000885 RID: 2181 RVA: 0x000388DC File Offset: 0x00036ADC
	private void Update()
	{
		if (this.state == 0 && this.onScreenTimer > 0f)
		{
			this.onScreenTimer -= Time.deltaTime;
			if (this.onScreenTimer <= 0f)
			{
				this.AnimateOffScreen();
			}
		}
		if (Input.GetKeyDown(KeyCode.P))
		{
			if (this.state == 0)
			{
				this.state = 1;
				this.AnimateOnScreen();
				return;
			}
			this.state = 0;
			this.AnimateOffScreen();
		}
	}

	// Token: 0x06000886 RID: 2182 RVA: 0x00038950 File Offset: 0x00036B50
	public void UpdateSheepPlayerIndexes(int shaunPlayerIndex, int timmyPlayerIndex, int shirleyPlayerIndex)
	{
		this.sheepPlayerIndex[0] = shaunPlayerIndex;
		this.sheepPlayerIndex[1] = timmyPlayerIndex;
		this.sheepPlayerIndex[2] = shirleyPlayerIndex;
		this.UpdateSheepDisplay(0, shaunPlayerIndex);
		this.UpdateSheepDisplay(1, timmyPlayerIndex);
		this.UpdateSheepDisplay(2, shirleyPlayerIndex);
		if (this.currentDisplayState == StorySheepStatusDisplay.DisplayState.OffScreen || this.currentDisplayState == StorySheepStatusDisplay.DisplayState.AnimatingOut)
		{
			this.AnimateOnScreen();
			return;
		}
		this.onScreenTimer = 2f;
	}

	// Token: 0x06000887 RID: 2183 RVA: 0x000389B4 File Offset: 0x00036BB4
	private void UpdateSheepDisplay(int sheepIndex, int playerIndex)
	{
		Color colourForPlayerIndex = this.GetColourForPlayerIndex(playerIndex);
		Sprite numberSpriteForPlayerIndex = this.GetNumberSpriteForPlayerIndex(playerIndex);
		Image image = this.colouredTabImages[sheepIndex];
		this.colouredMarkerImages[sheepIndex].color = colourForPlayerIndex;
		image.color = colourForPlayerIndex;
		Image image2 = this.numberImages[sheepIndex];
		if (numberSpriteForPlayerIndex != null)
		{
			image2.sprite = numberSpriteForPlayerIndex;
			image2.SetNativeSize();
			image2.gameObject.SetActive(true);
			image.gameObject.SetActive(true);
			return;
		}
		image2.gameObject.SetActive(false);
		image.gameObject.SetActive(false);
	}

	// Token: 0x06000888 RID: 2184 RVA: 0x00007FAE File Offset: 0x000061AE
	private Color GetColourForPlayerIndex(int index)
	{
		switch (index)
		{
		case 0:
			return this.playerOneColour;
		case 1:
			return this.playerTwoColour;
		case 2:
			return this.playerThreeColour;
		default:
			return this.noPlayerColour;
		}
	}

	// Token: 0x06000889 RID: 2185 RVA: 0x00007FDF File Offset: 0x000061DF
	private Sprite GetNumberSpriteForPlayerIndex(int index)
	{
		switch (index)
		{
		case 0:
			return this.numberOneSprite;
		case 1:
			return this.numberTwoSprite;
		case 2:
			return this.numberThreeSprite;
		default:
			return null;
		}
	}

	// Token: 0x04000929 RID: 2345
	public Color playerOneColour;

	// Token: 0x0400092A RID: 2346
	public Color playerTwoColour;

	// Token: 0x0400092B RID: 2347
	public Color playerThreeColour;

	// Token: 0x0400092C RID: 2348
	public Color noPlayerColour;

	// Token: 0x0400092D RID: 2349
	public Image[] colouredMarkerImages;

	// Token: 0x0400092E RID: 2350
	public Image[] colouredTabImages;

	// Token: 0x0400092F RID: 2351
	public Image[] numberImages;

	// Token: 0x04000930 RID: 2352
	public Sprite numberOneSprite;

	// Token: 0x04000931 RID: 2353
	public Sprite numberTwoSprite;

	// Token: 0x04000932 RID: 2354
	public Sprite numberThreeSprite;

	// Token: 0x04000933 RID: 2355
	private StorySheepStatusDisplay.DisplayState currentDisplayState;

	// Token: 0x04000934 RID: 2356
	private int[] sheepPlayerIndex = new int[]
	{
		0,
		-1,
		-1
	};

	// Token: 0x04000935 RID: 2357
	private RectTransform rootRectTransform;

	// Token: 0x04000936 RID: 2358
	private TweenBase movementTween;

	// Token: 0x04000937 RID: 2359
	private float onScreenTimer;

	// Token: 0x04000938 RID: 2360
	private const float onScreenDuration = 2f;

	// Token: 0x04000939 RID: 2361
	private const float movementTweenDuration = 0.5f;

	// Token: 0x0400093A RID: 2362
	private const float onScreenPosY = 0f;

	// Token: 0x0400093B RID: 2363
	private const float offScreenPosY = 280f;

	// Token: 0x0400093C RID: 2364
	private int state;

	// Token: 0x02000163 RID: 355
	private enum DisplayState
	{
		// Token: 0x0400093E RID: 2366
		OnScreen,
		// Token: 0x0400093F RID: 2367
		AnimatingOut,
		// Token: 0x04000940 RID: 2368
		OffScreen,
		// Token: 0x04000941 RID: 2369
		AnimatingIn
	}
}
