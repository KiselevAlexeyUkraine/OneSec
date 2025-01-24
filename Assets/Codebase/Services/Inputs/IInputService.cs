namespace Codebase.Services.Inputs
{
    public interface IInputService
    {
        public bool Left { get; }
        public bool Right { get; }
        public bool Jump { get; }
        public bool Slide { get; }
        public bool Fire { get; }
        public bool Escape { get; }
    }
}