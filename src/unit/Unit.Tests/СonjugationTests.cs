using NUnit.Framework;

namespace Unit.Tests
{
    public class СonjugationTests
    {
        [TestCase("contar", new string[] {"cuento", "cuentas", "cuenta", "contamos", "contáis", "cuentan" })]
        [TestCase("volar", new string[] { "vuelo", "vuelas", "vuela", "volamos", "voláis", "vuelan" })]
        [TestCase("desayunar", new string[] { "desayuno", "desayunas", "desayuna", "desayunamos", "desayunáis", "desayunan" })]
        public void Spanish_Сonjugation(string verb, string[] expected)
        {
            var result = ConjugateVerb(verb, 1);
            Assert.Multiple(() =>
            {
                Assert.That(result[0], Is.EqualTo(expected[0]), "YO");
                Assert.That(result[1], Is.EqualTo(expected[1]), "Tu");
                Assert.That(result[2], Is.EqualTo(expected[2]), "Ella/Elle");
                Assert.That(result[3], Is.EqualTo(expected[3]), "Nosotros");
                Assert.That(result[4], Is.EqualTo(expected[4]), "Vosotros");
                Assert.That(result[5], Is.EqualTo(expected[5]), "Usted");
            });
        }

        static string[] ConjugateVerb(string verb, int form)
        {
            var output = new string[6];
            output[0] = ConjugateVerb_One(verb, "ue", "o");
            output[1] = ConjugateVerb_One(verb, "ue", "as");
            output[2] = ConjugateVerb_One(verb, "ue", "a");
            output[3] = ConjugateVerb_One(verb, null, "amos");
            output[4] = ConjugateVerb_One(verb, null, "áis");
            output[5] = ConjugateVerb_One(verb, "ue", "an");

            return output;
        }

        static string ConjugateVerb_One(string word, string rootChange, string ending)
        {
            var output = "";
            bool isSwappedRoot = false;
            for (var i = 0; i < word.Length - 1; i++)
            {
                // check tail
                if (word[i] == 'a' && word[i + 1] == 'r' && i + 1 == word.Length - 1)
                {
                    return output + ending;
                }
                else
                {
                    if (word[i] == 'o' && !isSwappedRoot && rootChange != null)
                    {
                        isSwappedRoot = true;
                        output += rootChange;
                        continue;
                    }
                }
                output += word[i];


            }
            return "";
        }
    }
}