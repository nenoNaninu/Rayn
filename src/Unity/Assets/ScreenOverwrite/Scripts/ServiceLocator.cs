namespace ScreenOverwriter
{
    // かなりひどい実装。
    public static class ServiceLocator
    {
        public static readonly IServer Server = new DummyServer();
        public static readonly INotificator Notificator = new Notificator();
    }
}