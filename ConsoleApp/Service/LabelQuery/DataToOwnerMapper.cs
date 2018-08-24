using System.Collections.Generic;
using System.Linq;
using Service.LabelQuery.Model;
using Service.LabelQuery.RepositoryQueryResponse;
using Service.Query;
using Owner = Service.LabelQuery.Model.Owner;

namespace Service.LabelQuery
{
    public sealed class DataToOwnerMapper : IMapper<Data, Owner>
    {
        public Owner MapData(Data data)
        {
            var reposToMap = data.Organization.Repositories.Edges.Select(edge => edge.Node).ToList();

            var mappedRepos = MapRepos(reposToMap);

            var owner = new Owner(mappedRepos, data.Organization.Name);

            return owner;
        }

        private IEnumerable<Repository> MapRepos(List<Node> reposToMap)
        {
            return reposToMap.Select(repo => new Repository(repo.Name, MapLabels(repo.Labels))).ToList();
        }

        private IEnumerable<Label> MapLabels(Labels currentLabelPage)
        {
            return currentLabelPage.Edges.Select(edge => edge.Node).Select(node => new Label(node.Name, node.Description, node.Color)).ToList();
        }
    }
}
