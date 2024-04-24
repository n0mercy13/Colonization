using System.Collections.Generic;
using VContainer.Unity;

namespace Codebase.Infrastructure
{
    public partial class Bootstrap
    {
        private readonly IReadOnlyList<IInitializable> _initializables;

        public Bootstrap(IReadOnlyList<IInitializable> initializables)
        {
            _initializables = initializables;
        }
    }

    public partial class Bootstrap : IStartable
    {
        public void Start()
        {
            foreach (IInitializable initializable in _initializables)
                initializable.Initialize();
        }
    }
}
