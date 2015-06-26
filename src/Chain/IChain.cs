namespace AG.Framework.Chain
{
    public interface IChain : ICommand
    {
        void AddCommand(ICommand command);
    }
}