using System;
using System.Collections.Generic;
using System.Text;
using translator.Models;

namespace translator.Services
{
    public static class Texts
    {
        private static readonly Dictionary<string, string[]> Table = new()
        {
            ["toc"] = new[] { "სარჩევი", "Contents", "Содержание" },
            ["translator"] = new[] { "თარჯიმანი", "Translator", "Переводчик" },
            ["lessons"] = new[] { "გაკვეთილები (წიგნები)", "Lessons (books)", "Уроки (книги)" },
            ["test"] = new[] { "შემოწმება (დონეები)", "Test (levels)", "Проверка (уровни)" },
            ["profile"] = new[] { "პროფილი", "Profile", "Профиль" },
            ["exit"] = new[] { "გასვლა", "Log out", "Выход" },
            ["correct"] = new[] { "სწორია!", "Correct!", "Правильно!" },
            ["wrong"] = new[] { "არასწორია.", "Wrong.", "Неправильно." },
            ["congrats"] = new[]
       {
            "გილოცავთ! თქვენ გაიარეთ ყველა 8 დონე!",
            "Congratulations! You completed all 8 levels!",
            "Поздравляем! Вы прошли все 8 уровней!"
        },
            ["nick0"] = new[] { "დამწყები", "Beginner", "Новичок" },
            ["nick1"] = new[] { "მოსწავლე", "Student", "Ученик" },
            ["nick2"] = new[] { "მკვლევარი", "Explorer", "Исследователь" },
            ["nick3"] = new[] { "მოგზაური", "Traveler", "Путешественник" },
            ["nick4"] = new[] { "მთარგმნელი", "Translator", "Переводчик" },
            ["nick5"] = new[] { "ლინგვისტი", "Linguist", "Лингвист" },
            ["nick6"] = new[] { "ოსტატი", "Master", "Мастер" },
            ["nick7"] = new[] { "ექსპერტი", "Expert", "Эксперт" },
            ["nick8"] = new[] { "ლეგენდა", "Legend", "Легенда" },
            ["level1"] = new[] { "აირჩიე თარგმანი", "Pick the translation", "Выбери перевод" },
            ["level2"] = new[] { "უკუთარგმანი", "Reverse translation", "Обратный перевод" },
            ["level3"] = new[] { "ჩასვი სიტყვა წინადადებაში", "Fill the gap", "Вставь слово" },
            ["level4"] = new[] { "დაწერე თარგმანი", "Type the translation", "Напиши перевод" },
            ["level5"] = new[] { "ააწყვე წინადადება", "Build the sentence", "Составь предложение" },
            ["level6"] = new[] { "თარგმნე წინადადება", "Translate the sentence", "Переведи предложение" },
            ["level7"] = new[] { "ჩამოხრჩობილა", "Hangman", "Виселица" },
            ["level8"] = new[] { "ბოსის დონე: სწრაფი მიქსი", "Boss level: speed mix", "Босс: скоростной микс" }
        };

        public static string Get(string key, Lang lang)
        {
            return Table.TryGetValue(key, out string[]? values) ? values[(int)lang] : key;
        }

        public static string Both(string key, User user)
        {
            return Get(key, user.Native) + " / " + Get(key, user.Learning);
        }
    }
}
