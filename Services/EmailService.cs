using System.Text.RegularExpressions;           // Para validação do formato do e-mail com Regex
using DnsClient;                                // Para consulta de registros MX do domínio
using MailKit.Net.Smtp;                         // Para envio de e-mails via SMTP
using MailKit.Security;                         // Para definir opções de segurança na conexão SMTP
using MimeKit;                                  // Para criação e formatação das mensagens de e-mail
using Microsoft.Extensions.Options;             // Para injetar as configurações do appsettings.json
using Microsoft.Extensions.Logging;             // Para registrar logs e erros
using EmailValidationAPI.Models;                // Para utilizar a classe EmailSettings

namespace EmailValidationAPI.Services
{
  /// <summary>
  /// Serviço responsável por validar e enviar e-mails.
  /// </summary>
  public class EmailService
  {
    private readonly EmailSettings _emailSettings; // Armazena as configurações de e-mail
    private readonly ILogger<EmailService> _logger;  // Para registrar logs

    // Construtor com injeção das configurações e do logger
    public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
    {
      _emailSettings = emailSettings.Value;
      _logger = logger;
    }

    /// <summary>
    /// Valida o formato do e-mail utilizando expressão regular.
    /// </summary>
    public bool IsValidEmail(string email)
    {
      // Verifica se o e-mail corresponde ao padrão básico (ex: usuario@dominio.com)
      return Regex.IsMatch(email, @"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);
    }

    /// <summary>
    /// Verifica se o domínio possui registros MX configurados.
    /// </summary>
    public bool HasMXRecord(string domain)
    {
      try
      {
        var lookup = new LookupClient();
        // Consulta registros do tipo MX para o domínio
        var result = lookup.Query(domain, QueryType.MX);
        // Retorna true se houver ao menos um registro MX
        return result.Answers.Count > 0;
      }
      catch (Exception ex)
      {
        // Registra o erro e retorna false caso ocorra alguma exceção
        _logger.LogError($"Erro ao consultar registros MX para o domínio {domain}: {ex.Message}");
        return false;
      }
    }

    /// <summary>
    /// Envia o e-mail para o destinatário informado, após as validações.
    /// </summary>
    public async Task<bool> SendEmailAsync(string toEmail)
    {
      try
      {
        // Cria a mensagem de e-mail utilizando MimeKit
        var message = new MimeMessage();
        // Define o remetente
        message.From.Add(new MailboxAddress("Sistema", _emailSettings.FromEmail));
        // Define o destinatário
        message.To.Add(new MailboxAddress("", toEmail));
        // Define o assunto do e-mail
        message.Subject = "Teste de E-mail";
        // Define o corpo do e-mail como texto simples
        message.Body = new TextPart("plain")
        {
          Text = "Este é um teste de envio de e-mail via API .NET"
        };

        // Utiliza o MailKit para enviar o e-mail via SMTP
        using var smtpClient = new SmtpClient();
        // await smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.StartTls);
        await smtpClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.Port, SecureSocketOptions.None);
        await smtpClient.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
        await smtpClient.SendAsync(message);
        await smtpClient.DisconnectAsync(true);

        _logger.LogInformation($"E-mail enviado com sucesso para {toEmail}");
        return true;
      }
      catch (Exception ex)
      {
        _logger.LogError($"Erro ao enviar e-mail para {toEmail}: {ex.Message}");
        return false;
      }
    }
  }
}
