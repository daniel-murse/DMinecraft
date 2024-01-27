namespace DMinecraft.PhysicalClient.Graphics.OpenGL.GLObjects.Data.Programs
{
    internal class LinkedProgramInterface
    {
        public LinkedProgramInterface()
        {
        }

        public LinkedProgramInterface(InputInterface input, OutputInterface output, UniformInterface uniform, SubroutineInterface subroutine)
        {
            Input = input;
            Output = output;
            Uniform = uniform;
            Subroutine = subroutine;
        }

        public required InputInterface Input { get; init; }

        public required OutputInterface Output { get; init; }

        public required UniformInterface Uniform { get; init; }

        public required SubroutineInterface Subroutine { get; init; }
    }
}