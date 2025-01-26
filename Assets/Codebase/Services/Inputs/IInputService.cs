namespace Codebase.Services.Inputs
{
    public interface IInputService
    {
        public float Horizontal { get; }
        public bool Jump { get; }
        public bool Slide { get; }
        public bool Fire { get; }
        public bool Escape { get; }
    }
}