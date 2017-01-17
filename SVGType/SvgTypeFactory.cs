using PaintDotNet;

namespace SVGType
{
    public class SvgTypeFactory : IFileTypeFactory
    {
        public FileType[] GetFileTypeInstances()
        {
            return new FileType[]
            {
                new SvgType()
            };
        }
    }
}
