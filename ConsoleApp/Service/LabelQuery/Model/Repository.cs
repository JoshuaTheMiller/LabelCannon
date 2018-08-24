using System.Collections.Generic;
using System.Linq;

namespace Service.LabelQuery.Model
{
    public sealed class Repository
    {
        private readonly IList<Label> labels;

        public Repository(string name, IEnumerable<Label> labels)
        {
            Name = name;            
            this.labels = labels.ToList();
        }

        public void AddLabel(Label label)
        {
            this.labels.Add(label);
        }

        public string Name { get; }        
        public IEnumerable<Label> Labels => labels;
    }
}