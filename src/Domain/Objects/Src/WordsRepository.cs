using Service.Models;
using System.Collections.Generic;

namespace Objects.Src
{
    public class WordsRepository
    {
        public static Dictionary<string, string> GetWords(WordsType type)
        {
            switch (type)
            {
                case WordsType.Verbs:
                    return GetWerbos();
                case WordsType.Adjectives:
                    return GetAdjectivos();
                case WordsType.Adverbs:
                    return GetAdverbious();
                case WordsType.Questions:
                    return GetPreguntas();
                case WordsType.Prepositions:
                    return GetPreposicoes();

            }
            return new Dictionary<string, string>();
        }



        public static Dictionary<string, string> GetWerbos() => new()
        {
                {"comer", "кушать/eсть" },
                {"dormir", "спать" },
                {"amar", "любить" },
                {"escrever", "писать" },
                {"ler", "читать"},
                {"beber", "пить"},
                {"ficar", "находится"},
                {"precisar", "нуждаться" },
                {"entender", "понимать" },
                {"abrir", "открывать" },
                {"comprar", "покупать" },
                {"ver", "видеть" },
                {"ir", "ходить" },
                {"fazer", "делать" },
                {"saber", "знать" },
                {"dar", "давать" },
                {"ouvir", "слушать" },
                {"pedir", "просить" },
                {"poder", "мочь" },
                {"preferir", "предпочитать" },
                {"compor", "сочинять" },
                {"perguntar", "спрашивать" },
                {"sentir", "чувствовать" },
                {"sonhar", "снится" },
                {"referir", "относится" },
                {"procurar", "искать" },
                {"servir", "служить" },
                {"brincar", "играть" },
        };

        public static Dictionary<string, string> GetAdjectivos() => new()
        {
                {"Alto", "Высокий" },
                {"Bom", "Хороший" },
                {"Belo", "Прекрасный" },
                {"Mau", "Плохой" },
                {"Facil", "Простой/Легко" },
                {"Dificil", "Сложный" },
                {"Largo", "Широкий" },
                {"Fino", "Тонкий" },
                {"Feliz", "Счастливый" },
                {"Bravo", "Злой" },
                {"Calmo", "Спокойный" },
        };

        public static Dictionary<string, string> GetPreguntas() => new()
        {
                {"Como?", "Как?" },
                {"O que?", "Что?" },
                {"Onde?", "Где?" },
                {"Quem?", "Кто?" },
                {"Por quanto tempo?", "Как долго?"},
                {"Quanto?", "Сколько?"},
                {"Qual?", "Какой?"},
                {"Quando?", "Когда?" },
                {"Por que?", "Почему?" },
        };

        public static Dictionary<string, string> GetPreposicoes() => new()
        {
                {"Acima", "Над" },
                {"Depois", "После" },
                {"Ao longo", "Вдоль" },
                {"Em", "В" },
                {"Debaixo", "Под"},
                {"Ao lado de", "Рядом"},
                {"Pelo", "У"},
                {"Exceto", "Кроме" },
                {"Durante", "Во время" },
                {"Entre", "Между" },
                {"Em torno", "Вокруг" },
                {"Contra", "Против" },
                {"Do outro lado", "Напротив" },
                {"Atras", "За" },
                {"Para", "Для" },
                {"De", "От" },
                {"Fora", "Снаружи/Из" },
                {"Dentro", "Внутри" },
                {"Perto", "Около" },
                {"Sem", "Без" },
                {"Com", "с" }
        };

        public static Dictionary<string, string> GetAdverbious() => new()
        {
           {"Ontem", "Вчера" },
           {"Hoje", "Сегодня" },
           {"Amanha", "Завтра" },
           {"Agora", "В настоящее время" },
           {"Entao", "Затем" },
           {"Mais tarde", "Позже" },
           {"Agora mesmo", "Прямо сейчас" },
           {"Na proxima semana", "На следующей неделе" },
           {"Ja", "Уже" },
           {"Recentemente", "Недавно/За последнее время" },
           {"Logo", "Скоро" },
           {"Imediatamente", "Немедленно" },
           {"Ainda", "По-прежнему" },
           {"Aqui", "Здесь" },
           {"La", "Там" },
           {"Ali", "Вон там" },
           {"Em toda parte", "Везде" },
            // advérbios de modo
           {"Muito", "Очень/вполне/довольно" },
           {"Realmente", "На самом деле" },
           {"Rapido", "Очень быстро" },
           {"Rigido", "Жесткий" },
           {"Juntos", "Вместе" },
           {"Sozinho", "В одиночку" },
           {"Quase", "Почти" },
           // advérbios de freqüência
           {"Sempre", "Всегда" },
           {"Frequentemente", "Часто" },
           {"Geralmente", "Обычно" },
           {"As vezes/Ocasionalmente", "Иногда" },
           {"Raramente", "Редко" },
           {"Nunca", "Никогда" },
        };
    }
}
