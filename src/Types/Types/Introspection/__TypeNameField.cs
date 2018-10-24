namespace HotChocolate.Types.Introspection
{
    internal sealed class __TypeNameField
        : ObjectField
    {
        internal __TypeNameField()
           : base(IntrospectionFieldNames.TypeName, d =>
           {
               d.Description("The name of the current Object type at runtime.")
                   .Type<NonNullType<StringType>>()
                   .Resolver(ctx => ctx.ObjectType.Name);
           })
        {
        }

        public override bool IsIntrospectionField { get; } = true;
    }
}
