
public class BuildParameters
{
    const string _SolutionFile = "./Cake.ProGet.sln";

    public BuildParameters(ICakeContext context) { }   

    public string SolutionFile { get; set; }

    public string Configuration { get; set; }

    public string Target { get; set; }

    public static BuildParameters Load(ICakeContext context, BuildSystem buildSystem)
    {
        if (context == null) throw new ArgumentNullException("context");
        if (buildSystem == null) throw new ArgumentNullException("buildSystem");

        return new BuildParameters(context)
        {
            SolutionFile = _SolutionFile,
            Target = context.Argument("Target", "Default"),
            Configuration = context.Argument("Configuration", "Debug")         
        };
    }
}