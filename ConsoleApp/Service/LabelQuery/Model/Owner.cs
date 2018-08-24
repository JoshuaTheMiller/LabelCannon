using System.Collections.Generic;
using System.Linq;

namespace Service.LabelQuery.Model
{
    public sealed class Owner
    {
        private readonly IList<Repository> repositories;

        public Owner(IEnumerable<Repository> repositories, string name)
        {
            this.repositories = repositories.ToList();
            Name = name;
        }

        public void AddRepository(Repository repository)
        {
            this.repositories.Add(repository);
        }

        public IEnumerable<Repository> Repositories => repositories;
        public string Name { get; }
    }
}
