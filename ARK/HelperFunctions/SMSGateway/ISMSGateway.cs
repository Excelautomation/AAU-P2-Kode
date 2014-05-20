namespace ARK.HelperFunctions.SMSGateway
{
    public interface ISmsGateway
    {
        bool SendSms(string sender, string reciever, string message);
    }
}