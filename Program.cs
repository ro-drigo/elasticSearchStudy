using System;
using elasticSearchSample;
using elasticSearchSample.Models;
using Nest;

try
{
    var usersIndex = new ElasticSearch("users").ElasticSearchConnection();
    //var productsIndex = new ElasticSearch("products").ElasticSearchConnection(); //nao utilizavel

    /*// 1. Criar o índice
    await CreateIndex(client);

    // 2. Adicionar múltiplos documentos
    var users = GetSampleUsers();
    await AddUsers(client, users);*/

    // 3. Realizar uma busca
    await SearchUsers(usersIndex);
}
catch(Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}

static async Task CreateIndex(ElasticClient client)
{
    var createIndexResponse = await client.Indices.CreateAsync("users", c => c
        .Map<User>(m => m
            .AutoMap()
        )
    );

    Console.WriteLine($"Índice criado: {createIndexResponse.IsValid}");
}

static List<User> GetSampleUsers()
{
    return new List<User>
        {
            /*new User { Id = 1, Name = "Alice", Age = 30, Email = "alice@example.com", Location = "New York" },
            new User { Id = 2, Name = "Bob", Age = 25, Email = "bob@example.com", Location = "Los Angeles" },
            new User { Id = 3, Name = "Charlie", Age = 35, Email = "charlie@example.com", Location = "Chicago" },
            new User { Id = 4, Name = "Diana", Age = 28, Email = "diana@example.com", Location = "Miami" },*/
            new User { Id = 5, Name = "John", Age = 31, Email = "john@example.com", Location = "London" }
        };
}

static async Task AddUsers(ElasticClient client, List<User> users)
{
    var bulkResponse = await client.BulkAsync(b => b
        .Index("users")
        .IndexMany(users)
    );

    Console.WriteLine($"Documentos inseridos: {bulkResponse.IsValid}");
}

static async Task SearchUsers(ElasticClient client)
{
    //pega o nome exato
    /*var searchResponse = await client.SearchAsync<User>(s => s
        .Query(q => q
            .Match(m => m
                .Field(f => f.Name)
                .Query("Cadeira")
            )
        )
    );*/

    //fuzziness... pega valores com caracteres errados
    var searchResponse = client.Search<User>(s => s
        .Query(q => q
            .Match(m => m
                .Field(f => f.Name)  // Campo que você está buscando
                .Query("Ailec")       // Termo buscado
                //.Fuzziness(Fuzziness.Auto) // Busca aproximada com erros (o tanto de erros varia do tamanho do termo buscado)
                .Fuzziness(Fuzziness.EditDistance(2)) //permite 2 erros
            )
        )
    );

    //QueryString busca informações com * como se fosse um like
    /*var searchResponse = client.Search<User>(s => s
            .Query(q => q
            .QueryString(qs => qs
                .Fields(f => f.Field("name")) // Campo no índice
                .Query("ali*")                // Padrão de busca
            )
        )
    );*/

    //QueryString busca informações com * como se fosse um like
    /*var searchResponse = client.Search<User>(s => s
            .Query(q => q
            .Wildcard(w => w
            .Field(f => f.Name)
            .Value("ali") // Busca por nomes que começam com "ali"
        )
    )
    );*/

    foreach (var user in searchResponse.Documents)
    {
        Console.WriteLine($"Usuário encontrado: {user.Name}, {user.Age}, {user.Email}, {user.Location}");
    }
}
