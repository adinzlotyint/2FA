using System;
using System.Net.Mail;
public class EmailOtpService {
  private readonly Random _random = new Random();
  private readonly Dictionary<string, string> _otpStorage = new Dictionary<string, string>();

  public string GenerateOtp(string email) {
    string otp = _random.Next(100000, 999999).ToString();
    _otpStorage[email] = otp;

    // Send OTP to email
    SendEmail(email, "Your OTP Code", $"Your OTP code is: {otp}");

    return otp; // For demonstration purposes
  }

  public bool VerifyOtp(string email, string otp) {
    return _otpStorage.ContainsKey(email) && _otpStorage[email] == otp;
  }

  private void SendEmail(string toEmail, string subject, string body) {
    using (var client = new SmtpClient("smtp.example.com")) {
      client.Credentials = new System.Net.NetworkCredential("your-email@example.com", "your-email-password");
      var mailMessage = new MailMessage {
        From = new MailAddress("your-email@example.com"),
        Subject = subject,
        Body = body
      };
      mailMessage.To.Add(toEmail);
      client.Send(mailMessage);
    }
  }
}
