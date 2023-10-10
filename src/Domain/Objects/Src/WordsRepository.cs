using Service.Models;
using System.Collections.Generic;
using System.Linq;

namespace Objects.Src
{
    public class WordsRepository
    {
        public Dictionary<string, string> GetWords(WordsType type)
        {
            switch (type)
            {
                case WordsType.Verbs:
                    return this.Werbos;
                case WordsType.Adjectives:
                    return this.Adjectivos;
                case WordsType.Adverbs:
                    return this.Adverbious;
                case WordsType.Questions:
                    return this.Preguntas;
                case WordsType.Prepositions:
                    return this.Preposicoes;
                case WordsType.Nouns:
                    return this.Nouns;
                case WordsType.All:
                    return this.Werbos.
                        Concat(this.Adjectivos).
                        Concat(this.Preguntas).
                        Concat(this.Adverbious).
                        Concat(this.Nouns)
                        .Concat(this.Preposicoes).ToDictionary(kv => kv.Key, kv => kv.Value);

            }
            return new Dictionary<string, string>();
        }

        private Dictionary<string, string> Werbos = new Dictionary<string, string>()
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

        private Dictionary<string, string> Adjectivos = new Dictionary<string, string>()
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

        private Dictionary<string, string> Preguntas = new Dictionary<string, string>()
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

        private Dictionary<string, string> Preposicoes = new Dictionary<string, string>()
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

        private Dictionary<string, string> Adverbious = new Dictionary<string, string>()
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

        private Dictionary<string, string> Nouns = new Dictionary<string, string>()
        {
            {"Mae","Мама"},
            {"Pae","Мама"},
            {"o pianisto", "Пианист" },
            {"a pianista", "Пианистка" },
            {"Obrigado", "Спасибо" },
            {"Amor", "Любовь" },
            {"Felicidade", "Радость" },
            {"Gato", "Кошка" },
            {"Cao", "Собака" },
            {"Adeus", "Адьос" },
            {"Pais", "Страна" },
            {"Livro", "Книга" },
            {"o Carro", "Автомобиль" },
            {"a Mesa", "Стол" },
            {"Disculpe", "Извините" },
            {"Entrada", "Вход" },
            {"Saida", "Выход" },
            {"Aberto", "Открыто" },
            {"a sala de estar", "гостинная" },
            {"na garagem", "Гараж" },
            {"O fogão", "Плита" },
            {"o jornal", "Журнал" },
            {"o chão", "Пол" },
            {"a cozinha", "Кухня" },
            {"Hall", "Гостинная" }
        };
    }
}
