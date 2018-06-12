namespace Shop.Domain
{
    public class DMarketGameTokenResponce
    {
        public string GameToken;
        public string RefreshToken;
        public string ErrorText;

        public DMarketGameTokenResponce(string gameToken, string refreshToken)
        {
            GameToken = gameToken;
            RefreshToken = refreshToken;
            ErrorText = string.Empty;
        }

        public DMarketGameTokenResponce()
        { }
    }
}
