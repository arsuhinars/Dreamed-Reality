using System;
using System.Collections.Generic;
using UnityEngine;

namespace DreamedReality.Managers
{
    public enum SfxType
    {
        None,
        UIClick, UISwitch,
        NoteOpen, NoteClose,
        DoorOpen, DoorClose,
        GateOpen, GateClose,
        Footstep,
        ButtonClick,
        CodeSwitch,
        ItemPickup, ItemPut,
        ObjectPush,
        GoingToBed,
        StoneWallOpen,
        WaterSplash
    }

    public class AudioManager : MonoBehaviour
    {
        [Serializable]
        private struct AudioClipData
        {
            public SfxType sfxType;
            public AudioClip[] randomClips;
            [Range(0f, 1f)]
            public float volume;
            [Space]
            public bool isSpacial;
            public float minDistance;
            public float maxDistance;
        }

        [Serializable]
        private struct AmbientSource
        {
            public AudioSource audioSource;
            public bool isMusic;
            [Range(0f, 1f)]
            public float volume;
        }

        private const string MUSIC_VOLUME_PREFS_KEY = "MusicVolume";
        private const string SOUNDS_VOLUME_PREFS_KEY = "SoundsVolume";

        public static AudioManager Instance { get; private set; } = null;

        public float MusicVolume
        {
            get
            {
                m_musicVolume ??= PlayerPrefs.GetFloat(MUSIC_VOLUME_PREFS_KEY, 1f);
                return (float)m_musicVolume;
            }
            set
            {
                m_musicVolume = value;
                PlayerPrefs.SetFloat(MUSIC_VOLUME_PREFS_KEY, value);
                UpdateSourcesVolumes();
            }
        }
        public float SoundsVolume
        {
            get
            {
                m_soundsVolume ??= PlayerPrefs.GetFloat(SOUNDS_VOLUME_PREFS_KEY, 1f);
                return (float)m_soundsVolume;
            }
            set
            {
                m_soundsVolume = value;
                PlayerPrefs.SetFloat(SOUNDS_VOLUME_PREFS_KEY, value);
                UpdateSourcesVolumes();
            }
        }
        
        [SerializeField] private AudioSource m_audioSourcePrefab;
        [SerializeField] private int m_audioPoolSize;
        [SerializeField] private AudioClipData[] m_audioClips;
        [Space]
        [SerializeField] private AmbientSource[] m_ambientSources;

        private Dictionary<SfxType, AudioClipData> m_soundsByType;
        private LinkedList<AudioSource> m_activeSources;
        private LinkedList<AudioSource> m_inactiveSources;

        private float? m_musicVolume;
        private float? m_soundsVolume;

        public void PlaySound(SfxType type)
        {
            PlaySound(type, Vector3.zero);
        }

        public void PlaySound(SfxType type, Vector3 position)
        {
            if (type == SfxType.None)
            {
                return;
            }

            if (!m_soundsByType.TryGetValue(type, out var data))
            {
                Debug.LogError($"Audio clip with type {type} doesn't exist");
                return;
            }

            var source = GetSourceFromPool();
            if (source == null)
            {
                return;
            }

            var clips = data.randomClips;
            int idx = UnityEngine.Random.Range(0, clips.Length);

            source.clip = data.randomClips[idx];
            source.volume = data.volume * SoundsVolume;
            source.spatialBlend = data.isSpacial ? 1f : 0f;
            source.minDistance = data.minDistance;
            source.maxDistance = data.maxDistance;
            source.transform.position = position;
            source.Play();
        }

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else
            {
                Debug.LogWarning("Singleton already exists");
                Destroy(this);
            }
        }

        private void Start()
        {
            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.OnStart += OnGameStart;
                gameManager.OnEnd += OnGameEnd;
                gameManager.OnPause += OnGamePause;
                gameManager.OnResume += OnGameResume;
            }

            LevelManager.Instance.OnSceneStartedLoading += OnSceneStartedLoading;

            InitializePool();
            UpdateSourcesVolumes();
        }

        private void OnDestroy()
        {
            if (Instance == this)
            {
                Instance = null;
            }

            var gameManager = GameManager.Instance;
            if (gameManager != null)
            {
                gameManager.OnStart -= OnGameStart;
                gameManager.OnEnd -= OnGameEnd;
                gameManager.OnPause -= OnGamePause;
                gameManager.OnResume -= OnGameResume;
            }

            if (LevelManager.Instance != null)
            {
                LevelManager.Instance.OnSceneStartedLoading -= OnSceneStartedLoading;
            }

            CleanupPool();
        }

        private void Update()
        {
            var it = m_activeSources.First;
            while (it != null)
            {
                var jt = it.Next;

                var src = it.Value;
                if (!src.isPlaying)
                {
                    m_activeSources.Remove(it);
                    m_inactiveSources.AddLast(src);
                }

                it = jt;
            }
        }

        private void OnGameStart()
        {
            foreach (var source in m_activeSources)
            {
                source.Stop();
            }

            for (int i = 0; i < m_ambientSources.Length; ++i)
            {
                var src = m_ambientSources[i].audioSource;
                src.Stop();
                src.Play();
            }
        }

        private void OnGameEnd(GameEndReason reason)
        {
            for (int i = 0; i < m_ambientSources.Length; ++i)
            {
                m_ambientSources[i].audioSource.Pause();
            }
        }

        private void OnGamePause()
        {
            foreach (var source in m_activeSources)
            {
                source.Pause();
            }

            for (int i = 0; i < m_ambientSources.Length; ++i)
            {
                m_ambientSources[i].audioSource.Pause();
            }
        }

        private void OnGameResume()
        {
            foreach (var source in m_activeSources)
            {
                source.UnPause();
            }

            for (int i = 0; i < m_ambientSources.Length; ++i)
            {
                m_ambientSources[i].audioSource.UnPause();
            }
        }

        private void OnSceneStartedLoading()
        {
            for (int i = 0; i < m_ambientSources.Length; ++i)
            {
                m_ambientSources[i].audioSource.Pause();
            }
        }

        private void InitializePool()
        {
            m_soundsByType = new();
            for (int i = 0; i < m_audioClips.Length; ++i)
            {
                var data = m_audioClips[i];
                if (!m_soundsByType.TryAdd(data.sfxType, data))
                {
                    Debug.LogError("AudioManager audio clips must be unique");
                }
            }

            m_activeSources = new();
            m_inactiveSources = new();

            for (int i = 0; i < m_audioPoolSize; ++i)
            {
                var source = Instantiate(m_audioSourcePrefab, transform);

                source.loop = false;
                source.playOnAwake = false;
                source.Stop();

                m_inactiveSources.AddLast(source);
            }
        }

        private void CleanupPool()
        {
            foreach (var source in m_activeSources)
            {
                Destroy(source.gameObject);
            }

            foreach (var source in m_inactiveSources)
            {
                Destroy(source.gameObject);
            }

            m_soundsByType.Clear();
            m_activeSources.Clear();
            m_inactiveSources.Clear();
        }

        private AudioSource GetSourceFromPool()
        {
            var src = m_inactiveSources.Last;
            if (src == null)
            {
                return null;
            }
            m_activeSources.AddLast(src.Value);
            m_inactiveSources.RemoveLast();
            return src.Value;
        }

        private void UpdateSourcesVolumes()
        {
            for (int i = 0; i < m_ambientSources.Length; ++i)
            {
                var src = m_ambientSources[i];

                src.audioSource.volume = src.isMusic ? MusicVolume : SoundsVolume;
                src.audioSource.volume *= src.volume;
            }
        }
    }
}
