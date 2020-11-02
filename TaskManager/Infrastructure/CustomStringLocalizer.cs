using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace TaskManager.Infrastructure
{
    public class CustomStringLocalizer : IStringLocalizer
    {
        Dictionary<string, Dictionary<string, string>> resources;
        // ключи ресурсов
        const string TASKCOMP = "Completed";
        const string TASKMOD = "Modified";

        public CustomStringLocalizer()
        {
            // словарь для английского языка
            Dictionary<string, string> enDict = new Dictionary<string, string>
        {
            {TASKCOMP, "Welcome" },
            {TASKMOD, "Hello World!" }
        };
            // словарь для русского языка
            Dictionary<string, string> ruDict = new Dictionary<string, string>
        {
            {TASKCOMP, "Добо пожаловать" },
            {TASKMOD, "Привет мир!" }
        };
            // словарь для немецкого языка
            Dictionary<string, string> deDict = new Dictionary<string, string>
        {
            {TASKCOMP, "Willkommen" },
            {TASKMOD, "Hallo Welt!" }
        };
            // создаем словарь ресурсов
            resources = new Dictionary<string, Dictionary<string, string>>
        {
            {"en", enDict },
            {"ru", ruDict },
            {"de", deDict }
        };
        }
        // по ключу выбираем для текущей культуры нужный ресурс
        public LocalizedString this[string name]
        {
            get
            {
                var currentCulture = CultureInfo.CurrentUICulture;
                string val = "";
                if (resources.ContainsKey(currentCulture.Name))
                {
                    if (resources[currentCulture.Name].ContainsKey(name))
                    {
                        val = resources[currentCulture.Name][name];
                    }
                }
                return new LocalizedString(name, val);
            }
        }

        public LocalizedString this[string name, params object[] arguments] => throw new NotImplementedException();

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }

        public IStringLocalizer WithCulture(CultureInfo culture)
        {
            return this;
        }
    }
}
