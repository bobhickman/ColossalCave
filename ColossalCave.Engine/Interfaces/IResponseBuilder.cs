namespace ColossalCave.Engine.Interfaces
{
    public interface IResponseBuilder
    {
        string Text { get; }
        string Speech { get; }

        void AddToResponse(string speech, string text = null);

        void AddToResponse(string speech, int pause, string text = null);

        void AddToResponse(int messageId, int pause = 0);

        void PrefixResponse(string speech, string text = null);

        void PrefixResponse(string speech, int pause, string text = null);

        void PrefixResponse(int messageId, int pause = 0);

        void ReplaceResponse(string speech, string text = null);

        void ReplaceResponse(int messageId);
    }
}
