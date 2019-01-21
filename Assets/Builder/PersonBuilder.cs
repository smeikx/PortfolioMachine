using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;

public class PersonBuilder : MonoBehaviour
{
	[Header("Personendaten")]
	[SerializeField] TextAsset name;
	[SerializeField] TextAsset vertiefung;
	[SerializeField] TextAsset interessen;

	[Header("Medien-Dateien")]
	[SerializeField] Object[] mediaAssets = new Object[9];
	[Header("Projekt-Nr")]
	[SerializeField] int[] projektNr = new int[9];
	[Header("Projekt-Titel")]
	[SerializeField] TextAsset[] titel = new TextAsset[9];
	[Header("Beschreibungen")]
	[SerializeField] TextAsset[] beschreibung = new TextAsset[9];
	[Header("Aufgabenbereiche")]
	[SerializeField] TextAsset[] aufgabenbereich = new TextAsset[9];
	[Header("Sound-Bilder")]
	[SerializeField] Texture2D[] soundBilder = new Texture2D[9];

	PersonData personData;
	Transform mediumContainer;
	MediumData[] mediumData;

	string nameVertiefung;


	void Start()
	{
		personData = GetComponent<PersonData>();

		personData.name.text = name.text;
		personData.nameVertiefung.text = nameVertiefung = name.text + vertiefung.text;
		personData.interessen.text = interessen.text;


		mediumContainer = personData.mediums;

		int nrOfAvailaibleSlots = mediumContainer.childCount;
		int nrOfUsedSlots = 0;
		for (; nrOfUsedSlots < mediaAssets.Length && mediaAssets[nrOfUsedSlots]; nrOfUsedSlots++);

		mediumData = new MediumData[nrOfUsedSlots];
		for (int i = 0; i < nrOfUsedSlots; i++)
			mediumData[i] = mediumContainer.GetChild(i).GetComponent<MediumData>();
		
		// lösche ungenutzte Medium-Slots
		for (int i = nrOfAvailaibleSlots - 1; i >= nrOfUsedSlots; i--)
			Destroy(mediumContainer.GetChild(i).gameObject);


		// füge Medien-Assets ein
		for (int i = 0; i < nrOfUsedSlots; i++)
		{
			var mediaAsset = mediaAssets[i];
			System.Type type = mediaAsset.GetType();
			MediumData medium = mediumData[i];
			
			if (type == typeof(Texture2D))
				SetImage(medium, (Texture2D)mediaAsset);
			else if (type == typeof(VideoClip))
				SetVideo(medium, (VideoClip)mediaAsset);
			else if (type == typeof(AudioClip))
				SetSound(medium, (AudioClip)mediaAsset, soundBilder[i]);

			SetText(medium, i);
		}


		// lösche Setup-Komponenten

		/* Assets können wohl nur über Editor-Scripts gelöscht werden
		Destroy(name);
		Destroy(vertiefung);
		Destroy(interessen);
		foreach (TextAsset tc in titel) Destroy(tc);
		foreach (TextAsset tc in beschreibung) Destroy(tc);
		foreach (TextAsset tc in aufgabenbereich) Destroy(tc);
		*/

		Destroy(personData);
		foreach (MediumData md in mediumData) Destroy(md);

		Destroy(this);
	}


	void SetImage(MediumData medium, Texture2D texture)
	{
		Material mat = medium.slate.GetComponent<Renderer>().material;
		mat.mainTexture = texture;
	}

	
	void SetVideo(MediumData medium, VideoClip video)
	{
		VideoPlayer vp = medium.slate.GetComponent<VideoPlayer>();
		vp.enabled = true;
		vp.clip = video;
	}


	void SetSound(MediumData medium, AudioClip audio, Texture2D image)
	{
		AudioSource audiosource = medium.slate.GetComponent<AudioSource>();
		audiosource.enabled = true;
		audiosource.clip = audio;

		SetImage(medium, image);
	}


	void SetText(MediumData medium, int index)
	{
		medium.nameVertiefung.text = nameVertiefung;
		medium.projektNr.text = projektNr[index] > 0 ? projektNr[index].ToString("00") : "";
		medium.projektTitel.text = titel[index] ? titel[index].text : "";
		medium.beschreibung.text = beschreibung[index] ? beschreibung[index].text : "";
		medium.aufgabenbereich.text = aufgabenbereich[index] ? aufgabenbereich[index].text : "";
	}
}
