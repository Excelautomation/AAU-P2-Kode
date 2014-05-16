namespace ARK.HelperFunctions.SMSGateway
{
    public interface ISmsGateway
    {
        #region Public Methods and Operators

        bool SendSms(string sender, string reciever, string message);

        #endregion
    }
}