using System.Collections;

namespace WebApi.Test.InlineData
{
    // classe para fazer o culture inline data para os testes. vai ser usado no teste para simular os diversos idiomas. vai retornar um IEnumerable de um tipo object array
    public class CultureInlineDataTest : IEnumerable<Object[]>
    {
        public IEnumerator<object[]> GetEnumerator()
        {
            yield return new object[] { "en" };
            yield return new object[] { "pt-BR" };
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator(); 
    }
}
