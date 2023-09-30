using System.Collections.Generic;

namespace WordsApp
{
    internal class WordsProvider
    {
        public static Dictionary<string, string> GetWerbos() => new()
        {
                {"Comer", "Кушать/Есть" },
                {"Dormir", "Спать" },
                {"Amar", "Любить" },
                {"Escrever", "Писать" },
                {"Ler", "Читать"},
                {"Beber", "Пить"},
                {"Ficar", "Находится"},
                {"Precisar", "Нуждаться" },
                {"Entender", "Понимать" },
                {"Abrir", "Открывать" },
                {"Comprar", "Покупать" },
                {"Ver", "Видеть" },
                {"Ir", "Ходить" },
                {"Fazer", "Делать" },
                {"Saber", "Знать" },
                {"Dar", "Давать" },
                {"Ouvir", "Слушать" },
                {"Pedir", "Просить" },
                {"Poder", "Мочь" },
                {"Preferir", "Предпочитать" },
                {"Compor", "Сочинять" },
                {"Perguntar", "Спрашивать" },
                {"Sentir", "Чувствовать" },
                {"Sonhar", "Снится" },
                {"Referir", "Относится" },
                {"Procurar", "Искать" },
                {"Servir", "Служить" },
                {"Brincar", "Играть" },
        };

        public static Dictionary<string, string> GetAdjectivos() => new()
        {
                {"Alto", "Высокий" },
                {"Bom", "Хороший" },
                {"Belo", "Прекрасный" },
                {"Mau", "Плохой" },
                {"Facil", "Простой" },
                {"Dificil", "Сложный" },
                {"Largo", "Широкий" },
                {"Fino", "Тонкий" },
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
    }
}
