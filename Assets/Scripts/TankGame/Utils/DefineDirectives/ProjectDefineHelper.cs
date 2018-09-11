namespace TankGame.DefineDirectives
{
    public static class ProjectDefineHelper
    {
        public static BuildTypeDirective BuildDirective
        {
            get
            {
#if DEVELOP_MODE
                return BuildTypeDirective.Develop;
#elif RELEASE_MODE
                return BuildTypeDirective.Release;
#else
                return BuildTypeDirective.None;
#endif
            }
        }

        public static EnvironmentDirective TargetEnvironment
        {
            get { return EnvironmentDirective.ProductionSandbox; }
        }
    }
}