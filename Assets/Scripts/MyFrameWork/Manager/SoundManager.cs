using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ZFrameWork
{
    public class SoundManager : DDOLSingleton<SoundManager>
    {
        [Header("Music")]
        /// true if the music is enabled	
        public bool m_bMusicOn = true;
        /// the music volume
        [Range(0, 1)]
        public float m_fMusicVolume = 0.3f;

        [Header("Sound Effects")]
        /// true if the sound fx are enabled
        public bool m_fSfxOn = true;
        /// the sound fx volume
        [Range(0, 1)]
        public float m_fSfxVolume = 1f;

        protected AudioSource m_AsbackgroundMusic;

        /// <summary>
        /// Plays a background music.
        /// Only one background music can be active at a time.
        /// </summary>
        /// <param name="Clip">Your audio clip.</param>
        public virtual void PlayBackgroundMusic(AudioSource Music)
        {
            // if the music's been turned off, we do nothing and exit
            if (!m_bMusicOn)
                return;
            // if we already had a background music playing, we stop it
            if (m_AsbackgroundMusic != null)
                m_AsbackgroundMusic.Stop();
            // we set the background music clip
            m_AsbackgroundMusic = Music;
            // we set the music's volume
            m_AsbackgroundMusic.volume = m_fMusicVolume;
            // we set the loop setting to true, the music will loop forever
            m_AsbackgroundMusic.loop = true;
            // we start playing the background music
            m_AsbackgroundMusic.Play();
        }

        /// <summary>
        /// Plays a sound
        /// </summary>
        /// <returns>An audiosource</returns>
        /// <param name="sfx">The sound clip you want to play.</param>
        /// <param name="location">The location of the sound.</param>
        /// <param name="_fVolum">the Sound Volume.</param>
        /// <param name="loop">If set to true, the sound will loop.</param>
        public virtual AudioSource PlaySound(AudioClip sfx, Vector3 location, float _fVolume = 1,bool loop = false)
        {
            if (!m_fSfxOn)
                return null;
            // we create a temporary game object to host our audio source
            GameObject temporaryAudioHost = new GameObject("TempAudio");
            // we set the temp audio's position
            temporaryAudioHost.transform.position = location;
            // we add an audio source to that host
            AudioSource audioSource = temporaryAudioHost.AddComponent<AudioSource>() as AudioSource;
            // we set that audio source clip to the one in paramaters
            audioSource.clip = sfx;
            // we set the audio source volume to the one in parameters
            audioSource.volume = m_fSfxVolume == _fVolume ? m_fMusicVolume : _fVolume;
            // we set our loop setting
            audioSource.loop = loop;
            // we start playing the sound
            audioSource.Play();

            if (!loop)
            {
                // we destroy the host after the clip has played
                Destroy(temporaryAudioHost, sfx.length);
            }

            // we return the audiosource reference
            return audioSource;
        }

        /// <summary>
        /// Stops the looping sounds if there are any
        /// </summary>
        /// <param name="source">Source.</param>
        public virtual void StopLoopingSound(AudioSource source)
        {
            if (source != null)
            {
                Destroy(source.gameObject);
            }
        }
    }
}

