using PaintDotNet;

namespace SvgFileTypePlugin
{
    public class SvgFileTypeFactory : IFileTypeFactory
    {
        public FileType[] GetFileTypeInstances()
        {
            return new FileType[]
            {
                new SvgFileType()
            };
        }
    }
}
