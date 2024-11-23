using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
	[SerializeField] float FadeDuration = 0.5f;

	public static AudioManager Instance;
	Dictionary<string, Coroutine> RunningCoroutineFades;

	private void Awake()
	{
		if (Instance == null)
		{
			Instance = this;
			RunningCoroutineFades = new Dictionary<string, Coroutine>();
			DontDestroyOnLoad(this.gameObject);
		}
		else if (Instance != this)
		{
			Destroy(gameObject);
		}
	}

	public AudioSource this[string name]
	{
		get
		{
			try
			{
				return transform.Find(name).GetComponent<AudioSource>();
			}
			catch
			{
				return null;
			}
		}
	}
	public AudioSource this[int index]
	{
		get
		{
			try
			{
				return transform.GetChild(index).GetComponent<AudioSource>();
			}
			catch
			{
				return null;
			}
		}
	}

	public void PlayWithFadeIn(AudioClip clip, string audioSourceName)
	{
		PlayWithFadeIn(clip, this[audioSourceName]);
	}

	public void PlayWithFadeIn(AudioClip clip, string audioSourceName, float fadeDuration)
	{
		PlayWithFadeIn(clip, this[audioSourceName], fadeDuration);
	}

	public void PlayWithCrossFade(AudioClip clip, string audioSourceName)
	{
		PlayWithCrossFade(clip, audioSourceName, FadeDuration);
	}
	public void PlayWithCrossFade(AudioClip clip, string audioSourceName, float fadeDuration)
	{
		AudioSource currentAudio = this[audioSourceName];
		AudioSource newAudio = Instantiate(currentAudio, transform);

		newAudio.transform.SetSiblingIndex(currentAudio.transform.GetSiblingIndex());
		currentAudio.transform.SetAsLastSibling();

		FadeOutAudio(currentAudio);
		PlayWithFadeIn(clip, newAudio);

		newAudio.gameObject.name = audioSourceName;

		Destroy(currentAudio.gameObject, fadeDuration);
	}
	public void PlayWithFadeIn(AudioClip clip, AudioSource audio)
	{
		PlayWithFadeIn(clip, audio, FadeDuration);
	}

	public void PlayWithFadeIn(AudioClip clip, AudioSource audio, float fadeDuration)
	{
		if (clip == audio.clip) return;

		if (audio.isPlaying && audio.volume > 0)
		{
			FadeOutAudio(audio, fadeDuration, () =>
			{
				audio.clip = clip;
				audio.Play();
				FadeInAudio(audio, fadeDuration);
			});
		}
		else
		{
			audio.clip = clip;
			audio.Play();
			FadeInAudio(audio, fadeDuration);
		}
	}

	public void FadeInAudio(AudioSource audio)
	{
		FadeInAudio(audio, FadeDuration);
	}

	public void FadeInAudio(AudioSource audio, float fadeDuration)
	{
		if (RunningCoroutineFades.ContainsKey(audio.gameObject.name))
		{
			if (RunningCoroutineFades[audio.gameObject.name] != null)
			{
				StopCoroutine(RunningCoroutineFades[audio.gameObject.name]);
			}
			RunningCoroutineFades.Remove(audio.gameObject.name);
		}

		Coroutine newRoutine = StartCoroutine(ChangeAudioVolume(audio, 0, 1, fadeDuration));

		RunningCoroutineFades.Add(audio.gameObject.name, newRoutine);
	}

	public void FadeOutAudio(string audioName)
	{
		FadeOutAudio(this[audioName]);
	}
	public void FadeOutAudio(string audioName, float fadeDuration)
	{
		FadeOutAudio(this[audioName], fadeDuration);
	}
	public void FadeOutAudio(AudioSource audio)
	{
		FadeOutAudio(audio, FadeDuration);
	}

	public void FadeOutAudio(AudioSource audio, float fadeDutaion, Action onComplete = null)
	{
		//Debug.Log("Trying to fade out");
		if (RunningCoroutineFades.ContainsKey(audio.gameObject.name))
		{
			if (RunningCoroutineFades[audio.gameObject.name] != null)
			{
				StopCoroutine(RunningCoroutineFades[audio.gameObject.name]);
			}
			RunningCoroutineFades.Remove(audio.gameObject.name);
		}

		Coroutine newRoutine = StartCoroutine(ChangeAudioVolume(audio, 1, 0, fadeDutaion, onComplete));

		RunningCoroutineFades.Add(audio.gameObject.name, newRoutine);
	}

	IEnumerator ChangeAudioVolume(AudioSource audio, float initialVolume, float finalVolume, float fadeDuration, Action onComplete = null)
	{
		float f = 0;
		while (audio != null && audio.volume != finalVolume)
		{
			//if (audio.isPlaying)
			//{
			f += Time.unscaledDeltaTime / fadeDuration;
			audio.volume = Mathf.Lerp(initialVolume, finalVolume, f);
			//}
			yield return null;
		}

		onComplete?.Invoke();

		if (audio != null)
			RunningCoroutineFades.Remove(audio.gameObject.name);
	}

	public static void PlaySound(string clip)
	{
		if (clip == null || clip == "") return;
		if (Instance == null) return;
		try
		{
			if (Instance[clip].isPlaying) return;
			Instance[clip]?.Play();
		}
		catch(Exception e)
        {
			Debug.Log("AudioManager exception" + e);
        }
	}

	public static void StopSound(string clip)
	{
		if (Instance == null) return;
		try
		{
			if (!Instance[clip].isPlaying) return;
			Instance[clip]?.Stop();
		}
		catch (Exception e)
		{
			Debug.Log("AudioManager exception" + e);
		}
	}
}