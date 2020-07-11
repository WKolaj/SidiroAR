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
            ["TranslationWindow.EnglishButtonText"] = "ANGIELSKI"
        },
        ["en"] = new Dictionary<string, string>()
        {
            ["HelpPage.HeaderTitle"] = "Help",
            ["TranslationWindow.ContentText"] = "Please choose application language",
            ["TranslationWindow.PolishButtonText"] = "POLISH",
            ["TranslationWindow.EnglishButtonText"] = "ENGLISH"
        }
    };

    private static string _selectedLang = "pl";

    public static string GetTranslation(string key)
    {
        return Translator._translations[Translator._selectedLang][key];
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
