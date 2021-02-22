namespace GenerateSpec
{
    public abstract class Generator
    {
        public abstract string GetOutputName();

        public abstract string Generate(SpecData spec);
    }
}
