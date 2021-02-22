using System;
using System.IO;
using System.Text.Json;

namespace GenerateSpec
{
    public static class Program
    {
        static void Main(string[] args)
        {
            if (args.Length <= 0) {
                Console.Error.WriteLine("Missing spec_path argument");
                return;
            }
            string specPath = args[0];
            string specData = File.ReadAllText(specPath);
            SpecData spec;
            try {
                spec = JsonSerializer.Deserialize<SpecData>(specData);
            } catch (JsonException e) {
                Console.Error.WriteLine(e.ToString());
                return;
            }

            string outputPath = "";
            if (args.Length > 1) {
                outputPath = args[1];
            }

            Generate(new GenerateMd(), spec, outputPath);
            Generate(new GenerateCs(), spec, outputPath);
        }

        private static void Generate(Generator generator, SpecData spec, string outputPath)
        {
            string filename = generator.GetOutputName();
            string data = generator.Generate(spec);
            File.WriteAllText(Path.Join(outputPath, filename), data);
        }
    }
}
