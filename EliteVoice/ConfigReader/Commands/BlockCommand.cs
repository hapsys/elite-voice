using Saxon.Api;

namespace EliteVoice.ConfigReader.Commands
{
    class BlockCommand : AbstractCommand
    {
        public override int runCommand(XdmNode node)
        {
            runChilds(node);
            return 0;
        }
    }
}
