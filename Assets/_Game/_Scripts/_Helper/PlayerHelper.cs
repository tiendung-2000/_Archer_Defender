using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public static class PlayerHelper
{
    public static Sprite playerFlag;
    public static IEnumerator DoLoadPlayerFlag(string flagsFolderPath)
    {
        string countryCode = Application.systemLanguage.ToCountryCode();
        string savePath = Path.Combine(Application.persistentDataPath, flagsFolderPath);
        string filePath = Path.Combine(savePath, countryCode + ".png");
        if (File.Exists(filePath))
        {
            byte[] textureBytes = File.ReadAllBytes(filePath);
            Texture2D texture = new Texture2D(2, 2); // Create a temporary Texture2D
            texture.LoadImage(textureBytes); // Load the texture bytes into the temporary Texture2D

            playerFlag = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
            yield return new WaitForEndOfFrame();
        }
        else
        {
            yield return DownloadAndSaveFlag(countryCode, flagsFolderPath);
        }
    }
    private static IEnumerator DownloadAndSaveFlag(string countryCode, string flagsFolderPath)
    {
        string flagUrl = "https://flagsapi.com/" + countryCode.ToUpper() + "/flat/64.png";

        using (UnityWebRequest request = UnityWebRequestTexture.GetTexture(flagUrl))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.Success)
            {
                Texture2D texture = ((DownloadHandlerTexture)request.downloadHandler).texture;
                playerFlag = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), Vector2.zero);
                byte[] textureBytes = texture.EncodeToPNG();

                string savePath = Path.Combine(Application.persistentDataPath, flagsFolderPath);
                if (!Directory.Exists(savePath))
                {
                    Directory.CreateDirectory(savePath);
                }

                string filePath = Path.Combine(savePath, countryCode + ".png");
                File.WriteAllBytes(filePath, textureBytes);
            }
            else
            {
                playerFlag = null;
                Debug.LogError("Failed to download flag: " + request.error);
            }
        }
    }

    private static readonly Dictionary<SystemLanguage, string> COUTRY_CODES = new Dictionary<SystemLanguage, string>()
        {
            { SystemLanguage.Afrikaans, "ZA" },
            { SystemLanguage.Arabic    , "SA" },
            { SystemLanguage.Basque    , "US" },
            { SystemLanguage.Belarusian    , "BY" },
            { SystemLanguage.Bulgarian    , "BJ" },
            { SystemLanguage.Catalan    , "ES" },
            { SystemLanguage.Chinese    , "CN" },
            { SystemLanguage.Czech    , "HK" },
            { SystemLanguage.Danish    , "DK" },
            { SystemLanguage.Dutch    , "BE" },
            { SystemLanguage.English    , "US" },
            { SystemLanguage.Estonian    , "EE" },
            { SystemLanguage.Faroese    , "FU" },
            { SystemLanguage.Finnish    , "FI" },
            { SystemLanguage.French    , "FR" },
            { SystemLanguage.German    , "DE" },
            { SystemLanguage.Greek    , "JR" },
            { SystemLanguage.Hebrew    , "IL" },
            { SystemLanguage.Icelandic    , "IS" },
            { SystemLanguage.Indonesian    , "ID" },
            { SystemLanguage.Italian    , "IT" },
            { SystemLanguage.Japanese    , "JP" },
            { SystemLanguage.Korean    , "KR" },
            { SystemLanguage.Latvian    , "LV" },
            { SystemLanguage.Lithuanian    , "LT" },
            { SystemLanguage.Norwegian    , "NO" },
            { SystemLanguage.Polish    , "PL" },
            { SystemLanguage.Portuguese    , "PT" },
            { SystemLanguage.Romanian    , "RO" },
            { SystemLanguage.Russian    , "RU" },
            { SystemLanguage.SerboCroatian    , "SP" },
            { SystemLanguage.Slovak    , "SK" },
            { SystemLanguage.Slovenian    , "SI" },
            { SystemLanguage.Spanish    , "ES" },
            { SystemLanguage.Swedish    , "SE" },
            { SystemLanguage.Thai    , "TH" },
            { SystemLanguage.Turkish    , "TR" },
            { SystemLanguage.Ukrainian    , "UA" },
            { SystemLanguage.Vietnamese    , "VN" },
            { SystemLanguage.ChineseSimplified    , "CN" },
            { SystemLanguage.ChineseTraditional    , "CN" },
            { SystemLanguage.Unknown    , "US" },
            { SystemLanguage.Hungarian    , "HU" },
        };

    /// <summary>
    /// Returns approximate country code of the language.
    /// </summary>
    /// <returns>Approximated country code.</returns>
    /// <param name="language">Language which should be converted to country code.</param>
    public static string ToCountryCode(this SystemLanguage language)
    {
        string result;
        if (COUTRY_CODES.TryGetValue(language, out result))
        {
            return result;
        }
        else
        {
            return COUTRY_CODES[SystemLanguage.Unknown];
        }
    }
}
public static class Utils
{
    [Serializable]
    private class NamesList
    {
        public List<string> names;
    }

    static NamesList namesList;
    static NamesList CurrentNamesList
    {
        get
        {
            if (namesList == null)
            {
                TextAsset textAsset = Resources.Load("TextsFake/NamesList") as TextAsset;
                namesList = JsonUtility.FromJson<NamesList>(textAsset.text);
            }
            return namesList;
        }
    }

    public static string GetRandomName()
    {
        return CurrentNamesList.names[UnityEngine.Random.Range(0, CurrentNamesList.names.Count)];
    }

    public static List<string> GetRandomNames(int nbNames)
    {
        if (nbNames > CurrentNamesList.names.Count)
            throw new Exception("Asking for more random names than there actually are!");

        NamesList copy = new NamesList();
        copy.names = new List<string>(CurrentNamesList.names);

        List<string> result = new List<string>();

        for (int i = 0; i < nbNames; i++)
        {
            int rnd = UnityEngine.Random.Range(0, copy.names.Count);
            result.Add(copy.names[rnd]);
            copy.names.RemoveAt(rnd);
        }

        return result;
    }
}
