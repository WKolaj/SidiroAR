using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Translator
{
    private static Dictionary<string, Dictionary<string, string>> _translations = new Dictionary<string, Dictionary<string, string>>()
    {
        ["pl"] = new Dictionary<string, string>()
        {
            ["HelpPage.HeaderTitle"] = "Pomoc",
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
            ["LoginPage.LoginInputPlaceholder"] = "Email",
            ["LoginPage.PasswordInputPlaceholder"] = "Hasło",
            ["LoginPage.LoginButtonText"] = "Zaloguj",
            ["LoginPage.MainLabelText"] = "Logowanie"
        },
        ["en"] = new Dictionary<string, string>()
        {
            ["HelpPage.HeaderTitle"] = "Help",
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
            ["LoginPage.LoginInputPlaceholder"] = "Email",
            ["LoginPage.PasswordInputPlaceholder"] = "Password",
            ["LoginPage.LoginButtonText"] = "Log in",
            ["LoginPage.MainLabelText"] = "Logging in"

        }
    };

    private static string _selectedLang = "pl";

    public static string GetTranslation(string key)
    {
        if (!Translator._translations.ContainsKey(Translator._selectedLang)) return key;

        var translationsSet = Translator._translations[Translator._selectedLang];

        if (!translationsSet.ContainsKey(key)) return key;

        return translationsSet[key];
    }

    public static string[] GetPossibleLangs()
    {
        return Translator._translations.Keys.ToArray();
    }

    public static string GetSelectedLang()
    {
        return _selectedLang;
    }

    public static void SetLang(string lang)
    {
        Translator._selectedLang = lang;
    }
}
