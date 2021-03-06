using System;
using System.Reflection;
using HotChocolate.Types;

namespace HotChocolate.Configuration
{
    internal class FieldBinding
    {
        public FieldBinding(string name, MemberInfo member, ObjectField field)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Name = name;
            Member = member ?? throw new ArgumentNullException(nameof(member));
            Field = field ?? throw new ArgumentNullException(nameof(field));
        }

        public string Name { get; }
        public MemberInfo Member { get; }
        public ObjectField Field { get; }
    }
}
