using System.Diagnostics;

class DiagramGenerator
{
    static void Main(string[] args)
    {
        // Ruta al archivo PlantUML y al .jar
        string plantUmlJarPath = "plantuml.jar"; // Debe estar en el mismo directorio o especificar ruta completa
        string inputFile = "diagrama.puml";      // Archivo PlantUML generado
        string outputDir = ".";                  // Directorio de salida

        var process = new Process
        {
            StartInfo = new ProcessStartInfo
            {
                FileName = "java",
                Arguments = $"-jar {plantUmlJarPath} -tpng {inputFile} -o {outputDir}",
                RedirectStandardOutput = true,
                UseShellExecute = false,
                CreateNoWindow = true,
            }
        };
        process.Start();
        process.WaitForExit();
        System.Console.WriteLine("Imagen generada en el directorio actual.");
    }
}
