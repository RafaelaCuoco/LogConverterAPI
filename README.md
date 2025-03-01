<!DOCTYPE html>
<html lang="pt-br">
<head>
    <meta charset="UTF-8">
    <meta name="viewport" content="width=device-width, initial-scale=1.0">
</head>
<body>
    <h1>LogConverterAPI</h1>
    <p>Uma API para transformar logs no formato "MINHA CDN" para o formato "Agora".</p>
    <div class="section">
        <h2>Índice</h2>
        <ul>
            <li><a href="#descricao">Descrição</a></li>
            <li><a href="#instalacao">Instalação</a></li>
            <li><a href="#banco">Configuração do Banco de Dados</a></li>
            <li><a href="#uso">Uso</a></li>
            <li><a href="#testes">Testes</a></li>
            <li><a href="#licenca">Licença</a></li>
        </ul>
    </div>
    <div class="section" id="descricao">
        <h2>Descrição</h2>
        <p>A <strong>LogConverterAPI</strong> é uma aplicação desenvolvida em .NET Core 2.1 que converte logs no formato "MINHA CDN" para o formato "Agora". A API também salva os logs transformados em arquivos organizados por ano e mês.</p>
        <p>Principais funcionalidades:</p>
        <ul>
            <li>Transformação de logs no formato "MINHA CDN" para "Agora".</li>
            <li>Armazenamento dos logs transformados em arquivos organizados por data.</li>
            <li>Integração com banco de dados para persistência de logs originais e transformados.</li>
        </ul>
    </div>
    <div class="section" id="instalacao">
        <h2>Instalação</h2>
        <p>Para configurar o projeto localmente, siga as etapas abaixo:</p>
        <ol>
            <li>Clone o repositório:</li>
            <pre><code>git clone https://github.com/seu-usuario/LogConverterAPI.git</code></pre>
            <li>Navegue até a pasta do projeto:</li>
            <pre><code>cd LogConverterAPI</code></pre>
            <li>Restaure as dependências:</li>
            <pre><code>dotnet restore</code></pre>
            <li>Compile o projeto:</li>
            <pre><code>dotnet build</code></pre>
            <li>Execute a aplicação:</li>
            <pre><code>dotnet run --project LogConverterAPI</code></pre>
        </ol>
    </div>
    <div class="section" id="banco">
    <h2>Configuração do Banco de Dados</h2>
    <h3>Passo 1: Configurar a String de Conexão</h3>
    <p>Abra o arquivo <code>appsettings.json</code> no projeto <code>LogConverterAPI</code> e configure a string de conexão para o SQL Server:</p>
    <pre><code class="json">{
  "ConnectionStrings": {
    "DefaultConnection": "Server=SEU_SERVIDOR;Database=LogConverterDB;User Id=SEU_USUARIO;Password=SUA_SENHA;TrustServerCertificate=True;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}</code></pre>
    <blockquote>
        <p>Substitua <code>SEU_SERVIDOR</code>, <code>SEU_USUARIO</code> e <code>SUA_SENHA</code> pelas suas credenciais do SQL Server.</p>
    </blockquote>
    <h3>Passo 2: Criar o Banco de Dados no SQL Server</h3>
    <p>Em seu <strong>SQL Server Management Studio (SSMS)</strong> ou outra ferramenta de gerenciamento de banco de dados, crie o banco de dados vazio para que o Entity Framework possa popular as tabelas automaticamente:</p>
    <pre><code class="sql">CREATE DATABASE LogConverterDB;</code></pre>
    <h3>Passo 3: Aplicar Migrações com o Entity Framework Core</h3>
    <p>Para criar as tabelas no banco de dados, execute as migrações do Entity Framework Core. Siga os passos abaixo:</p>
    <ol>
        <li>Abra um terminal na raiz do projeto <code>LogConverterAPI</code>.</li>
        <li>Aplique as migrações ao banco de dados:</li>
        <pre><code class="bash">dotnet ef database update</code></pre>
    </ol>
    <h3>Passo 4: Verificar a Conexão</h3>
    <p>Para garantir que a aplicação está conectada corretamente ao banco de dados:</p>
    <ol>
        <li>Execute a aplicação usando o comando:</li>
        <pre><code class="bash">dotnet run --project LogConverterAPI</code></pre>
        <li>Verifique se o banco de dados foi criado e populado com as tabelas necessárias.</li>
    </ol>
    </div>
    <div class="section" id="uso">
        <h2>Uso</h2>
        <p>A API expõe um endpoint para transformar logs. Envie uma requisição POST para o endpoint <code>/api/logs/transformar</code> com o seguinte corpo JSON:</p>
        <pre><code>{
    "conteudo": "312|200|HIT|\"GET /robots.txt HTTP/1.1\"|100.2",
    "salvarNoServidor": true
}</code></pre>
        <p>O campo <code>conteudo</code> deve conter os logs no formato "MINHA CDN", e o campo <code>salvarNoServidor</code> indica se os logs devem ser salvos em arquivos no servidor.</p>
    </div>
    <div class="section" id="testes">
        <h2>Testes</h2>
        <p>Para executar os testes unitários:</p>
        <ol>
            <li>Navegue até a pasta do projeto de testes:</li>
            <pre><code>cd LogConverterAPI.Tests</code></pre>
            <li>Execute os testes:</li>
            <pre><code>dotnet test</code></pre>
        </ol>
        <p>Os testes cobrem os principais cenários, como validação de entradas, transformação de logs e comportamento do controlador.</p>
    </div>
    <div class="section" id="licenca">
        <h2>Licença</h2>
        <p>Este projeto está licenciado sob a <a href="https://opensource.org/licenses/MIT">MIT License</a>. Veja o arquivo <a href="LICENSE">LICENSE</a> para mais detalhes.</p>
    </div>
    <div align="center">
  📚 <a href="https://github.com/RafaelaCuoco/LogConverterAPI/wiki">Documentação</a> | 
  🔗 <a href="https://github.com/RafaelaCuoco/LogConverterAPI">Repositório</a> | 
  📧 <a href="mailto:rafaela.cuoco@gmail.com">Contato</a> 
</div>
</body>
</html>
