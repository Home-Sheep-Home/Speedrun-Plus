// For people who want to directly edit the Assembly-CSharp file here is the code

using System;
using Pixelplacement;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x0200015A RID: 346
public class Story_LevelCompletePopup : AppScreen
{
	// Token: 0x06000842 RID: 2114 RVA: 0x00007BC1 File Offset: 0x00005DC1
	private void Start()
	{
		this.levelUI = base.GetComponentInParent<LevelUI>();
	}

	// Token: 0x06000843 RID: 2115 RVA: 0x000373EC File Offset: 0x000355EC
	public override void Update()
	{
		base.Update();
		switch (this.state)
		{
		case Story_LevelCompletePopup.State.None:
			this.UpdateBonusUnlocked();
			return;
		case Story_LevelCompletePopup.State.Show:
			this.UpdateShow();
			return;
		case Story_LevelCompletePopup.State.Time:
			this.UpdateTime();
			return;
		case Story_LevelCompletePopup.State.Difference:
			this.UpdateDifference();
			return;
		case Story_LevelCompletePopup.State.Stars:
			this.UpdateStars();
			return;
		case Story_LevelCompletePopup.State.Collectables:
			this.UpdateCollectables();
			this.UpdateBonusUnlocked();
			return;
		default:
			return;
		}
	}

	// Token: 0x06000844 RID: 2116 RVA: 0x00037454 File Offset: 0x00035654
	private void UpdateShow()
	{
		this.timer += Time.deltaTime;
		float num = Mathf.Clamp01(this.timer / 0.25f);
		float num2 = Tween.EaseOutBack.Evaluate(num);
		Vector3 localScale = base.transform.localScale;
		localScale.Set(num2, num2, num2);
		this.panel.transform.localScale = localScale;
		Color color = this.blackOverlay.color;
		color.a = 0.5f * num2;
		this.blackOverlay.color = color;
		if (num >= 1f)
		{
			if (this.finishedTime == 0f)
			{
				this.finishedTime = this.timer;
			}
			if (this.timer >= this.finishedTime + 0.3f)
			{
				this.state = Story_LevelCompletePopup.State.Time;
				this.timer = 0f;
				this.finishedTime = 0f;
				AudioController.Play("UI-countUp-05");
			}
		}
	}

	// Token: 0x06000845 RID: 2117 RVA: 0x0003753C File Offset: 0x0003573C
	private void UpdateTime()
	{
		this.timer += Time.deltaTime;
		float num = Mathf.Clamp01(this.timer / 1f);
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)Mathf.Lerp(0f, this.timeTaken, num));
		string text = string.Format("{0:D2}:{1:D2}.{2:D3}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
		if (text.Substring(0, 1) == "0")
		{
			text = text.Remove(0, 1);
		}
		this.timeTakenText.text = text;
		if (num >= 1f)
		{
			if (this.finishedTime == 0f)
			{
				this.finishedTime = this.timer;
			}
			if (this.timer >= this.finishedTime + 0.3f)
			{
				this.timer = 0f;
				this.finishedTime = 0f;
				if (this.timeDifferenceText.text != "")
				{
					this.state = Story_LevelCompletePopup.State.Difference;
					this.timeDifferenceText.gameObject.SetActive(true);
					this.timeDifferenceText.transform.localScale = Vector3.zero;
				}
				else
				{
					this.state = Story_LevelCompletePopup.State.Stars;
					this.tweenIndex = 0;
				}
			}
			AudioController.Stop("UI-countUp-05");
			AudioController.Play("PIZZA-ding-03-microwave");
		}
	}

	// Token: 0x06000846 RID: 2118 RVA: 0x00037698 File Offset: 0x00035898
	private void UpdateDifference()
	{
		this.timer += Time.deltaTime;
		float num = Mathf.Clamp01(this.timer / 0.4f);
		float num2 = Tween.EaseOutBack.Evaluate(num);
		Vector3 localScale = this.timeDifferenceText.transform.localScale;
		localScale.Set(num2, num2, num2);
		this.timeDifferenceText.transform.localScale = localScale;
		if (num >= 1f)
		{
			this.state = Story_LevelCompletePopup.State.Stars;
			this.timer = 0f;
			this.tweenIndex = 0;
		}
	}

	// Token: 0x06000847 RID: 2119 RVA: 0x00037724 File Offset: 0x00035924
	private void UpdateStars()
	{
		float num = this.timer;
		this.timer += Time.deltaTime;
		float num2 = Mathf.Clamp01(this.timer / 0.5f);
		float t = Tween.EaseInOut.Evaluate((num2 <= 0.5f) ? (num2 * 2f) : (1f - (num2 - 0.5f) * 2f));
		float num3 = 0.175f;
		bool flag = num < num3 && this.timer >= num3;
		float num4 = Mathf.Lerp(1f, 1.2f, t);
		if (this.tweenIndex >= this.starCount)
		{
			if (this.finishedTime == 0f)
			{
				this.finishedTime = this.timer;
			}
			if (this.timer >= this.finishedTime + 0.3f)
			{
				this.state = Story_LevelCompletePopup.State.Collectables;
				this.timer = 0f;
				this.finishedTime = 0f;
				this.tweenIndex = 0;
				return;
			}
		}
		else
		{
			if (this.tweenIndex < this.starImages.Length)
			{
				Image image = this.starImages[this.tweenIndex];
				Vector3 localScale = image.transform.localScale;
				localScale.Set(num4, num4, num4);
				image.transform.localScale = localScale;
				if (flag)
				{
					image.sprite = this.starFilledSprite;
					AudioController.Play("UI-star");
				}
			}
			else
			{
				int i = 0;
				int num5 = this.starImages.Length;
				while (i < num5)
				{
					Image image2 = this.starImages[i];
					Vector3 localScale2 = image2.transform.localScale;
					localScale2.Set(num4, num4, num4);
					image2.transform.localScale = localScale2;
					i++;
				}
				if (flag)
				{
					this.starOutline.gameObject.SetActive(true);
					AudioController.Play("UI-star");
					AudioController.Play("UI-star");
				}
			}
			if (num2 >= 1f)
			{
				this.timer = 0f;
				this.tweenIndex++;
			}
		}
	}

	// Token: 0x06000848 RID: 2120 RVA: 0x00037918 File Offset: 0x00035B18
	private void UpdateCollectables()
	{
		if (this.tweenIndex >= this.cupcakesFound + this.socksFound + this.bonusesFound || this.tweenIndex >= this.collectableImages.Length)
		{
			this.state = Story_LevelCompletePopup.State.None;
			this.timer = 0f;
			this.tweenIndex = 0;
		}
		float num = this.timer;
		this.timer += Time.deltaTime;
		float num2 = Mathf.Clamp01(this.timer / 0.5f);
		float t = Tween.EaseInOut.Evaluate((num2 <= 0.5f) ? (num2 * 2f) : (1f - (num2 - 0.5f) * 2f));
		float num3 = 0.175f;
		bool flag = num < num3 && this.timer >= num3;
		float num4 = Mathf.Lerp(1f, 1.2f, t);
		if (this.tweenIndex < this.collectableImages.Length)
		{
			Image image = this.collectableImages[this.tweenIndex];
			Vector3 localScale = image.transform.localScale;
			localScale.Set(num4, num4, num4);
			image.transform.localScale = localScale;
			if (flag)
			{
				if (this.tweenIndex < this.cupcakesFound)
				{
					image.sprite = this.cupcakeCollectableSprite;
				}
				else if (this.tweenIndex < this.cupcakesFound + this.socksFound)
				{
					image.sprite = this.sockCollectableSprite;
				}
				else if (this.tweenIndex < this.cupcakesFound + this.socksFound + this.bonusesFound)
				{
					image.sprite = this.bonusCollectableSprite;
					Vector3 position = this.bonusUnlockedPopup.transform.position;
					position.x = image.transform.position.x;
					this.bonusUnlockedPopup.transform.position = position;
					this.bonusUnlockedPopup.transform.localScale = Vector3.zero;
					this.bonusUnlockedPopup.gameObject.SetActive(true);
					this.bonusUnlockTimer = 0f;
				}
				AudioController.Play("UI-socks_extras");
			}
		}
		if (num2 >= 1f)
		{
			this.timer = 0f;
			this.tweenIndex++;
		}
	}

	// Token: 0x06000849 RID: 2121 RVA: 0x00037B48 File Offset: 0x00035D48
	private void UpdateBonusUnlocked()
	{
		if (this.bonusUnlockTimer < 0f)
		{
			return;
		}
		this.bonusUnlockTimer += Time.deltaTime;
		float num = Mathf.Clamp01(this.bonusUnlockTimer / 0.5f);
		float d = Tween.EaseOutBack.Evaluate(num);
		this.bonusUnlockedPopup.transform.localScale = Vector3.one * d;
		if (num >= 1f)
		{
			this.bonusUnlockTimer = -1f;
		}
	}

	// Token: 0x0600084A RID: 2122 RVA: 0x00037BC4 File Offset: 0x00035DC4
	private void LateUpdate()
	{
		if (this.state == Story_LevelCompletePopup.State.Collectables)
		{
			Vector2 sizeDelta = this.bonusUnlockedBody.rectTransform.sizeDelta;
			sizeDelta.x = this.bonusUnlockedText.rectTransform.sizeDelta.x + 40f;
			this.bonusUnlockedBody.rectTransform.sizeDelta = sizeDelta;
		}
	}

	// Token: 0x0600084B RID: 2123 RVA: 0x00037C20 File Offset: 0x00035E20
	public override void Enable(Action callback, bool instant = false)
	{
		base.Enable(callback, instant);
		base.SetFirstSelectedGameObject(this.firstSelectedGameObject);
		this.bonusUnlockedPopup.gameObject.SetActive(false);
		if (!instant)
		{
			this.timeTakenText.text = "0:00";
			this.timeDifferenceText.gameObject.SetActive(false);
			this.timeDifferenceText.transform.localScale = Vector3.one;
			int i = 0;
			int num = 3;
			while (i < num)
			{
				this.starImages[i].sprite = this.starEmptySprite;
				i++;
			}
			this.starOutline.gameObject.SetActive(false);
			int j = 0;
			int num2 = this.collectableImages.Length;
			while (j < num2)
			{
				this.collectableImages[j].sprite = this.emptyCollectableSprite;
				j++;
			}
			this.timer = 0f;
			this.state = Story_LevelCompletePopup.State.Show;
			this.panel.transform.localScale = Vector3.zero;
			this.blackOverlay.color = Color.clear;
		}
	}

	// Token: 0x0600084C RID: 2124 RVA: 0x00007BCF File Offset: 0x00005DCF
	public void SetIsFinalLevelForChapter(bool finalLevel)
	{
		this.isFinalLevel = finalLevel;
		this.nextLevelButtonText.text = (this.isFinalLevel ? LocalisationManager.Instance().GetTranslation("Finish") : LocalisationManager.Instance().GetTranslation("NLevel"));
	}

	// Token: 0x0600084D RID: 2125 RVA: 0x00037D20 File Offset: 0x00035F20
	public void UpdateLevelInformation(string levelId, float timeTaken, float timeDifference, int starCount, bool firstPlay)
	{
		this.SetupTimings(timeTaken, timeDifference, firstPlay);
		LevelCollectables levelCollectableData = CollectablesDataHandler.Instance().GetLevelCollectableData(levelId);
		this.isBonusLevel = (LevelDataHandler.Instance().GetEpisodeIdForLevelId(levelId) == "bonus");
		if (!LevelDataHandler.Instance().IsLevelTraining(levelId))
		{
			int num = levelCollectableData.maxBonuses + levelCollectableData.maxCakes + levelCollectableData.maxSocks;
			int collectedCakes = levelCollectableData.collectedCakes;
			int collectedSocks = levelCollectableData.collectedSocks;
			int collectedBonuses = levelCollectableData.collectedBonuses;
			this.SetupCollectables(starCount, num, collectedCakes, collectedSocks, collectedBonuses);
			this.trainingText.gameObject.SetActive(false);
			return;
		}
		int i = 0;
		int num2 = this.starImages.Length;
		while (i < num2)
		{
			this.starImages[i].gameObject.SetActive(false);
			i++;
		}
		int j = 0;
		int num3 = this.collectableImages.Length;
		while (j < num3)
		{
			this.collectableImages[j].gameObject.SetActive(false);
			j++;
		}
		string trainingLocalisationKeyForLevelId = LocalisationManager.Instance().GetTrainingLocalisationKeyForLevelId(levelId);
		string translation = LocalisationManager.Instance().GetTranslation(trainingLocalisationKeyForLevelId);
		this.trainingText.text = translation;
		this.trainingText.gameObject.SetActive(true);
	}

	// Token: 0x0600084E RID: 2126 RVA: 0x00037E4C File Offset: 0x0003604C
	public void SetupTimings(float timeTaken, float timeDifference, bool firstPlay)
	{
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)timeTaken);
		string text = string.Format("{0:D2}:{1:D2}.{2:D3}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
		if (text.Substring(0, 1) == "0")
		{
			text = text.Remove(0, 1);
		}
		this.timeTakenText.text = text;
		if (!firstPlay && timeDifference != 0f)
		{
			string text2;
			if (Mathf.Abs(timeDifference) >= 2f)
			{
				text2 = Mathf.Round(Mathf.Abs(timeDifference)).ToString();
			}
			else
			{
				text2 = Mathf.Abs(timeDifference).ToString("F2");
			}
			this.timeDifferenceText.text = string.Concat(new string[]
			{
				LocalisationManager.Instance().GetTranslation("LCompleted"),
				"\n",
				text2,
				" ",
				LocalisationManager.Instance().GetTranslation("Secs"),
				" ",
				(timeDifference > 0f) ? LocalisationManager.Instance().GetTranslation("Quicker") : LocalisationManager.Instance().GetTranslation("Slower")
			});
			this.timeDifferenceText.gameObject.SetActive(true);
		}
		else
		{
			this.timeDifferenceText.text = "";
			this.timeDifferenceText.gameObject.SetActive(false);
		}
		this.timeTaken = timeTaken;
	}

	// Token: 0x0600084F RID: 2127 RVA: 0x00037FC0 File Offset: 0x000361C0
	public void SetupCollectables(int starCount, int collectablesCount, int cupcakesFound, int socksFound, int bonusesFound)
	{
		for (int i = 0; i < 3; i++)
		{
			Image image = this.starImages[i];
			image.sprite = ((i < starCount) ? this.starFilledSprite : this.starEmptySprite);
			image.SetNativeSize();
		}
		this.starOutline.gameObject.SetActive(starCount > 3);
		for (int j = 0; j < this.collectableImages.Length; j++)
		{
			Image image2 = this.collectableImages[j];
			if (j < collectablesCount)
			{
				if (j < cupcakesFound)
				{
					image2.sprite = this.cupcakeCollectableSprite;
				}
				else if (j < cupcakesFound + socksFound)
				{
					image2.sprite = this.sockCollectableSprite;
				}
				else if (j < cupcakesFound + socksFound + bonusesFound)
				{
					image2.sprite = this.bonusCollectableSprite;
				}
				else
				{
					image2.sprite = this.emptyCollectableSprite;
				}
				image2.SetNativeSize();
			}
			image2.gameObject.SetActive(j < collectablesCount);
		}
		this.starCount = starCount;
		this.collectablesCount = collectablesCount;
		this.cupcakesFound = cupcakesFound;
		this.socksFound = socksFound;
		this.bonusesFound = bonusesFound;
	}

	// Token: 0x06000850 RID: 2128 RVA: 0x00007C0B File Offset: 0x00005E0B
	public void ReplayButtonPressed()
	{
		if (this.TryStopTweening())
		{
			return;
		}
		this.levelUI.OnReplayLevelButtonPressed(true);
		AudioController.Play("ui_select");
	}

	// Token: 0x06000851 RID: 2129 RVA: 0x00007C2D File Offset: 0x00005E2D
	public void LevelsButtonPressed()
	{
		if (this.TryStopTweening())
		{
			return;
		}
		Menu_ScreenManager.bootToStoryLevelSelect = true;
		PlayerManager.instance.ReturnToMenu();
		AudioController.Play("ui_select");
	}

	// Token: 0x06000852 RID: 2130 RVA: 0x000380B8 File Offset: 0x000362B8
	public void NextLevelButtonPressed()
	{
		if (this.TryStopTweening())
		{
			return;
		}
		if (this.isFinalLevel)
		{
			if (!this.isBonusLevel)
			{
				this.levelUI.OnFinalLevelButtonPressed();
			}
			else
			{
				Menu_ScreenManager.bootToStoryChapterSelect = true;
				PlayerManager.instance.ReturnToMenu();
			}
		}
		else
		{
			this.levelUI.OnNextLevelButtonPressed();
		}
		AudioController.Play("ui_select");
	}

	// Token: 0x06000853 RID: 2131 RVA: 0x00038114 File Offset: 0x00036314
	private bool TryStopTweening()
	{
		if (this.state == Story_LevelCompletePopup.State.None)
		{
			return false;
		}
		this.panel.transform.localScale = Vector3.one;
		TimeSpan timeSpan = TimeSpan.FromSeconds((double)this.timeTaken);
		string text = string.Format("{0:D2}:{1:D2}.{2:D3}", timeSpan.Minutes, timeSpan.Seconds, timeSpan.Milliseconds);
		if (text.Substring(0, 1) == "0")
		{
			text = text.Remove(0, 1);
		}
		this.timeTakenText.text = text;
		this.timeDifferenceText.gameObject.SetActive(true);
		int i = 0;
		int num = 3;
		while (i < num)
		{
			Image image = this.starImages[i];
			image.sprite = ((i < this.starCount) ? this.starFilledSprite : this.starEmptySprite);
			image.transform.localScale = Vector3.one;
			i++;
		}
		this.starOutline.gameObject.SetActive(this.starCount > 3);
		int j = 0;
		int num2 = this.collectableImages.Length;
		while (j < num2)
		{
			Image image2 = this.collectableImages[j];
			if (j < this.collectablesCount)
			{
				if (j < this.cupcakesFound)
				{
					image2.sprite = this.cupcakeCollectableSprite;
				}
				else if (j < this.cupcakesFound + this.socksFound)
				{
					image2.sprite = this.sockCollectableSprite;
				}
				else if (j < this.cupcakesFound + this.socksFound + this.bonusesFound)
				{
					image2.sprite = this.bonusCollectableSprite;
					Vector3 position = this.bonusUnlockedPopup.transform.position;
					position.x = image2.transform.position.x;
					this.bonusUnlockedPopup.transform.position = position;
					this.bonusUnlockedPopup.transform.localScale = Vector3.one;
					this.bonusUnlockedPopup.gameObject.SetActive(true);
				}
				else
				{
					image2.sprite = this.emptyCollectableSprite;
				}
				image2.SetNativeSize();
			}
			image2.gameObject.SetActive(j < this.collectablesCount);
			image2.transform.localScale = Vector3.one;
			j++;
		}
		this.state = Story_LevelCompletePopup.State.None;
		this.timer = 0f;
		base.transform.localScale = Vector3.one;
		AudioController.Stop("UI-countUp-05");
		return true;
	}

	// Token: 0x040008DA RID: 2266
	public const float ShowTweenDuration = 0.25f;

	// Token: 0x040008DB RID: 2267
	public const float TimeCountUpDuration = 1f;

	// Token: 0x040008DC RID: 2268
	public const float DifferenceTweenDuraton = 0.4f;

	// Token: 0x040008DD RID: 2269
	public const float StarsTweenDuraton = 0.5f;

	// Token: 0x040008DE RID: 2270
	public const float StarsTweenUpScale = 1.2f;

	// Token: 0x040008DF RID: 2271
	public const float CollectablesTweenDuraton = 0.5f;

	// Token: 0x040008E0 RID: 2272
	public const float CollectablesTweenUpScale = 1.2f;

	// Token: 0x040008E1 RID: 2273
	public const float TimeBetween = 0.3f;

	// Token: 0x040008E2 RID: 2274
	public GameObject panel;

	// Token: 0x040008E3 RID: 2275
	public Image blackOverlay;

	// Token: 0x040008E4 RID: 2276
	public Text timeTakenText;

	// Token: 0x040008E5 RID: 2277
	public Text timeDifferenceText;

	// Token: 0x040008E6 RID: 2278
	public Text nextLevelButtonText;

	// Token: 0x040008E7 RID: 2279
	public Text trainingText;

	// Token: 0x040008E8 RID: 2280
	public Image[] starImages;

	// Token: 0x040008E9 RID: 2281
	public Image starOutline;

	// Token: 0x040008EA RID: 2282
	public Sprite starFilledSprite;

	// Token: 0x040008EB RID: 2283
	public Sprite starEmptySprite;

	// Token: 0x040008EC RID: 2284
	public Image[] collectableImages;

	// Token: 0x040008ED RID: 2285
	public Sprite emptyCollectableSprite;

	// Token: 0x040008EE RID: 2286
	public Sprite cupcakeCollectableSprite;

	// Token: 0x040008EF RID: 2287
	public Sprite sockCollectableSprite;

	// Token: 0x040008F0 RID: 2288
	public Sprite bonusCollectableSprite;

	// Token: 0x040008F1 RID: 2289
	public GameObject bonusUnlockedPopup;

	// Token: 0x040008F2 RID: 2290
	public Image bonusUnlockedBody;

	// Token: 0x040008F3 RID: 2291
	public Text bonusUnlockedText;

	// Token: 0x040008F4 RID: 2292
	private LevelUI levelUI;

	// Token: 0x040008F5 RID: 2293
	private Story_LevelCompletePopup.State state;

	// Token: 0x040008F6 RID: 2294
	private float timer;

	// Token: 0x040008F7 RID: 2295
	private float bonusUnlockTimer = -1f;

	// Token: 0x040008F8 RID: 2296
	private float timeTaken;

	// Token: 0x040008F9 RID: 2297
	private float finishedTime;

	// Token: 0x040008FA RID: 2298
	private int starCount;

	// Token: 0x040008FB RID: 2299
	private int collectablesCount;

	// Token: 0x040008FC RID: 2300
	private int cupcakesFound;

	// Token: 0x040008FD RID: 2301
	private int socksFound;

	// Token: 0x040008FE RID: 2302
	private int bonusesFound;

	// Token: 0x040008FF RID: 2303
	private int tweenIndex;

	// Token: 0x04000900 RID: 2304
	private bool isFinalLevel;

	// Token: 0x04000901 RID: 2305
	private bool isBonusLevel;

	// Token: 0x0200015B RID: 347
	public enum State
	{
		// Token: 0x04000903 RID: 2307
		None,
		// Token: 0x04000904 RID: 2308
		Show,
		// Token: 0x04000905 RID: 2309
		Time,
		// Token: 0x04000906 RID: 2310
		Difference,
		// Token: 0x04000907 RID: 2311
		Stars,
		// Token: 0x04000908 RID: 2312
		Collectables
	}
}
