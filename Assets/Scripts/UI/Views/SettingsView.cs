using DreamedReality.Managers;
using DreamedReality.UI.Elements;
using System.Collections.Generic;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

namespace DreamedReality.UI.Views
{
    public class SettingsView : BaseUIView
    {
        private const string LANGUAGE_DROPDOWN_NAME = "LanguageDropdown";
        private const string SOUND_SLIDER_NAME = "SoundSlider";
        private const string MUSIC_SLIDER_NAME = "MusicSlider";

        public new class UxmlFactory : UxmlFactory<SettingsView> { }

        private List<Locale> m_locales;
        private DropdownField m_languageDropdown;
        private Slider m_soundSlider;
        private Slider m_musicSlider;

        public SettingsView()
        {
            RegisterCallback<AttachToPanelEvent>(OnAttachToPanel);
            RegisterCallback<DetachFromPanelEvent>(OnDetachFromPanel);
        }

        private void OnAttachToPanel(AttachToPanelEvent ev)
        {
            m_languageDropdown = this.Q<DropdownField>(LANGUAGE_DROPDOWN_NAME);
            m_soundSlider = this.Q<Slider>(SOUND_SLIDER_NAME);
            m_musicSlider = this.Q<Slider>(MUSIC_SLIDER_NAME);

            m_languageDropdown?.RegisterCallback<ChangeEvent<string>>(
                OnLanguageDropdownChange
            );
            m_soundSlider?.RegisterCallback<ChangeEvent<float>>(
                OnSoundSliderChange
            );
            m_musicSlider?.RegisterCallback<ChangeEvent<float>>(
                OnMusicSliderChange
            );

            UpdateLanguageDropdown();

            schedule.Execute(UpdateSliders).StartingIn(10);
        }

        private void OnDetachFromPanel(DetachFromPanelEvent ev)
        {
            m_languageDropdown?.UnregisterCallback<ChangeEvent<string>>(
                OnLanguageDropdownChange
            );
            m_soundSlider?.UnregisterCallback<ChangeEvent<float>>(
                OnSoundSliderChange
            );
            m_musicSlider?.UnregisterCallback<ChangeEvent<float>>(
                OnMusicSliderChange
            );

            m_languageDropdown = null;
            m_soundSlider = null;
            m_musicSlider = null;
        }

        private void UpdateLanguageDropdown()
        {
            if (m_languageDropdown == null)
            {
                return;
            }

            var currLocale = LocalizationSettings.SelectedLocale;
            m_locales = LocalizationSettings.AvailableLocales.Locales;

            int valueIndex = 0;
            var languages = new List<string>(m_locales.Count);
            for (int i = 0; i < m_locales.Count; ++i)
            {
                var identifier = m_locales[i].Identifier;
                languages.Add(identifier.CultureInfo.NativeName);
                if (m_locales[i] == currLocale)
                {
                    valueIndex = i;
                }
            }

            m_languageDropdown.choices = languages;
            m_languageDropdown.index = valueIndex;
        }

        private void UpdateSliders()
        {
            var audioManager = AudioManager.Instance;
            if (audioManager == null)
            {
                return;
            }

            if (m_soundSlider != null)
            {
                m_soundSlider.value = audioManager.SoundsVolume;
            }

            if (m_musicSlider != null)
            {
                m_musicSlider.value = audioManager.MusicVolume;
            }
        }

        private void OnLanguageDropdownChange(ChangeEvent<string> ev)
        {
            LocalizationSettings.SelectedLocale = m_locales[m_languageDropdown.index];
        }

        private void OnSoundSliderChange(ChangeEvent<float> ev)
        {
            AudioManager.Instance.SoundsVolume = ev.newValue;
        }

        private void OnMusicSliderChange(ChangeEvent<float> ev)
        {
            AudioManager.Instance.MusicVolume = ev.newValue;
        }
    }
}
