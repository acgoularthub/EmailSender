namespace EmailValidationAPI.Models
{
  /// <summary>
  /// Representa as configurações de e-mail utilizadas para o envio via SMTP.
  /// </summary>
  public class EmailSettings
  {
    public string SmtpServer { get; set; } = string.Empty; // Endereço do servidor SMTP
    public int Port { get; set; }                          // Porta do servidor SMTP (587)
    public string Username { get; set; } = string.Empty;     // Nome de usuário para autenticação
    public string Password { get; set; } = string.Empty;     // Senha para autenticação
    public string FromEmail { get; set; } = string.Empty;    // E-mail remetente
  }
}
