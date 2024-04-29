using System.Collections.Generic;
using VContainer.Unity;

namespace Codebase.Infrastructure
{
    public partial class Bootstrap
    {
        private readonly IReadOnlyList<IInitialize> _initializables;

        public Bootstrap(IReadOnlyList<IInitialize> initializables)
        {
            _initializables = initializables;
        }
    }

    public partial class Bootstrap : IStartable
    {
        public void Start()
        {
            foreach (IInitialize initializable in _initializables)
                initializable.Initialize();
        }
    }
}
