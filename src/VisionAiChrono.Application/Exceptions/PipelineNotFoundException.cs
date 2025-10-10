namespace VisionAiChrono.Application.Exceptions
{
    public class PipelineNotFoundException : Exception
    {
        public PipelineNotFoundException(string message): base(message)
        {
        }
    }
}
