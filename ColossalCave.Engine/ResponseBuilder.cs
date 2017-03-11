using System.Text;
using ColossalCave.Engine.Interfaces;

namespace ColossalCave.Engine
{
    public class ResponseBuilder : IResponseBuilder
    {
        private IMessageProvider _messageProvider;

        private StringBuilder _speechBuffer;
        private StringBuilder _textBuffer;

        public string Speech
        {
            get
            {
                var speech = _speechBuffer.ToString();
                if (speech.Contains("</") || speech.Contains("/>"))
                    speech = "<speech>" + speech + "</speech";
                return speech;
            }
        }

        public string Text
        {
            get { return _textBuffer.ToString(); }
        }

        public ResponseBuilder(IMessageProvider messageProvider)
        {
            _messageProvider = messageProvider;
            _speechBuffer = new StringBuilder();
            _textBuffer = new StringBuilder();
        }

        public void AddToResponse(string speech, string text = null)
        {
            _speechBuffer.Append(speech);
            _textBuffer.Append(text ?? speech);
        }

        public void AddToResponse(string speech, int pause, string text = null)
        {
            _speechBuffer.Append(speech);
            if (pause > 0)
            {
                _speechBuffer.Append($"<break time='{pause}s'/>");
                _textBuffer.Append("\n");
            }
            _textBuffer.Append(text ?? speech);
        }

        public void AddToResponse(int messageId, int pause = 0)
        {
            var msg = _messageProvider.GetMessage(messageId);
            AddToResponse(msg.SpeechOrText, pause, msg.TextOrSpeech);
        }

        public void AddToResponse(Mnemonic mn, int pause = 0)
        {
            AddToResponse((int)mn, pause);
        }

        public void PrefixResponse(string speech, string text = null)
        {
            _speechBuffer.Insert(0, speech);
            _textBuffer.Insert(0, text ?? speech);
        }

        public void PrefixResponse(string speech, int pause, string text = null)
        {
            if (pause > 0)
            {
                _speechBuffer.Insert(0, $"<break time='{pause}s'/>");
                _textBuffer.Insert(0, "\n");
            }
            _speechBuffer.Insert(0, speech);
            _textBuffer.Insert(0, text ?? speech);
        }

        public void PrefixResponse(int messageId, int pause = 0)
        {
            var msg = _messageProvider.GetMessage(messageId);
            PrefixResponse(msg.SpeechOrText, pause, msg.TextOrSpeech);
        }

        public void PrefixResponse(Mnemonic mn, int pause = 0)
        {
            PrefixResponse((int)mn, pause);
        }

        public void ReplaceResponse(string speech, string text = null)
        {
            _speechBuffer.Clear();
            _textBuffer.Clear();
            AddToResponse(speech, text);
        }

        public void ReplaceResponse(int messageId)
        {
            var msg = _messageProvider.GetMessage(messageId);
            ReplaceResponse(msg.SpeechOrText, msg.TextOrSpeech);
        }

        public void ReplaceResponse(Mnemonic mn)
        {
            ReplaceResponse((int)mn);
        }
    }
}
