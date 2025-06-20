namespace ArxLibertatisProcGenTools.Generators.Texture
{
    [Description("Returns just a fixed texture")]
    public class SingleTexture : ITextureGenerator
    {
        private readonly string path;

        public SingleTexture(string path)
        {
            this.path = path;
        }

        public string GetTexturePath(int polygonIndex)
        {
            return path;
        }
    }
}
