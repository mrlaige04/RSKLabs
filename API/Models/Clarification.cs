namespace API.Models
{
    public class Clarification
    {
        public IEnumerable<Set> Sets { get; set; }

        public IEnumerable<Group> Groups { get; set; }

        public IEnumerable<Group> ClarifyGroups()
        {
            var clarifiedGroups = new List<Group>();

            for (int i = 0; i < Groups.Count(); i++)
            {
                clarifiedGroups.Add(Groups.ElementAt(i));
                ClarifyGroup(Groups.ElementAt(i), Groups.Except(clarifiedGroups));
                Groups.ElementAt(i).Sets = Groups.ElementAt(i).Sets.Distinct().OrderBy(s => s.Code);
            }

            

            return Groups;
        }

        private int ClarifyGroup(Group group, IEnumerable<Group> groups)
        {
            var deletedGroups = new List<Group>();

            while (true)
            {
                if (!TryAbsorbGroup(group, groups.Except(deletedGroups), out var deletedGroup))
                {
                    break;
                }

                if (deletedGroup != null)
                {
                    deletedGroups.Add(deletedGroup);
                }
            }

            Groups = Groups.Except(deletedGroups).ToList();

            foreach (var gr in groups)
            {
                TryAbsorbSet(group, gr);
            }

            return deletedGroups.Count;
        }

        private bool TryAbsorbGroup(Group group, IEnumerable<Group> groups, out Group deletedGroup)
        {
            deletedGroup = null;

            foreach (var gr in groups)
            {
                if (Included(group.Operations, gr.Operations))
                {
                    deletedGroup = gr;
                    group.AddSets(gr.Sets);
                    return true;
                }
            }

            return false;
        }

        private bool TryAbsorbSet(Group group, Group gr)
        {
            foreach (var set in gr.Sets)
            {
                if (Included(group.Operations, set.Operations))
                {
                    group.Sets = group.Sets.Append(set);
                    gr.Sets = gr.Sets.Except(new List<Set> { set });
                    return true;
                }
            }

            return false;
        }

        private bool Included(IEnumerable<string> g1, IEnumerable<string> g2)
        {
            return g2.All(g => g1.Contains(g));
        }
    }

    public class Set
    {
        public int Code { get; set; }

        public IEnumerable<string> Operations { get; set; }
    }

    public class Group
    {
        public int Code { get; set; }

        public IEnumerable<Set> Sets { get; set; }

        public IEnumerable<string> Operations => Sets.SelectMany(set => set.Operations).DistinctBy(s => s.ToLower());

        public void AddSets(IEnumerable<Set> sets)
        {
            foreach (var set in sets)
            {
                Sets = Sets.Append(set);
            }
        }
    }
}
