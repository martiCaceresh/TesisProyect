namespace OptiArroz_Tesis_Proyect.Data.Interfaces
{
	public interface INotificationSenderDA
	{
        void SendNotification(List<string> PhoneNumbers, string Message);
    }
}
