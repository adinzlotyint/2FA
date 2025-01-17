namespace Auth2FA.Model.Requests {
  public class Update2FARequest {
    public string Method { get; set; }
    public string SecretKey { get; set; }
  }
}
