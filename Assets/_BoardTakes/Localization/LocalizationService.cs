using System;
using System.Collections.Generic;
using BoardTakes.Core;
using UnityEngine;

namespace BoardTakes.Localization
{
    /// <summary>
    /// Tiny JSON loc. No Unity Localization package, no runtime codegen.
    /// Files: Resources/BoardTakes/Loc/ui_en.json as { "items": [ {"k":"...","v":"..."} ] }
    /// </summary>
    public sealed class LocalizationService : ILocalizationService
    {
        const string ResourceFolder = "BoardTakes/Loc/ui_{0}";

        readonly Dictionary<string, string> _table = new(StringComparer.Ordinal);
        public string CurrentCode { get; private set; } = "en";

        public void SetLocale(string code)
        {
            if (string.IsNullOrWhiteSpace(code)) code = "en";
            code = code.Trim().ToLowerInvariant();
            LoadTable(code);
            CurrentCode = code;
            if (GameKernel.Exists)
                GameKernel.Instance.Events.Publish(new LocaleChanged(code));
        }

        public string Get(string key)
        {
            if (string.IsNullOrEmpty(key)) return string.Empty;
            return _table.TryGetValue(key, out var value) ? value : key;
        }

        void LoadTable(string code)
        {
            _table.Clear();
            var asset = Resources.Load<TextAsset>(string.Format(ResourceFolder, code));
            if (asset == null && code != "en")
                asset = Resources.Load<TextAsset>(string.Format(ResourceFolder, "en"));
            if (asset == null)
            {
                BoardLog.Warn($"No loc table for '{code}'.");
                return;
            }

            LocFile wrapper;
            try
            {
                wrapper = JsonUtility.FromJson<LocFile>(asset.text);
            }
            catch (Exception ex)
            {
                BoardLog.Error("Loc JSON parse failed: " + ex.Message);
                return;
            }

            if (wrapper?.items == null) return;
            for (var i = 0; i < wrapper.items.Length; i++)
            {
                var row = wrapper.items[i];
                if (row == null || string.IsNullOrEmpty(row.k)) continue;
                _table[row.k] = row.v ?? string.Empty;
            }

            BoardLog.Info($"Locale '{code}' loaded ({_table.Count} keys).");
        }

        [Serializable]
        class LocFile
        {
            public LocPair[] items;
        }

        [Serializable]
        class LocPair
        {
            public string k;
            public string v;
        }
    }
}
