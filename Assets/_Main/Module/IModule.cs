using Constant;

namespace Module
{
    public interface IModule
    {
        public Enums.Modules ModuleKey { get; }
        public bool ModuleStatus { get; set; }
    }
}