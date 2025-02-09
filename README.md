# EmailValidatorAPI

EmailValidatorAPI é uma API desenvolvida em ASP.NET Core que valida o formato de e-mails, verifica se o domínio possui registros MX e, se as validações forem positivas, envia o e-mail.

## Funcionalidades

- Validação do formato do e-mail
- Verificação de registros MX do domínio
- Envio de e-mails via SMTP

## Requisitos

- .NET 8.0 SDK
- Um servidor SMTP configurado

## Instalação

1. Clone o repositório:

    ```sh
    git clone https://github.com/acgoularthub/EmailSender.git
    cd EmailSender
    ```

2. Restaure as dependências:

    ```sh
    dotnet restore
    ```

3. Configure as suas credenciais de e-mail no arquivo [appsettings.json](http://_vscodecontentref_/0):

    ```json
    {
      "Logging": {
        "LogLevel": {
          "Default": "Information",
          "Microsoft.AspNetCore": "Warning"
        }
      },
      "AllowedHosts": "*",
      "EmailSettings": {
        "SmtpServer": "smtp.seuservidor.com",
        "Port": 587,
        "Username": "seu-usuario",
        "Password": "sua-senha",
        "FromEmail": "seuemail@dominio.com"
      }
    }
    ```

## Uso

1. Execute a aplicação:

    ```sh
    dotnet run
    ```

2. A API estará disponível em `http://localhost:5224` (ou a porta configurada).

## Endpoints

### Validar e Enviar E-mail

- **URL:** `/api/email/validate-and-send`
- **Método:** `POST`
- **Corpo da Requisição:**

    ```json
    {
      "email": "email@dominio.com"
    }
    ```

- **Respostas:**
  - `200 OK`: E-mail enviado com sucesso.
  - `400 Bad Request`: E-mail inválido ou domínio sem registros MX.
  - `500 Internal Server Error`: Erro ao enviar e-mail.

