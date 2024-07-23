using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Melanchall.DryWetMidi.Core;
using Melanchall.DryWetMidi.Interaction;
using System.IO;
using System;

public class SongManager : MonoBehaviour
{
    public static SongManager Instance;
    public AudioSource audioSource;
    public Lane[] lanes;
    public float songDelayInSeconds;
    public double marginOfError; // in seconds

    public int inputDelayInMilliseconds;
    public string fileLocation;
    public float noteTime;
    public float noteSpawnY;
    public float noteTapY;
    public float noteDespawnY
    {
        get
        {
            return noteTapY - (noteSpawnY - noteTapY);
        }
    }

    public static MidiFile midiFile;

    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        ReadFromFile();
    }

    private void ReadFromFile()
    {
        try
        {
            string filePath = Path.Combine(Application.streamingAssetsPath, fileLocation);
            midiFile = MidiFile.Read(filePath);
            GetDataFromMidi();
        }
        catch (Exception e)
        {
            Debug.LogError($"Error reading MIDI file: {e.Message}");
        }
    }

    public void GetDataFromMidi()
    {
        var notes = midiFile.GetNotes();
        var array = new Melanchall.DryWetMidi.Interaction.Note[notes.Count];
        notes.CopyTo(array, 0);

        foreach (var lane in lanes)
        {
            lane.SetTimeStamps(array);
        }

        Invoke(nameof(StartSong), songDelayInSeconds);
    }

    public void StartSong()
    {
        audioSource.Play();
        StartCoroutine(WaitAndReturnToMainMenu(audioSource.clip.length + 2f)); // Add 2 seconds to the song length
    }

    private IEnumerator WaitAndReturnToMainMenu(float delay)
    {
        yield return new WaitForSeconds(delay);
        // Load your main menu scene here
        UnityEngine.SceneManagement.SceneManager.LoadScene("MainMenu");
    }

    public static double GetAudioSourceTime()
    {
        return (double)Instance.audioSource.timeSamples / Instance.audioSource.clip.frequency;
    }
}
