using Microsoft.AspNetCore.Mvc;
using EmailValidationAPI.Services;

namespace EmailValidationAPI.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class EmailController : ControllerBase
  {
    private readonly EmailService _emailService;

    public EmailController(EmailService emailService)
    {
      _emailService = emailService;
    }

    /// <summary>
    /// Endpoint que valida o formato do e-mail, verifica se o domínio possui registros MX
    /// e, se as validações forem positivas, envia o e-mail.
    /// </summary>
    /// <param name="request">Requisição contendo o e-mail a ser validado e utilizado para envio</param>
    /// <returns>Status da operação (sucesso ou erro)</returns>
    [HttpPost("validate-and-send")]
    public async Task<IActionResult> ValidateAndSendEmail([FromBody] EmailRequest request)
    {
      if (!_emailService.IsValidEmail(request.Email))
        return BadRequest("E-mail inválido!");

      var domain = request.Email.Split('@')[1];

      // Verifica se o domínio possui registros MX
      if (!_emailService.HasMXRecord(domain))
        return BadRequest("Domínio sem registros MX!");

      // Tenta enviar o e-mail
      var success = await _emailService.SendEmailAsync(request.Email);
      if (!success)
        return StatusCode(500, "Erro ao enviar e-mail.");

      return Ok("E-mail enviado com sucesso!");
    }
  }

  /// <summary>
  /// Classe que representa a requisição para o endpoint de e-mail.
  /// </summary>
  public class EmailRequest
  {
    // Propriedade que contém o e-mail a ser validado e utilizado para envio
    public string Email { get; set; } = string.Empty;
  }
}
