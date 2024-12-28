using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class CameraNoise : MonoBehaviour
{
    public AudioClip audioClip;
    private AudioSource audioSrc;

    public void Start()
    {
        audioSrc = GetComponent<AudioSource>();
        audioSrc.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartCoroutine(SwishSFX());
        }
        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            StartCoroutine(SwishSFX());
        }
    }

    IEnumerator SwishSFX()
    {
        audioSrc.enabled = true;
        audioSrc.Play();
        yield return new WaitForSeconds(audioSrc.clip.length);
        audioSrc.enabled = false;
        //audioSrc.clip = audioClip;
        //audioSrc.Play();
    }
}
