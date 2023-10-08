using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;


public class LocalSelector : MonoBehaviour
{
    public void ChangeLanguage(int newLanguageID)
    {
        StartCoroutine(SetNewLocal(newLanguageID));
    }
    private IEnumerator SetNewLocal(int languageId)
    {
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[languageId];

        SaveSystem.PlayerSettings.id_local = (Language)languageId;
    }
}
