﻿using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator
{
    /// <summary>
    /// Dictionary containg all translations
    /// </summary>
    private static Dictionary<string, Dictionary<string, string>> _translations = new Dictionary<string, Dictionary<string, string>>()
    {
        ["pl"] = new Dictionary<string, string>()
        {
            ["HelpPage.HeaderTitle"] = "Pomoc",
            ["SettingsPage.HeaderTitle"] = "Ustawienia aplikacji",
            ["TranslationWindow.ContentText"] = "Proszę, wybierz język aplikacji",
            ["TranslationWindow.PolishButtonText"] = "POLSKI",
            ["TranslationWindow.EnglishButtonText"] = "ANGIELSKI",
            ["Menu.UserMenuItemText"] = "Użytkownik",
            ["Menu.SettingsMenuItemText"] = "Ustawienia aplikacji",
            ["Menu.LanguageMenuItemText"] = "Język",
            ["Menu.HelpMenuItemText"] = "Pomoc",
            ["Menu.ExitMenuItemText"] = "Zakończ",
            ["Menu.LogOutMenuItemText"] = "Wyloguj",
            ["MainPage.ModelsLabelText"] = "Twoje modele",
            ["Menu.ExitWindowDialog.ContentText"] = "Czy na pewno chcesz zamknąć aplikację?",
            ["Menu.ExitWindowDialog.YesButtonText"] = "TAK",
            ["Menu.ExitWindowDialog.CancelButtonText"] = "ANULUJ",
            ["Menu.LogOutWindowDialog.ContentText"] = "Czy na pewno chcesz się wylogować?",
            ["Menu.ExitWindowDialog.YesButtonText"] = "TAK",
            ["Menu.ExitWindowDialog.NoButtonText"] = "NIE",
            ["LoginPage.LoginInputPlaceholder"] = "Email",
            ["LoginPage.PasswordInputPlaceholder"] = "Hasło",
            ["LoginPage.LoginButtonText"] = "Zaloguj",
            ["LoginPage.MainLabelText"] = "Logowanie",
            ["ModelItem.StartDownloadFileButtonText"] = "Pobierz",
            ["ModelItem.StopDownloadFileButtonText"] = "Zatrzymaj pobieranie",
            ["ModelItem.RemoveFileButtonText"] = "Usuń plik",
            ["ModelItem.ShowModelButtonText"] = "Wyświetl",
            ["DeleteModelFileWindow.ContentText"] = "Czy na pewno chcesz usunąć plik modelu",
            ["DeleteModelFileWindow.YesButtonText"] = "TAK",
            ["DeleteModelFileWindow.NoButtonText"] = "NIE",
            ["StopModelDownloading.ContentText"] = "Czy na pewno chcesz zatrzymać pobiernie pliku modelu",
            ["StopModelDownloading.YesButtonText"] = "TAK",
            ["StopModelDownloading.NoButtonText"] = "NIE",
            ["ErrorDialogWindow.OkButtonText"] = "OK",
            ["Errors.LoggingIn.HttpResponseErrorCode400"] = "Nieprawidłowy login lub hasło",
            ["Errors.LoggingIn.ConnectionError"] = "Błąd połączenia z serwerem",
            ["Errors.RefreshingData.ConnectionError"] = "Błąd połączenia z serwerem",
            ["Errors.RefreshingData.HttpResponseErrorCode401"] = "Dostęp zabroniony",
            ["Errors.RefreshingData.HttpResponseErrorCode403"] = "Dostęp zabroniony",
            ["Errors.RefreshingData.HttpResponseErrorCode404"] = "Nie znaleziono użytkownika",
            ["Errors.DownloadingError.DownloadingError"] = "Błąd podczas pobierania pliku"
        },
        
        ["en"] = new Dictionary<string, string>()
        {
            ["HelpPage.HeaderTitle"] = "Help",
            ["SettingsPage.HeaderTitle"] = "Application settings",
            ["TranslationWindow.ContentText"] = "Please choose application language",
            ["TranslationWindow.PolishButtonText"] = "POLISH",
            ["TranslationWindow.EnglishButtonText"] = "ENGLISH",
            ["Menu.UserMenuItemText"] = "User",
            ["Menu.SettingsMenuItemText"] = "Application settings",
            ["Menu.LanguageMenuItemText"] = "Language",
            ["Menu.HelpMenuItemText"] = "Help",
            ["Menu.ExitMenuItemText"] = "Exit",
            ["Menu.LogOutMenuItemText"] = "Log out",
            ["MainPage.ModelsLabelText"] = "Your models",
            ["Menu.ExitWindowDialog.ContentText"] = "Do you really want to quit?",
            ["Menu.ExitWindowDialog.YesButtonText"] = "YES",
            ["Menu.ExitWindowDialog.CancelButtonText"] = "CANCEL",
            ["Menu.LogOutWindowDialog.ContentText"] = "Do you really want to log out?",
            ["Menu.ExitWindowDialog.YesButtonText"] = "YES",
            ["Menu.ExitWindowDialog.NoButtonText"] = "NO",
            ["LoginPage.LoginInputPlaceholder"] = "Email",
            ["LoginPage.PasswordInputPlaceholder"] = "Password",
            ["LoginPage.LoginButtonText"] = "Log in",
            ["LoginPage.MainLabelText"] = "Logging in",
            ["ModelItem.StartDownloadFileButtonText"] = "Download",
            ["ModelItem.StopDownloadFileButtonText"] = "Stop the download",
            ["ModelItem.RemoveFileButtonText"] = "Delete file",
            ["ModelItem.ShowModelButtonText"] = "Display",
            ["DeleteModelFileWindow.ContentText"] = "Do you really want to delete file of model",
            ["DeleteModelFileWindow.YesButtonText"] = "YES",
            ["DeleteModelFileWindow.NoButtonText"] = "NO",
            ["StopModelDownloading.ContentText"] = "Do you really want to stop downloading file of model",
            ["StopModelDownloading.YesButtonText"] = "YES",
            ["StopModelDownloading.NoButtonText"] = "NO",
            ["ErrorDialogWindow.OkButtonText"] = "OK",
            ["Errors.LoggingIn.HttpResponseErrorCode400"] = "Invalid user or password",
            ["Errors.LoggingIn.ConnectionError"] = "Error while connecting to server",
            ["Errors.RefreshingData.ConnectionError"] = "Error while connecting to server",
            ["Errors.RefreshingData.HttpResponseErrorCode401"] = "Forbidden access",
            ["Errors.RefreshingData.HttpResponseErrorCode403"] = "Forbidden access",
            ["Errors.RefreshingData.HttpResponseErrorCode404"] = "No user found",
            ["Errors.DownloadingError.DownloadingError"] = "Error while downloading file"

        }
    };

    /// <summary>
    /// Actual selected language
    /// </summary>
    private static string _selectedLang = "pl";

    /// <summary>
    /// Method for getting translation based on key
    /// if selected lang or key does not exist - return key
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public static string GetTranslation(string key)
    {
        if (!Translator._translations.ContainsKey(Translator._selectedLang)) return key;

        var translationsSet = Translator._translations[Translator._selectedLang];

        if (!translationsSet.ContainsKey(key)) return key;

        return translationsSet[key];
    }

    /// <summary>
    /// Method for getting all possible languages
    /// </summary>
    /// <returns></returns>
    public static string[] GetPossibleLangs()
    {
        return Translator._translations.Keys.ToArray();
    }

    /// <summary>
    /// Method for getting actual selected language
    /// </summary>
    /// <returns></returns>
    public static string GetSelectedLang()
    {
        return _selectedLang;
    }

    /// <summary>
    /// Method for setting new language
    /// </summary>
    /// <param name="lang"></param>
    public static void SetLang(string lang)
    {
        Translator._selectedLang = lang;
    }
}