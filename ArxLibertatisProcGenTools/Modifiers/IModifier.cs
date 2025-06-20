using ArxLibertatisEditorIO.WellDoneIO;

namespace ArxLibertatisProcGenTools.Modifiers
{
    public interface IModifier
    {
        void Apply(WellDoneArxLevel wdl);
    }
}
