

namespace UnityPlugin.Decoder 
{
    public class DeckLinkDecoder : MediaDecoder
    {
        private DeckLinkInput input = null;
        protected DeckLinkInput Input
        {
            get {
                if (null == input)
                {
                    input = gameObject.AddComponent<DeckLinkInput>();
                }
                return input;
            }
        }

        protected void Awake() 
        {
            input = gameObject.AddComponent<DeckLinkInput>();
        }

        public override void initDecoder(string path)
        {
            string indexStr = path.Replace(InputSource.URL_DECKLINK, string.Empty).Replace("index=", string.Empty);
            int idx = 0;
            if (int.TryParse(indexStr, out idx)) {
                Input._deviceIndex = idx;
            }
            base.onInitComplete.Invoke();
        }

        public override void setPause()
        {
            Input.Pause();
        }

        public override void setResume()
        {
            Input.Unpause();
        }

        public override void startDecoding()
        {
            Input.Begin(true);
        }

        public override void stopDecoding()
        {
            Input.StopInput();
        }

         public override Texture GetResult()
        {
            return Input.OutputTexture;
        }

    }
}