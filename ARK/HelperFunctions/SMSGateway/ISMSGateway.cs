namespace ARK.HelperFunctions.SMSGateway
{
    interface ISmsGateway
    {
        bool SendSms(string sender, string reciever, string message);
    }
}
